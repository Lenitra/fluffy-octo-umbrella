using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    
    [SerializeField] private int units = 0;
    [SerializeField] private string owner = "none";
    [SerializeField] private bool hq = false;
    [SerializeField] private int[] position = new int[2];


    [SerializeField] private GameObject hqObject;


    void Start()
    {
        // name of the object is "Hexagon x, z"
        position[0] = int.Parse(gameObject.name.Split("Hexagon ")[1].Split(",")[0]);
        position[1] = int.Parse(gameObject.name.Split("Hexagon ")[1].Split(",")[1]);


        // TODO: TEST
        if (position[0] == 0 && position[1] == 0) {
            hq = true;
        }


        if (hq) {
            hqObject.SetActive(true);
        } else {
            hqObject.SetActive(false);
        }
        
    }
}
