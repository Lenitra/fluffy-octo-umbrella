using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GridGenerator gridGenerator;
    [SerializeField] private CamControler camControler;
    private ServerClient serverClient;

    private string username = "Lenitra";

    private void OnEnable()
    {
        serverClient = FindObjectOfType<ServerClient>();
        ServerClient.OnGameDataReceived += SetupTiles; // Abonnement de l'événement

    }

    private void OnDisable()
    {
        ServerClient.OnGameDataReceived -= SetupTiles; // Désabonnement de l'événement
    }

    public void SetupTiles(string jsonData)
    {
        // Désérialiser le JSON dans une structure adaptée à JsonUtility
        GameData gameData = JsonUtility.FromJson<GameData>(jsonData);

        // Vérification que les données de jeu ne soient pas nulles
        if (gameData == null || gameData.hexes == null || gameData.hexes.Length == 0)
        {
            Debug.LogError("Erreur: les données du jeu sont nulles ou vides.");
            return;
        }

        // Créer un dictionnaire à partir des hexes pour faciliter la manipulation
        Dictionary<string, HexData> hexDictionary = gameData.hexes
            .ToDictionary(hex => hex.key, hex => (HexData)hex);

        // Créer une liste des données d'hexagones
        List<Dictionary<string, object>> tilesData = hexDictionary.Values
            .Where(hex => hex != null) // S'assurer que hex n'est pas null
            .Select(hex => hex.ToDictionary())
            .ToList();

        // Vérification de la taille de la liste
        if (tilesData.Count == 0)
        {
            Debug.LogWarning("Aucun hex trouvé dans les données reçues.");
        }

        // Mettre à jour la grille
        gridGenerator.UpdateGrid(tilesData);
    }


    public void moveUnitsBtnClic(string origin , string destination, int units = 5)
    {
        Debug.Log("Move units from " + origin + " to " + destination + " with " + units + " units.");
        // send a http request to the server
        serverClient.moveUnits(origin, destination, units);
    }

}






// Classe GameData pour contenir le tableau des hexagones
[Serializable]
public class GameData
{
    public HexData[] hexes;  // Tableau d'hexagones
}

[Serializable]
public class HexData
{
    public string key;
    public int units;
    public string type;
    public string owner;

    // La conversion en dictionnaire
    public Dictionary<string, object> ToDictionary()
    {
        var dict = new Dictionary<string, object>
        {
            {"type", type},
            {"owner", owner},
            {"units", units},
            {"key", key}
        };
        return dict;
    }
}
