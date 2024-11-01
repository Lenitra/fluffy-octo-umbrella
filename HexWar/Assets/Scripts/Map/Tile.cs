using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;



public class Tile : MonoBehaviour
{
    // private with getters and setters
    public int units = 0;
    public string owner = "";
    public int[] position = new int[2];
    public string type = "";
    public TextMeshPro tileInfos;



    private float selectionOffset = 0.8f;

    // textmeshpro for info on the tile
    [SerializeField] private GameObject toShowOnSelected;
    [SerializeField] private GameObject hoverOwner;

    public GameObject moreInfo;

    [Header("Prefabs")]
    [SerializeField] private GameObject hqPrefab;
    [SerializeField] private GameObject moneyPrefab;
    [SerializeField] private GameObject radarPrefab;
    [SerializeField] private GameObject barrackPrefab;
    

    private GameObject infrastrucutre;



    void Start()
    {
        // name of the object is "Hexagon x, z"
        position[0] = int.Parse(gameObject.name.Split("Hexagon ")[1].Split(":")[0]);
        position[1] = int.Parse(gameObject.name.Split("Hexagon ")[1].Split(":")[1]);

        
        toShowOnSelected.gameObject.SetActive(false);


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
        float r = float.Parse(PlayerPrefs.GetString("color").Split('|')[0])/255;
        float g = float.Parse(PlayerPrefs.GetString("color").Split('|')[1])/255;
        float b = float.Parse(PlayerPrefs.GetString("color").Split('|')[2])/255;
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


        // switch pour configurer l'objet à instancier en fonction du type en minuscule
        Destroy(infrastrucutre);
        infrastrucutre = null;

        switch (type.ToLower().Split(":")[0])
        {
            case "hq":
                infrastrucutre = Instantiate(hqPrefab, Vector3.zero, Quaternion.identity, transform);
                infrastrucutre.transform.localPosition = Vector3.zero;
                break;

            case "money":
                infrastrucutre = Instantiate(moneyPrefab, Vector3.zero, Quaternion.identity, transform);
                infrastrucutre.transform.localPosition = Vector3.zero;
                break;

            case "radar":
                infrastrucutre = Instantiate(radarPrefab, Vector3.zero, Quaternion.identity, transform);
                infrastrucutre.transform.localPosition = Vector3.zero;
                break;

            case "barrack":
                infrastrucutre = Instantiate(barrackPrefab, Vector3.zero, Quaternion.identity, transform);
                infrastrucutre.transform.localPosition = Vector3.zero;
                break;

            case "defensive":
                // Ajouter des actions spécifiques pour "defensive" si nécessaire
                break;

            case "offensive":
                // Ajouter des actions spécifiques pour "offensive" si nécessaire
                break;

            case "":
                break;

            default:
                Debug.LogWarning("Type non reconnu : " + type);
                break;
        }



    }



    public void setupTile(int units, string owner, string type) {
        this.units = units;
        this.owner = owner;
        this.type = type;
        if (owner == ""){
            tileInfos.text = "<sprite=91>" + units;
        } else {
            tileInfos.text = "<sprite=112>" + owner + "\n" + "<sprite=91>" + units;
        }
        Start();
    }


    public IEnumerator selectTile()
    {
        toShowOnSelected.gameObject.SetActive(true);
        float animationDuration = 0.25f; // Durée de l'animation en secondes
        float elapsedTime = 0f;
        float startPositionY = transform.position.y;
        float targetPositionY = startPositionY + selectionOffset;
        
        while (elapsedTime < animationDuration)
        {
            float moveStep = (selectionOffset / animationDuration) * Time.deltaTime; // Déplacement par frame basé sur le temps
            transform.position = new Vector3(transform.position.x, transform.position.y + moveStep, transform.position.z);
            
            elapsedTime += Time.deltaTime;
            yield return null; // Attend jusqu'au prochain frame
        }
        
        // Assure que la position finale est atteinte
        transform.position = new Vector3(transform.position.x, targetPositionY, transform.position.z);
        yield return new WaitForSeconds(0.1f); // Attente après l'animation
    }

    public IEnumerator unselectTile(GameObject tile)
    {
        toShowOnSelected.gameObject.SetActive(false);
        float animationDuration = 0.25f; // Durée de l'animation en secondes
        float elapsedTime = 0f;
        float startPositionY = transform.position.y;
        float targetPositionY = startPositionY - selectionOffset;

        while (elapsedTime < animationDuration)
        {
            float moveStep = (selectionOffset / animationDuration) * Time.deltaTime; // Déplacement par frame basé sur le temps
            transform.position = new Vector3(transform.position.x, transform.position.y - moveStep, transform.position.z);
            
            elapsedTime += Time.deltaTime;
            yield return null; // Attend jusqu'au prochain frame
        }

        // Assure que la position finale est atteinte
        transform.position = new Vector3(transform.position.x, targetPositionY, transform.position.z);
        yield return new WaitForSeconds(0.1f); // Attente après l'animation
    }





}
