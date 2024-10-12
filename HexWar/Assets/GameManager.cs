using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class GameManager : MonoBehaviour
{
    private string username = "Lenitra";

    [SerializeField] private GridGenerator gridGenerator;
    [SerializeField] private CamControler camControler;

    private List<Dictionary<string, object>> tilesData = new List<Dictionary<string, object>>();

    private string jsonTest = @"
    {
        ""centerTile"" : {
            ""x"" : 1,
            ""z"" : 1
        },
        ""hexes"" : [
            {
                ""x"" : 0,
                ""z"" : 0,
                ""type"" : ""HQ:1"",
                ""owner"" : ""Lenitra"",
                ""units"" : 100
            },
            {
                ""x"" : 1,
                ""z"" : 0,
                ""type"" : """",
                ""owner"" : """",
                ""units"" : 0
            },
            {
                ""x"" : 2,
                ""z"" : 0,
                ""type"" : """",
                ""owner"" : """",
                ""units"" : 0
            },
            {
                ""x"" : 0,
                ""z"" : 1,
                ""type"" : """",
                ""owner"" : """",
                ""units"" : 0
            },
            {
                ""x"" : 1,
                ""z"" : 1,
                ""type"" : """",
                ""owner"" : """",
                ""units"" : 0
            },
            {
                ""x"" : 2,
                ""z"" : 1,
                ""type"" : """",
                ""owner"" : """",
                ""units"" : 0
            },
            {
                ""x"" : 0,
                ""z"" : 2,
                ""type"" : """",
                ""owner"" : """",
                ""units"" : 0
            },
            {
                ""x"" : 1,
                ""z"" : 2,
                ""type"" : ""HQ:1"",
                ""owner"" : ""Tamer"",
                ""units"" : 0
            },
            {
                ""x"" : 2,
                ""z"" : 2,
                ""type"" : """",
                ""owner"" : """",
                ""units"" : 0
            }
        ]
    }";

    void Start()
    {
        SetupTiles();
    }


    public void SetupTiles(){
        GameData gameData = JsonUtility.FromJson<GameData>(jsonTest);
        tilesData = gameData.hexes.Select(hex => hex.ToDictionary()).ToList();
        
        gridGenerator.GenerateTiles(tilesData);

        // Récupération des coordonnées du centerTile
        int centerTileX = gameData.centerTile.x;
        int centerTileZ = gameData.centerTile.z;

        GameObject centerTile = GameObject.Find("Hexagon " + centerTileX + ", " + centerTileZ);
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
    public CenterTileData centerTile;  // Ajout de la propriété centerTile
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
    public int z;
    public string type;
    public string owner;
    public int units;
    public bool hq;

    public Dictionary<string, object> ToDictionary()
    {
        return new Dictionary<string, object>
        {
            {"x", x},
            {"z", z},
            {"type", type},
            {"owner", owner},
            {"units", units},
            {"hq", hq}
        };
    }
}
