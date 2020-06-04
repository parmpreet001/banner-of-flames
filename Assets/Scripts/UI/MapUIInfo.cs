using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//This script contains basic info that will be used by other elements of 
public class MapUIInfo : MonoBehaviour
{
    public GameObject selectedAllyUnit;
    public GameObject HoveringUnit;
    private Cursor Cursor;
    public BattleManager BattleManager;

    public AllyStats SelectedAllyUnit_AllyStats;
    public AllyMove SelectedAllyUnit_AllyMove;

    private void Start()
    {
        Cursor = GameObject.Find("Cursor").GetComponent<Cursor>();
        BattleManager = GameObject.Find("MapAndBattleManager").GetComponent<BattleManager>();
    }

    private void Update()
    {
        HoveringUnit = Cursor.GetCurrentUnit();
        if (HoveringUnit && HoveringUnit.GetComponent<TileMove>().selected)
        {
            selectedAllyUnit = HoveringUnit;
            SelectedAllyUnit_AllyStats = selectedAllyUnit.GetComponent<AllyStats>();
            SelectedAllyUnit_AllyMove = selectedAllyUnit.GetComponent<AllyMove>();
        }
            
        if (selectedAllyUnit && (!selectedAllyUnit.GetComponent<AllyMove>().selected || selectedAllyUnit.GetComponent<AllyMove>().finished))
            selectedAllyUnit = null;
    }
}
