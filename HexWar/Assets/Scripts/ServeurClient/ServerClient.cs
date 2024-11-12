using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System;
using UnityEngine.SceneManagement;
using System.Linq;


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
                string hexes = request.downloadHandler.text;
                // delete characters jusqu'à la première virgule
                hexes = hexes.Substring(hexes.IndexOf(",") + 1);
                // supprimer les derniers caractères jusqu'à la dernière parenthèse fermante
                hexes = hexes.Substring(0, hexes.LastIndexOf("}") + 1);


                string money = request.downloadHandler.text;
                // garder les caractères jusqu'à la première virgule
                money = money.Substring(0, money.IndexOf(","));
                // garder uniquement les chiffres
                money = new string(money.Where(char.IsDigit).ToArray());


                gameManager.UpdateMoney(money);
                gameManager.SetupTiles(hexes);
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
    


    public void build(string tile, string type, int lvl)
    {
        StartCoroutine(BuildCoro(tile, type, lvl));
    }

    IEnumerator BuildCoro(string tile, string type, int lvl)
    {
        UnityWebRequest request = UnityWebRequest.Get("http://localhost:5000/buildbat/" + tile + "/" + type);
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
        }
    }

    
    void OnApplicationQuit()
    {
        StopAllCoroutines();
    }


}
