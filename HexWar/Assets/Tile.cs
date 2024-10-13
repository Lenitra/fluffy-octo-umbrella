using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    
    public int units = 0;
    public string owner = "none";
    public bool hq = false;
    public int[] position = new int[2];
    public string type = "";


    public GameObject hqObject;
    private int selectionOffset = 2;



    void Start()
    {
        // name of the object is "Hexagon x, z"
        position[0] = int.Parse(gameObject.name.Split("Hexagon ")[1].Split(",")[0]);
        position[1] = int.Parse(gameObject.name.Split("Hexagon ")[1].Split(",")[1]);

        if(type.Split(":")[0] == "HQ") {
            hq = true;
        } else {
            hq = false;
        }



        if (hq) {
            hqObject.SetActive(true);
        } else {
            hqObject.SetActive(false);
        }
    }

    public void setType(string type) {
        this.type = type;
        if (type.Split(":")[0] == "HQ") {
            hq = true;
        } else {
            hq = false;
        }
        if (hq) {
            hqObject.SetActive(true);
        } else {
            hqObject.SetActive(false);
        }
    }

    public void setupTile(int units, string owner, string type) {
        this.units = units;
        this.owner = owner;
        this.type = type;
        Start();
    }


    public IEnumerator selectTile()
    {

        for (int i = 0; i < selectionOffset * 10; i++)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + 0.01f, transform.position.z);
            yield return new WaitForSeconds(0.001f);
        }
        yield return new WaitForSeconds(0.1f);
    }

    public IEnumerator unselectTile(GameObject tile)
    {
        for (int i = 0; i < selectionOffset * 10; i++)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - 0.01f, transform.position.z);
            yield return new WaitForSeconds(0.001f);
        }
        yield return new WaitForSeconds(0.1f);
    }

}
