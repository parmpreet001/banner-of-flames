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

    public AllyStats SelectedAllyUnit_AllyStats;
    public AllyMove SelectedAllyUnit_AllyMove;

    private void Start()
    {
        cursor = GameObject.Find("Cursor").GetComponent<Cursor>();
        battleManager = GameObject.Find("MapAndBattleManager").GetComponent<BattleManager>();
    }

    private void Update()
    {
        hoveringUnit = cursor.GetCurrentUnit();
        if (hoveringUnit && hoveringUnit.GetComponent<TileMove>().selected)
        {
            selectedAllyUnit = hoveringUnit;
            SelectedAllyUnit_AllyStats = selectedAllyUnit.GetComponent<AllyStats>();
            SelectedAllyUnit_AllyMove = selectedAllyUnit.GetComponent<AllyMove>();
        }
            
        if (selectedAllyUnit && (!selectedAllyUnit.GetComponent<AllyMove>().selected || selectedAllyUnit.GetComponent<AllyMove>().finished))
            selectedAllyUnit = null;
    }
}
