using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class CamControler : MonoBehaviour
{
    private float moveSpeed = 400f;       // Vitesse de déplacement de la caméra
    private float zoomSpeed = 1000f;       // Vitesse de zoom avec la molette
    private float minZoom = 5f;          // Limite minimale du zoom
    private float maxZoom = 60f;         // Limite maximale du zoom
    private float smoothTime = 0.2f;     // Temps de lissage pour les mouvements et zooms

    private Vector3 velocity = Vector3.zero;  // Vitesse pour le SmoothDamp
    private Coroutine moveCoroutine = null;   // Coroutine pour l'animation de translation

    void LateUpdate()
    {
        HandleMovement();
        HandleZoom();
    }

    void HandleMovement()
    {           
        // check if the click is on a UI element
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return; 
        }
        if (Input.GetMouseButton(0))  // Vérifie si le bouton gauche de la souris est enfoncé
        {

            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            // Crée un vecteur de direction basé sur le mouvement de la souris
            Vector3 direction = new Vector3(-mouseX, 0f, -mouseY).normalized;

            // Calcule la position cible en fonction du mouvement de la souris
            Vector3 targetPosition = transform.position + direction * moveSpeed * Time.deltaTime;

            // Lissage du mouvement
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }
    }

    void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.01f)
        {
            float targetFov = Camera.main.fieldOfView - scroll * zoomSpeed;
            targetFov = Mathf.Clamp(targetFov, minZoom, maxZoom);

            // Interpolation du zoom pour un effet de lissage
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, targetFov, Time.deltaTime / smoothTime);
        }
    }

    public void lookTile(GameObject tile)
    {
        Vector3 tilePos = tile.transform.position;

        // Arrêter la coroutine en cours si elle existe
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        moveCoroutine = StartCoroutine(TranslateToTile(tilePos));
    }

    IEnumerator TranslateToTile(Vector3 tilePos)
    {
        Vector3 targetPos = new Vector3(tilePos.x, transform.position.y, tilePos.z - 3);
        float elapsed = 0f;

        while (Vector3.Distance(transform.position, targetPos) > 0.01f)
        {
            transform.position = Vector3.Lerp(transform.position, targetPos, elapsed / smoothTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos; // Ajustement final pour une position précise
    }

    
}

