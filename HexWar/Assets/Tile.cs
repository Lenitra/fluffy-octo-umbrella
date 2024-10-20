using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;



public class Tile : MonoBehaviour
{
    // private with getters and setters
    public int units { get; set; } = 0;
    public string owner { get; set; } = "";
    public bool hq { get; set; } = false;
    public int[] position { get; set; } = new int[2];
    public string type { get; set; } = "";



    public GameObject hqObject;
    private float selectionOffset = 0.8f;

    // textmeshpro for info on the tile
    [SerializeField] private GameObject toShowOnSelected;
    [SerializeField] private TextMeshPro tileInfo;
    [SerializeField] private GameObject hoverOwner;
    



    void Start()
    {
        // name of the object is "Hexagon x, z"
        position[0] = int.Parse(gameObject.name.Split("Hexagon ")[1].Split(":")[0]);
        position[1] = int.Parse(gameObject.name.Split("Hexagon ")[1].Split(":")[1]);

        

        // set the text of the tileInfo
        toShowOnSelected.gameObject.SetActive(false);
        string msg;
        msg = "<sprite=36> " + position[0] + ":" + position[1] + "\n";
        if (owner != "") {
            msg += "<sprite=112>" + owner + "\n";
        }
        
        msg += "<sprite=91>" + units + "\n";
        if (type != "")
            msg += "<sprite=45>" + type.Split(":")[0] + " " + type.Split(":")[1];
        tileInfo.text = msg;

        // add a material to the hoverOwner
        // set renderinmode to transparent
        Material material = hoverOwner.GetComponent<Renderer>().material;
        material.SetFloat("_Mode", 2);
        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        material.SetInt("_ZWrite", 0);
        material.DisableKeyword("_ALPHATEST_ON");
        material.EnableKeyword("_ALPHABLEND_ON");
        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;

        // set the color of the hoverOwner
        float r = float.Parse(PlayerPrefs.GetString("color").Split(',')[0])/255;
        float g = float.Parse(PlayerPrefs.GetString("color").Split(',')[1])/255;
        float b = float.Parse(PlayerPrefs.GetString("color").Split(',')[2])/255;
        float a = 20f / 255f;
        material.color = new Color(r,g,b,a); 



        //  if the owner is the same as the username from playerprefs, show the hoverOwner 
        if (owner == PlayerPrefs.GetString("username"))
        {
            hoverOwner.SetActive(true);
        }
        else
        {
            hoverOwner.SetActive(false);
        }

        if(type.Split(":")[0] == "HQ") {
            hq = true;
            hqObject.SetActive(true);
        }
        else {
            hq = false;
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
        toShowOnSelected.gameObject.SetActive(true);
        float movePerFrame = selectionOffset / 10;
        for (int i = 0; i < 10; i++)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + movePerFrame, transform.position.z);
            yield return new WaitForSeconds(0.001f);
        }
        yield return new WaitForSeconds(0.1f);
    }

    public IEnumerator unselectTile(GameObject tile)
    {
        toShowOnSelected.gameObject.SetActive(false);
        float movePerFrame = selectionOffset / 10;
        for (int i = 0; i < 10; i++)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - movePerFrame, transform.position.z);
            yield return new WaitForSeconds(0.001f);
        }
        yield return new WaitForSeconds(0.1f);
    }




}
