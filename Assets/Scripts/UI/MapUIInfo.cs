using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//This script contains basic info that will be used by other elements of 
public class MapUIInfo : MonoBehaviour
{
    public GameObject mapAndBattleManager;
    public GameObject selectedAllyUnit;
    public GameObject hoveringUnit;
    private Cursor cursor;
    public BattleManager _BattleManager;

    public AllyStats selectedAllyUnit_AllyStats;
    public AllyMove selectedAllyUnit_AllyMove;

    private void Start()
    {
        cursor = GameObject.Find("Cursor").GetComponent<Cursor>();
        _BattleManager = mapAndBattleManager.GetComponent<BattleManager>();
    }

    private void Update()
    {
        hoveringUnit = cursor.GetCurrentUnit();
        if (hoveringUnit && hoveringUnit.GetComponent<TileMove>().selected)
        {
            selectedAllyUnit = hoveringUnit;
            selectedAllyUnit_AllyStats = selectedAllyUnit.GetComponent<AllyStats>();
            selectedAllyUnit_AllyMove = selectedAllyUnit.GetComponent<AllyMove>();
        }
            
        if (selectedAllyUnit && (!selectedAllyUnit.GetComponent<AllyMove>().selected || selectedAllyUnit.GetComponent<AllyMove>().finished))
            selectedAllyUnit = null;
    }
}
