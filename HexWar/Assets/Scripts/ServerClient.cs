using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System;
using UnityEngine.SceneManagement;

public class ServerClient : MonoBehaviour
{
    private GameManager gameManager;
    private float pollInterval = 2.5f; // Interval en secondes


    void Start()
    {
        gameManager = GetComponent<GameManager>();
        StartCoroutine(PollGameState());
    }

    // Boucle de call au serveur pour récupérer l'état du jeu
    IEnumerator PollGameState()
    {
        while (true)
        {
            StartCoroutine(GetGameState());
            yield return new WaitForSeconds(pollInterval);
        }
    }


    public void updateMap()
    {
        // lancer la coroutine dans 1 seconde
        Invoke("GetGameState", 0.25f);
    }

    IEnumerator GetGameState()
    {
        float startTime = Time.time;
        UnityWebRequest request = UnityWebRequest.Get("http://localhost:5000/get_hex/"+ PlayerPrefs.GetString("username"));
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error: " + request.error);
        }
        else
        {
            // if request.downloadHandler.text start with "error" then we have an error
            if (request.downloadHandler.text.ToLower().StartsWith("error : "))
            {
                // change the scene to the login scene
                SceneManager.LoadScene("Home");
                Debug.LogError("error: " + request.downloadHandler.text);
            }
            else
            {
                gameManager.SetupTiles(request.downloadHandler.text);
            }
        }
        Debug.Log("PollGameState took: " + (Time.time - startTime) + " seconds");
        gameManager.seeAllUnits();
        gameManager.seeAllUnits();
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
            if (request.downloadHandler.text.ToLower().StartsWith("error : "))
            {
                // change the scene to the login scene
                SceneManager.LoadScene("Home");
                Debug.LogError("error: " + request.downloadHandler.text);
            }

            else
            {
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

                    StartCoroutine(gameManager.moveUnitsAnimation(moves));
                }
            }
        }

    }
    
    
    void OnApplicationQuit()
    {
        StopAllCoroutines();
    }


}
