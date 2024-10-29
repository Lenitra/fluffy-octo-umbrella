using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;

public class LoginSys : MonoBehaviour
{

    [SerializeField] private InputField LOGINusernameInput;
    [SerializeField] private InputField LOGINpasswordInput;
    [SerializeField] private GameObject LOGINloading;
    [SerializeField] private Button LOGINbutton;


    void Start()
    {
        LOGINbutton.onClick.AddListener(() => StartLogin(LOGINusernameInput.text, LOGINpasswordInput.text));
    }


    // Méthode pour démarrer le processus de connexion
    public void StartLogin(string username, string password)
    {   
        LOGINloading.gameObject.SetActive(true);
        StartCoroutine(Login(username, password));
    }

    // Coroutine pour envoyer la requête POST au serveur Flask
    IEnumerator Login(string username, string password)
    {
        string url = "http://localhost:5000/login";
        
        // Création des données JSON à envoyer
        string jsonData = "{\"username\":\"" + username + "\",\"password\":\"" + password + "\"}";
        byte[] postData = System.Text.Encoding.UTF8.GetBytes(jsonData);
        
        // Configuration de la requête UnityWebRequest
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        request.uploadHandler = new UploadHandlerRaw(postData);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        
        // Envoi de la requête et attente de la réponse
        yield return request.SendWebRequest();
        
        // Gestion des erreurs de connexion
        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Erreur de connexion: " + request.error);
        }
        else
        {
            // Traitement de la réponse du serveur
            string responseText = request.downloadHandler.text;
            Debug.Log("Réponse du serveur: " + responseText);
            LOGINloading.gameObject.SetActive(false);
            if (responseText == "OK")
            {
                // Connexion réussie
                Debug.Log("Connexion réussie");
                // ajouter le username dans les PlayerPrefs
                PlayerPrefs.SetString("username", username);
                // changement de scene
                UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
            }
            else
            {
                // Connexion échouée
                Debug.Log("Connexion échouée");
                StartCoroutine(shakeUI());
            }
        }
    }
    
    IEnumerator shakeUI()
    {
        LOGINpasswordInput.text = "";
        Vector3 initialPos = LOGINbutton.GetComponent<RectTransform>().localPosition; // Correction de la déclaration
        RectTransform rectTransform = LOGINbutton.GetComponent<RectTransform>(); // Stockage du RectTransform pour éviter de le rappeler plusieurs fois

        for (int i = 0; i < 7; i++)
        {
            rectTransform.localPosition = initialPos + new Vector3(5, 0, 0);
            yield return new WaitForSeconds(0.05f);
            rectTransform.localPosition = initialPos - new Vector3(5, 0, 0); // Ajout d'un déplacement inverse pour l'effet de shake
            yield return new WaitForSeconds(0.05f);
        }

        rectTransform.localPosition = initialPos;
    }

}
