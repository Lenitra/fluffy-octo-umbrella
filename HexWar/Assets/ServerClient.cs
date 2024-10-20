using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class ServerClient : MonoBehaviour
{
    private GameManager gameManager;

    public static event Action<string> OnGameDataReceived; // Déclaration de l'événement

    private float pollInterval = 5.0f; // Interval en secondes
    private string gameServerURL = "http://localhost:5000/get_hex/"+ "Lenitra"; // URL du serveur de jeu

    void Start()
    {
        gameManager = GetComponent<GameManager>();
        StartCoroutine(PollGameState());
    }

    IEnumerator PollGameState()
    {
        while (true)
        {
            Debug.Log("Polling game state...");
            StartCoroutine(GetGameState());
            yield return new WaitForSeconds(pollInterval);
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
            if (request.downloadHandler.text != "NOPE")
            {
                // we recieve a list of moves
                string tmp = request.downloadHandler.text;
                // delete all [ and ]
                tmp = tmp.Replace("[", "");
                tmp = tmp.Replace("]", "");
                tmp = tmp.Replace("\"", "");

                string[] moves = tmp.Split(',');

                tmp = "";
                // debug inside the array
                foreach (string move in moves)
                {
                    tmp += move + " ";
                }
                Debug.Log("Moves: " + tmp);

                StartCoroutine(gameManager.moveUnitsAnimation(moves));
            }
        }
    }
    
    
    void OnApplicationQuit()
    {
        StopAllCoroutines();
    }


}
