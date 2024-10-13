using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamControler : MonoBehaviour
{
    public float moveSpeed = 10f;   // Vitesse de déplacement de la caméra
    public float zoomSpeed = 15f;   // Vitesse de zoom avec la molette
    public float minZoom = 5f;      // Limite minimale du zoom
    public float maxZoom = 60f;     // Limite maximale du zoom

    void FixedUpdate()
    {
        HandleMovement();
        HandleZoom();
    }

    // Gestion du mouvement horizontal et vertical de la caméra
    void HandleMovement()
    {
        // Obtenir les entrées des axes
        float horizontal = Input.GetAxis("Horizontal");  // Par défaut sur les touches A/D ou flèches gauche/droite
        float vertical = Input.GetAxis("Vertical");      // Par défaut sur les touches W/S ou flèches haut/bas

        // Calcul du déplacement
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        // Appliquer le déplacement à la caméra
        if (direction.magnitude >= 0.1f)
        {
            transform.position += direction * moveSpeed * Time.deltaTime;
        }
    }

    // Gestion du zoom avec la molette de la souris
    void HandleZoom()
    {
        // Obtenir l'entrée de la molette de la souris
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        // Si on détecte un défilement
        if (scroll != 0f)
        {
            // Calcul du nouveau champ de vision (zoom) de la caméra
            float newFov = Camera.main.fieldOfView - scroll * zoomSpeed;

            // Limiter le champ de vision entre minZoom et maxZoom
            Camera.main.fieldOfView = Mathf.Clamp(newFov, minZoom, maxZoom);
        }
    }

    public void lookTile(GameObject tile){
        Vector3 tilePos = tile.transform.position;
        transform.position = new Vector3(tilePos.x, transform.position.y, tilePos.z-4);
    }
}