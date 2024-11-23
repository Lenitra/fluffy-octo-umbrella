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

    private Coroutine activeCoroutine; // Référence à la coroutine active

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
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit))
                    {


                        if (PlayerPrefs.GetString("username") == hit.collider.gameObject.GetComponent<Tile>().owner){

                            if (selectedTile == null)
                            {
                                selectedTile = hit.collider.gameObject;
                                camControler.lookTile(selectedTile);
                                selectedTile.GetComponent<Tile>().select();
                                StartAnimatingTileInfoPanel(true);
                            } 
                            
                            else {
                                selectedTile.GetComponent<Tile>().unselect();
                                StartAnimatingTileInfoPanel(false);
                                if (hit.collider.gameObject == selectedTile){
                                    selectedTile = null;
                                    return;
                                } else {
                                    selectedTile = hit.collider.gameObject;
                                    selectedTile.GetComponent<Tile>().select();
                                    camControler.lookTile(selectedTile);
                                    StartAnimatingTileInfoPanel(true);
                                }
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
                            selectedTile.GetComponent<Tile>().unselect();
                            StartAnimatingTileInfoPanel(false);
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

            // Si la tile est vide
            if(selectedTile.GetComponent<Tile>().type == ""){
                buildPanel.gameObject.SetActive(true);
            }

            // Si il y a un batiment au niveau inférieur à 5
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
    



    private IEnumerator animateTileInfoPanel()
    {
        // Position cible en X relative (par rapport à l'ancrage)
        float targetX = 100f;

        // Récupération du RectTransform
        RectTransform rectTransform = tileInfoPanel.GetComponent<RectTransform>();

        // Tant que la position relative actuelle est inférieure à la cible
        while (rectTransform.anchoredPosition.x < targetX)
        {
            // Lerp pour une transition fluide
            rectTransform.anchoredPosition = Vector2.Lerp(
                rectTransform.anchoredPosition, 
                new Vector2(targetX, rectTransform.anchoredPosition.y), 
                Time.deltaTime * 10 // Ajustez la vitesse
            );

            // Vérification de proximité pour éviter une boucle infinie
            if (Mathf.Abs(rectTransform.anchoredPosition.x - targetX) < 0.01f)
            {
                rectTransform.anchoredPosition = new Vector2(targetX, rectTransform.anchoredPosition.y);
                break;
            }

            yield return new WaitForEndOfFrame();
        }

        // La coroutine est terminée, réinitialisation de la référence
        activeCoroutine = null;
    }

    private IEnumerator animateTileInfoPanelBack()
    {
        // Position cible en X relative (par rapport à l'ancrage)
        float targetX = -100f;

        // Récupération du RectTransform
        RectTransform rectTransform = tileInfoPanel.GetComponent<RectTransform>();

        // Tant que la position relative actuelle est supérieure à la cible
        while (rectTransform.anchoredPosition.x > targetX)
        {
            // Lerp pour une transition fluide
            rectTransform.anchoredPosition = Vector2.Lerp(
                rectTransform.anchoredPosition, 
                new Vector2(targetX, rectTransform.anchoredPosition.y), 
                Time.deltaTime * 10 // Ajustez la vitesse
            );

            // Vérification de proximité pour éviter une boucle infinie
            if (Mathf.Abs(rectTransform.anchoredPosition.x - targetX) < 0.01f)
            {
                rectTransform.anchoredPosition = new Vector2(targetX, rectTransform.anchoredPosition.y);
                break;
            }

            yield return new WaitForEndOfFrame();
        }

        // La coroutine est terminée, réinitialisation de la référence
        activeCoroutine = null;
    }

    public void StartAnimatingTileInfoPanel(bool moveForward)
    {
        // Désactiver le bouton de construction si la tile est lvl 5
        if (selectedTile.GetComponent<Tile>().type.EndsWith("5")){
            buildBtn.interactable = false;
        }
        else {
            buildBtn.interactable = true;
        }

        // Si une coroutine est déjà active, on l'arrête
        if (activeCoroutine != null)
        {
            StopCoroutine(activeCoroutine);
            activeCoroutine = null;
        }

        // Démarrer la nouvelle coroutine
        if (moveForward)
        {
            activeCoroutine = StartCoroutine(animateTileInfoPanel());
        }
        else
        {
            activeCoroutine = StartCoroutine(animateTileInfoPanelBack());
        }
    }




}