using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradePanel : MonoBehaviour
{

    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text description;
    [SerializeField] private Button buildBtn;
    [SerializeField] private PlayerControler playerControler;
    [SerializeField] private Button closeBtn;
    private Tile selectedTile;

    void Start()
    {

        closeBtn.onClick.AddListener(() => gameObject.SetActive(false));

        // buildBtn.onClick.AddListener(() => playerControler.getFromBuildPanel(buildings[currentBuilding].GetComponent<BuildUI>().type));
    }
    
    public void Initialise(Tile selectedTile)
    {
        this.selectedTile = selectedTile;
        
        Debug.Log("build : " + selectedTile.type.Split(':')[0]);
        
        string build = selectedTile.type.Split(':')[0];
        int lvl = int.Parse(selectedTile.type.Split(':')[1]);
        string completedesc = build.ToLower() + "Infos" + (lvl + 1);
        title.text = DataManager.Instance.GetData(build.ToLower());
        description.text = DataManager.Instance.GetData(completedesc);


    }
}
