using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : MonoBehaviour
{

    // TODO: Ajouter de l'UI pour afficher les informations des hexagones sélectionnés





    private GameObject selectedTile;
    int offset = 2;

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

                Debug.Log(hit.collider.gameObject.name);


                if (selectedTile == null)
                {
                    selectedTile = hit.collider.gameObject;
                    StartCoroutine(selectTile(selectedTile));


                } else {
                    StartCoroutine(unselectTile(selectedTile));
                    if (hit.collider.gameObject == selectedTile){
                        selectedTile = null;
                        return;
                    } else {
                        selectedTile = hit.collider.gameObject;
                        StartCoroutine(selectTile(selectedTile));
                    }
                }
            }
        }
    }



    IEnumerator selectTile(GameObject tile)
    {

        for (int i = 0; i < offset*10; i++)
        {
            tile.transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y + 0.01f, tile.transform.position.z);
            yield return new WaitForSeconds(0.001f);
        }
        yield return new WaitForSeconds(0.1f);
    }

    IEnumerator unselectTile(GameObject tile)
    {
        for (int i = 0; i < offset*10; i++)
        {
            tile.transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y - 0.01f, tile.transform.position.z);
            yield return new WaitForSeconds(0.001f);
        }
        yield return new WaitForSeconds(0.1f);
    }
}
