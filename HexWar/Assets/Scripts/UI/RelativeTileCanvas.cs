using UnityEngine;

public class RelativeTileCanvas : MonoBehaviour
{

    [SerializeField] private GameObject lockPanel;
    [SerializeField] private PlayerControler playerControler;


    [SerializeField] private GameObject tileInfoPanel;
    private UltimateRadialMenu tileInfoMenu;
    private Tile selectedTile;


    void Start()
    {
        tileInfoMenu = tileInfoPanel.GetComponent<UltimateRadialMenu>();

        // Button Info
        UltimateRadialButtonInfo tileInfoBtn1 = new UltimateRadialButtonInfo();
        tileInfoMenu.RegisterButton( defaultBtnAction, tileInfoBtn1, 0 );

        // Button Move
        UltimateRadialButtonInfo tileInfoBtn2 = new UltimateRadialButtonInfo();
        tileInfoMenu.RegisterButton( moveUnitsBtnClic, tileInfoBtn2, 1 );

        // Button Close
        UltimateRadialButtonInfo tileInfoBtn3 = new UltimateRadialButtonInfo();
        tileInfoMenu.RegisterButton(playerControler.unselectTile, tileInfoBtn3, 2 );

        // Button Build
        UltimateRadialButtonInfo tileInfoBtn4 = new UltimateRadialButtonInfo();
        tileInfoMenu.RegisterButton( buildBtnClic, tileInfoBtn4, 3 );

    
    }


    void defaultBtnAction()
    {
        Debug.Log("Le bouton a été cliqué !");
    }


    private void buildBtnClic()
    {
        desactivate();
        lockPanel.SetActive(true);
        playerControler.buildBtnClic();
    }

    private void moveUnitsBtnClic()
    {
        desactivate();
        lockPanel.SetActive(true);
        playerControler.moveUnitsBtnClic();
    }


    public void lookTile(GameObject tile){
        transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y + 2f, tile.transform.position.z -0.15f);
    }

    public void activateTileInfoPanel(Tile tile){
        desactivate();
        selectedTile = tile;
        gameObject.SetActive(true);
        tileInfoPanel.SetActive(true);
        lockPanel.SetActive(true);
    }

    public void desactivate(){
        gameObject.SetActive(false);
        tileInfoPanel.SetActive(false);
        lockPanel.SetActive(false);
    }


}
