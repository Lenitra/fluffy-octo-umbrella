using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerControler : MonoBehaviour
{

    // TODO: Ajouter de l'UI pour afficher les informations des hexagones sélectionnés
    [SerializeField] private TextMeshProUGUI tileInfo;
    [SerializeField] private RectTransform tileInfoPanel;


    [SerializeField] private GridGenerator gridGenerator;



    private GameObject selectedTile;
    // Update is called once per frame
    void Update()
    {
        // lancer de raycast
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {



                if (selectedTile == null)
                {
                    selectedTile = hit.collider.gameObject;
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
                        StartCoroutine(animateTileInfoPanel());
                    }
                }
            }

            if (selectedTile != null){
                string msg = "NAME : " + selectedTile.name;
                msg += "\nPOSITION : " + selectedTile.transform.position;
                msg += "\nCalculated : " + gridGenerator.GetHexCoordinates(selectedTile.GetComponent<Tile>().position[0], selectedTile.GetComponent<Tile>().position[1])[0] + ", " + gridGenerator.GetHexCoordinates(selectedTile.GetComponent<Tile>().position[0], selectedTile.GetComponent<Tile>().position[1])[1];
                msg += "\nTAG : " + selectedTile.GetComponent<Tile>().position[0] + ", " + selectedTile.GetComponent<Tile>().position[1];
                msg += "\nUNITS : " + selectedTile.GetComponent<Tile>().units;
                msg += "\nOWNER : " + selectedTile.GetComponent<Tile>().owner;
                msg += "\nHQ : " + selectedTile.GetComponent<Tile>().hq;
                msg += "\nTYPE : " + selectedTile.GetComponent<Tile>().type;
                tileInfo.text = msg;
            }

        }
    }



    private IEnumerator animateTileInfoPanel(){
        // translate the panel to the right 
        for (int i = 0; i < 10; i++)
        {
            tileInfoPanel.position = new Vector3(tileInfoPanel.position.x + 20, tileInfoPanel.position.y, tileInfoPanel.position.z);
            yield return new WaitForSeconds(0.001f);
        }
    }

    private IEnumerator animateTileInfoPanelBack(){
        // translate the panel to the right
        for (int i = 0; i < 10; i++)
        {
            tileInfoPanel.position = new Vector3(tileInfoPanel.position.x - 20, tileInfoPanel.position.y, tileInfoPanel.position.z);
            yield return new WaitForSeconds(0.001f);
        }
    }

}
