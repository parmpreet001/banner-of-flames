using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//This script contains basic info that will be used by other elements of 
public class MapUIInfo : MonoBehaviour
{
    public GameObject selectedAllyUnit;
    public GameObject hoveringUnit;
    private Cursor cursor;
    public BattleManager battleManager;
    public MapManager mapManager;
    public TileController tileController;

    public AllyStats selectedAllyUnit_AllyStats;
    public AllyMove selectedAllyUnit_AllyMove;

    private void Start()
    {
        cursor = GameObject.Find("Cursor").GetComponent<Cursor>();
        battleManager = GameObject.Find("MapAndBattleManager").GetComponent<BattleManager>();
        mapManager = GameObject.Find("MapAndBattleManager").GetComponent<MapManager>();
        tileController = GameObject.Find("MapAndBattleManager").GetComponent<TileController>();
    }

    private void Update()
    {
        hoveringUnit = cursor.GetCurrentUnit();
        if (mapManager.selectedUnit && mapManager.selectedUnit.tag == "PlayerUnit")
        {
            selectedAllyUnit = mapManager.selectedUnit;
            selectedAllyUnit_AllyStats = mapManager.selectedUnit.GetComponent<AllyStats>();
        }
            
        else
            selectedAllyUnit = null;
    }
}
