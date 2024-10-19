using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class PlayerControler : MonoBehaviour
{

    // TODO: Ajouter de l'UI pour afficher les informations des hexagones sélectionnés
    [SerializeField] private RectTransform tileInfoPanel;

    [SerializeField] private Button moveUnitsBtn;

    [SerializeField] private GridGenerator gridGenerator;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private CamControler camControler;


    private string state = "";

    private GameObject selectedTile;
    // Update is called once per frame


    void Start(){
        // add event listener to moveUnitsBtn
        moveUnitsBtn.onClick.AddListener(moveUnitsBtnClic);
        // set playerpref username 

        // TODO: Pour débug
        PlayerPrefs.SetString("username", "Lenitra");
        PlayerPrefs.SetString("color", "255, 72, 0");


    }


    void Update()
    {
        // lancer de raycast
        if (Input.GetMouseButtonDown(0))
        {
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
                        gameManager.moveUnitsBtnClic(selectedTile.name.Split(' ')[1], hit.collider.gameObject.name.Split(' ')[1]);
                        state = "";

                        // StartCoroutine(selectedTile.GetComponent<Tile>().unselectTile(selectedTile));
                        // StartCoroutine(animateTileInfoPanelBack());

                        // selectedTile = null;
                    }
                }
            }
        }
    }


    private void moveUnitsBtnClic(){
        if (selectedTile != null){
            if (selectedTile.GetComponent<Tile>().units > 0){
                state = "move";
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
