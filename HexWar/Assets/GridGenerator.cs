using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{

    public GameObject hexPrefab;
    public int gridSize = 16;
    public float hexSize = 0.5f;
    public float animmYOffset = 1f; 
    public float gridGap = 0.02f;







    public void GenerateTiles(List<Dictionary<string, object>> tilesData)
    {
        foreach (Dictionary<string, object> tileData in tilesData)
        {
            int x = (int)tileData["x"];
            int z = (int)tileData["z"];
            string owner = (string)tileData["owner"];
            int units = (int)tileData["units"];
            string type_id = (string)tileData["type"];

            GameObject hex = GameObject.Instantiate(hexPrefab);
            hex.name = "Hexagon " + x + ", " + z;
            hex.transform.SetParent(this.transform);
            hex.transform.position = new Vector3(GetHexCoordinates(x, z)[0], 0, GetHexCoordinates(x, z)[1]);
            hex.GetComponent<Tile>().setupTile(units, owner, type_id);
        }
    }


    // Permet de récupérer les coordonnées dans unity à partir de la position dans le tableau
    public float[] GetHexCoordinates(int x, int z){
        int[] coords = new int[2];
        float hexWidth = hexSize * 2f; // La largeur de l'hexagone est deux fois la taille (diamètre)
        float hexHeight = Mathf.Sqrt(3) * hexSize; // La hauteur est sqrt(3) * hexSize (environ 1.732)
        float offsetX = (hexWidth + gridGap) * 0.75f; // Décalage horizontal pour les colonnes impaires avec gridGap

        float xPos = x * offsetX;
        float zPos = z * (hexHeight + gridGap); // Prendre en compte le gridGap sur l'axe Z

        // Décaler les hexagones sur les lignes impaires
        if (x % 2 == 1) {
            zPos += (hexHeight + gridGap) / 2f; // Prendre en compte le gridGap aussi ici
        }

        return new float[] {xPos, zPos};

    }

    // IEnumerator GenerateGridCoroutine() {
    //     float hexWidth = hexSize * 2f; // La largeur de l'hexagone est deux fois la taille (diamètre)
    //     float hexHeight = Mathf.Sqrt(3) * hexSize; // La hauteur est sqrt(3) * hexSize (environ 1.732)
    //     float offsetX = (hexWidth + gridGap) * 0.75f; // Décalage horizontal pour les colonnes impaires avec gridGap

    //     for (int x = 0; x < gridSize; x++) {
    //         for (int z = 0; z < gridSize; z++) {
    //             GameObject hex = Instantiate(hexPrefab);

    //             // Déterminer le décalage pour les lignes impaires
    //             float xPos = x * offsetX;
    //             float zPos = z * (hexHeight + gridGap); // Prendre en compte le gridGap sur l'axe Z

    //             // Décaler les hexagones sur les lignes impaires
    //             if (x % 2 == 1) {
    //                 zPos += (hexHeight + gridGap) / 2f; // Prendre en compte le gridGap aussi ici
    //             }

    //             hex.transform.position = new Vector3(xPos, -animmYOffset, zPos);
    //             hex.name = "Hexagon " + x + ", " + z;
    //             hex.transform.SetParent(this.transform);
    //             StartCoroutine(AnimateHexagon(hex));

    //             // Attendre avant de passer à l'hexagone suivant
    //             yield return new WaitForSeconds(0.001f);
    //         }
    //     }
    // }


    IEnumerator AnimateHexagon(GameObject hex){
        for (int i = 0; i < animmYOffset*10+1; i++){
            hex.transform.position = new Vector3(hex.transform.position.x, hex.transform.position.y + 0.1f, hex.transform.position.z);
            yield return new WaitForSeconds(0.0001f);
        }
        for (int i = 0; i < 5; i++){
            hex.transform.position = new Vector3(hex.transform.position.x, hex.transform.position.y - 0.2f, hex.transform.position.z);
            yield return new WaitForSeconds(0.0001f);
        }
    }

        



    // void GenerateGrid() {
    //     float hexWidth = hexSize * 2f; // La largeur de l'hexagone est deux fois la taille (diamètre)
    //     float hexHeight = Mathf.Sqrt(3) * hexSize; // La hauteur est sqrt(3) * hexSize (environ 1.732)
    //     float offsetX = (hexWidth + gridGap) * 0.75f; // Décalage horizontal pour les colonnes impaires avec gridGap

    //     for (int x = 0; x < gridSize; x++) {
    //         for (int z = 0; z < gridSize; z++) {
    //             GameObject hex = Instantiate(hexPrefab);

    //             // Déterminer le décalage pour les lignes impaires
    //             float xPos = x * offsetX;
    //             float zPos = z * (hexHeight + gridGap); // Prendre en compte le gridGap sur l'axe Z

    //             // Décaler les hexagones sur les lignes impaires
    //             if (x % 2 == 1) {
    //                 zPos += (hexHeight + gridGap) / 2f; // Prendre en compte le gridGap aussi ici
    //             }

    //             hex.transform.position = new Vector3(xPos, 0, zPos);
    //             hex.name = "Hexagon " + x + ", " + z;
    //             hex.transform.SetParent(this.transform);
    //         }
    //     }
    // }





    // void Update(){
    //     // on press space, generate a new grid
    //     if (Input.GetKeyDown(KeyCode.Space)){
    //         foreach (Transform child in transform){
    //             Destroy(child.gameObject);
    //         }
    //         StartCoroutine(GenerateGridCoroutine());
    //     }
    // }


}
