using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GridGenerator gridGenerator;
    [SerializeField] private CamControler camControler;

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
        GameData gameData = JsonUtility.FromJson<GameData>(jsonData);
        List<Dictionary<string, object>> tilesData = gameData.hexes.Select(hex => hex.ToDictionary()).ToList();
        
        gridGenerator.GenerateTiles(tilesData);

        // Récupération des coordonnées du centreTile
        int centerTileX = gameData.centerTile.x;
        int centerTileZ = gameData.centerTile.z;

        GameObject centerTile = GameObject.Find($"Hexagon {centerTileX}, {centerTileZ}");
        if (centerTile != null)
        {
            camControler.lookTile(centerTile);
        }
    }
}

// Classes auxiliaires pour analyser les données JSON
[Serializable]
public class GameData
{
    public CenterTileData centerTile;  // Propriété centerTile
    public List<HexData> hexes;
}

[Serializable]
public class CenterTileData
{
    public int x;
    public int z;  
}

[Serializable]
public class HexData
{
    public int x;
    public int z;  // Correspond à 'z' et non 'y'
    public string type;
    public string owner;
    public int units;

    public Dictionary<string, object> ToDictionary()
    {
        var dict = new Dictionary<string, object>
        {
            {"x", x},
            {"z", z},
            {"type", type},
            {"owner", owner},
            {"units", units}
        };
        return dict;
    }
}
