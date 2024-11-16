using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System;

public class PlayerControler : MonoBehaviour
{

    // Ajouter de l'UI pour afficher les informations des hexagones sélectionnés
    [SerializeField] private RectTransform tileInfoPanel;
    [SerializeField] private GameObject movePanel;
    [SerializeField] private GameObject buildPanel;
    [SerializeField] private GameObject upgradePanel;

    [SerializeField] private Button moveUnitsBtn;
    [SerializeField] private Button buildBtn;

    [SerializeField] private TextMeshProUGUI tileInfo;



    private GridGenerator gridGenerator;
    private GameManager gameManager;
    private CamController camControler;

    private Vector3 previousMousePosition;
    private float timeClicked = 0;


    private string state = "";

    private GameObject selectedTile;
    private int selectedUnits;

    public float tmpdist = 0;


    void Start(){

        gridGenerator = GetComponent<GridGenerator>();
        gameManager = GetComponent<GameManager>();
        camControler = Camera.main.GetComponent<CamController>();
        previousMousePosition = Input.mousePosition;

        // add event listener to moveUnitsBtn
        moveUnitsBtn.onClick.AddListener(moveUnitsBtnClic);
        buildBtn.onClick.AddListener(buildBtnClic);

        movePanel.gameObject.SetActive(false);
        buildPanel.gameObject.SetActive(false);
    }


    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            timeClicked += Time.deltaTime;
            tmpdist += Vector3.Distance(Input.mousePosition, previousMousePosition);
            previousMousePosition = Input.mousePosition; // Met à jour la position précédente de la souris
        }

        if (selectedTile != null){
            tileInfo.text = "<sprite=36>" + selectedTile.GetComponent<Tile>().position[0] + ":" + selectedTile.GetComponent<Tile>().position[1] + "\n<sprite=112>" + selectedTile.GetComponent<Tile>().owner + "\n" + "<sprite=91>" + selectedTile.GetComponent<Tile>().units;
        } else {
            tileInfo.text = "";
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
                            StartCoroutine(selectedTile.GetComponent<Tile>().unselectTile(selectedTile));
                            StartCoroutine(animateTileInfoPanelBack());
                            selectedTile = null;

                        }
                    }
                }

            }   
            timeClicked = 0;
            tmpdist = 0;
        }
    }


    private void buildBtnClic(){
        if (selectedTile != null){
            if(selectedTile.GetComponent<Tile>().type == ""){
                buildPanel.gameObject.SetActive(true);
            }
            else if (!selectedTile.GetComponent<Tile>().type.EndsWith("5")){
                upgradePanel.gameObject.GetComponent<UpgradePanel>().Initialise(selectedTile.GetComponent<Tile>());
                upgradePanel.gameObject.SetActive(true);
            }
        }
    }


    private void moveUnitsBtnClic(){
        if (selectedTile != null){
            if (selectedTile.GetComponent<Tile>().units > 0){
                movePanel.gameObject.SetActive(true);
                movePanel.GetComponent<MovePanel>().init(selectedTile.GetComponent<Tile>().units);
            }
        }
    }

    public void getFromMovePanel(int units){
        movePanel.gameObject.SetActive(false);
        state = "move";
        selectedUnits = units;
    }

    public void getFromBuildPanel(string type){
        buildPanel.gameObject.SetActive(false);
        gameManager.buildBtnClic(selectedTile.name.Split(' ')[1], type); 
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
