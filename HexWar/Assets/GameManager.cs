using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.Linq;
using System;
using TMPro;

public class GameManager : MonoBehaviour
{
    private GridGenerator gridGenerator;
    private CamControler camControler;
    private ServerClient serverClient;

    private string username = "Lenitra";


    // EFFECTS
    [SerializeField] private LineRenderer moveUnitsLine;


    void Start(){
        // get the 
        serverClient = GetComponent<ServerClient>();
        gridGenerator = GetComponent<GridGenerator>();
        camControler = Camera.main.GetComponent<CamControler>();
    }

    private void OnEnable()
    {
        
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



public IEnumerator moveUnitsAnimation(string from, string to)
{
    // get the origin and destination hexagons
    GameObject originHex = gridGenerator.getHex(int.Parse(from.Split(":")[0]), int.Parse(from.Split(":")[1]));
    GameObject destinationHex = gridGenerator.getHex(int.Parse(to.Split(":")[0]), int.Parse(to.Split(":")[1]));

    // get the origin and destination positions
    Vector3 originPos = new Vector3(originHex.transform.position.x, originHex.transform.position.y + 0.25f, originHex.transform.position.z);
    Vector3 destinationPos = new Vector3(destinationHex.transform.position.x, destinationHex.transform.position.y + 0.25f, destinationHex.transform.position.z);

    // Set the initial position of the line
    moveUnitsLine.positionCount = 2;
    moveUnitsLine.SetPosition(0, originPos);
    moveUnitsLine.SetPosition(1, originPos); // Start with both points at the origin

    float duration = 1.0f; // time to draw and erase the line (1 second each)
    float elapsed = 0f;

    // Animate the line drawing over the duration
    while (elapsed < duration)
    {
        elapsed += Time.deltaTime;
        float t = Mathf.Clamp01(elapsed / duration); // t goes from 0 to 1 over the duration
        Vector3 currentPos = Vector3.Lerp(originPos, destinationPos, t); // Interpolate position
        moveUnitsLine.SetPosition(1, currentPos); // Update the end position of the line

        yield return null; // Wait until the next frame
    }

    // Ensure the line is fully drawn at the end
    moveUnitsLine.SetPosition(1, destinationPos);


    // Reset the elapsed time for the fade out
    elapsed = 0f;

    // Animate the line fading out progressively from origin to destination
    while (elapsed < duration)
    {
        elapsed += Time.deltaTime;
        float t = Mathf.Clamp01(elapsed / duration); // t goes from 0 to 1 over the duration
        Vector3 currentPos = Vector3.Lerp(originPos, destinationPos, t); // Interpolate position from origin to destination
        moveUnitsLine.SetPosition(0, currentPos); // Update the start position of the line progressively towards the destination

        yield return null; // Wait until the next frame
    }

    // Ensure the line is fully erased at the end
    moveUnitsLine.SetPosition(0, destinationPos);

    // Remove the line completely
    moveUnitsLine.positionCount = 0;
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
