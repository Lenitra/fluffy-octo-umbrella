using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class GameStatePoller : MonoBehaviour
{
    public static event Action<string> OnGameDataReceived; // Déclaration de l'événement

    private float pollInterval = 2.0f; // Interval en secondes
    private string gameServerURL = "http://localhost:5000/get_player_map";

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
}
