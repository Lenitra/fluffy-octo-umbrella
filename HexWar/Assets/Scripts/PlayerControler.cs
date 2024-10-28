using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class PlayerControler : MonoBehaviour
{

    // Ajouter de l'UI pour afficher les informations des hexagones sélectionnés
    [SerializeField] private RectTransform tileInfoPanel;
    [SerializeField] private GameObject movePanel;
    [SerializeField] private Button moveUnitsBtn;
    [SerializeField] private Button buildBtn;



    private int selectedUnits = 0;
    private GridGenerator gridGenerator;
    private GameManager gameManager;
    private CamControler camControler;

    private float timeClicked = 0;


    private string state = "";

    private GameObject selectedTile;
    // Update is called once per frame


    void Start(){

        gridGenerator = GetComponent<GridGenerator>();
        gameManager = GetComponent<GameManager>();
        camControler = Camera.main.GetComponent<CamControler>();


        // add event listener to moveUnitsBtn
        moveUnitsBtn.onClick.AddListener(moveUnitsBtnClic);
        // set playerpref username 

        // TODO: Pour débug
        PlayerPrefs.SetString("username", "Lenitra");
        PlayerPrefs.SetString("color", "105, 5, 133");


    }


    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            timeClicked += Time.deltaTime;
        }




        // lancer de raycast
        if (Input.GetMouseButtonUp(0))
        {
            
            if (timeClicked < 0.4f) {
            
                // check if the click is on a UI element
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    return; 
                }

                if (state == ""){
                    // move units
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit))
                    {



                        if (selectedTile == null)
                        {
                            selectedTile = hit.collider.gameObject;
                            camControler.lookTile(selectedTile);
                            StartCoroutine(selectedTile.GetComponent<Tile>().selectTile());
                            StartCoroutine(animateTileInfoPanel());

                        } else {
                            StartCoroutine(selectedTile.GetComponent<Tile>().unselectTile(selectedTile));
                            StartCoroutine(animateTileInfoPanelBack());
                            if (hit.collider.gameObject == selectedTile){
                                selectedTile = null;
                                return;
                            } else {
                                selectedTile = hit.collider.gameObject;
                                StartCoroutine(selectedTile.GetComponent<Tile>().selectTile());
                                camControler.lookTile(selectedTile);
                                StartCoroutine(animateTileInfoPanel());
                            }
                        }
                    }
                }

                else if (state == "move"){
                    // move units
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.collider.gameObject.tag == "Tile"){
                            gameManager.moveUnitsBtnClic(selectedTile.name.Split(' ')[1], hit.collider.gameObject.name.Split(' ')[1], selectedUnits);
                            state = "";
                        }
                    }
                }

            }   
            timeClicked = 0;
        }
    }


    private void moveUnitsBtnClic(){
        if (selectedTile != null){
            if (selectedTile.GetComponent<Tile>().units > 0){
                state = "move";
                movePanel.SetActive(true);
            }
        }
    }

    

    // annimation : translate the panel to the right
    private IEnumerator animateTileInfoPanel(){
        // translate the panel to the right 
        for (int i = 0; i < 10; i++)
        {
            tileInfoPanel.position = new Vector3(tileInfoPanel.position.x + 20, tileInfoPanel.position.y, tileInfoPanel.position.z);
            yield return new WaitForSeconds(0.001f);
        }
    }


    // annimation : translate the panel to the left
    private IEnumerator animateTileInfoPanelBack(){
        // translate the panel to the right
        for (int i = 0; i < 10; i++)
        {
            tileInfoPanel.position = new Vector3(tileInfoPanel.position.x - 20, tileInfoPanel.position.y, tileInfoPanel.position.z);
            yield return new WaitForSeconds(0.001f);
        }
    }

}
