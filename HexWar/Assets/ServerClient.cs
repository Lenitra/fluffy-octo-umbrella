using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class ServerClient : MonoBehaviour
{
    public static event Action<string> OnGameDataReceived; // Déclaration de l'événement

    private float pollInterval = 5.0f; // Interval en secondes
    private string gameServerURL = "http://localhost:5000/get_hex/"+ "Lenitra" + "/" + "2"; // URL du serveur de jeu
    // private string gameServerURL = "http://localhost:5000/get_all_map"; // URL du serveur de jeu

    void Start()
    {
        StartCoroutine(PollGameState());
    }

    IEnumerator PollGameState()
    {
        while (true)
        {
            Debug.Log("Polling game state...");
            yield return new WaitForSeconds(pollInterval);
            StartCoroutine(GetGameState());
        }
    }

    IEnumerator GetGameState()
    {
        UnityWebRequest request = UnityWebRequest.Get(gameServerURL);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error: " + request.error);
        }
        else
        {
            Debug.Log("Received: " + request.downloadHandler.text);
            OnGameDataReceived?.Invoke(request.downloadHandler.text); // Lancement de l'événement avec les données JSON
        }
    }

    
    
    public void moveUnits(string from, string to, int units)
    {
        StartCoroutine(MoveUnitsCoro(from, to, units));
    }
    
    IEnumerator MoveUnitsCoro(string from, string to, int units)
    {
        UnityWebRequest request = UnityWebRequest.Get("http://localhost:5000/move_units/" + from + "/" + to + "/" + units);
        yield return request.SendWebRequest();
        // debug the response
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error: " + request.error);
        }
        else
        {
            Debug.Log("Received: " + request.downloadHandler.text);
        }
    }
    
    
    void OnApplicationQuit()
    {
        StopAllCoroutines();
    }


}
