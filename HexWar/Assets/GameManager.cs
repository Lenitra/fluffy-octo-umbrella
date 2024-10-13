using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GridGenerator gridGenerator;
    [SerializeField] private CamControler camControler;

    private string username = "Lenitra";

    private void OnEnable()
    {
        GameStatePoller.OnGameDataReceived += SetupTiles; // Abonnement à l'événement
    }

    private void OnDisable()
    {
        GameStatePoller.OnGameDataReceived -= SetupTiles; // Désabonnement de l'événement
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

        // Trouver le HQ du joueur
        var hqData = hexDictionary
            .FirstOrDefault(hex => hex.Value != null && hex.Value.owner == username && hex.Value.type.Contains("HQ"));

        // Vérifier si le HQ a été trouvé
        if (!hqData.Equals(default(KeyValuePair<string, HexData>)))
        {
            // Extraire les coordonnées à partir de la clé (e.g., "16:16")
            string[] coords = hqData.Key.Split(':');
            int x = int.Parse(coords[0]);
            int z = int.Parse(coords[1]);

            // S'assurer que gridGenerator.getHex(x, z) ne retourne pas null
            var hexTile = gridGenerator.getHex(x, z);
            if (hexTile != null)
            {
                // Placer la caméra sur le HQ
                camControler.lookTile(hexTile);
            }
            else
            {
                Debug.LogError("Erreur: Impossible de trouver l'hex à la position " + x + ":" + z);
            }
        }
        else
        {
            Debug.LogWarning("HQ non trouvé pour le joueur " + username);
        }
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
