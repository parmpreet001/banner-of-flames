using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//This script contains basic info that will be used by other elements of 
public class MapUIInfo : MonoBehaviour
{
    public GameObject MapAndBattleManager;
    public GameObject SelectedAllyUnit;
    public GameObject HoveringUnit;
    private Cursor Cursor;
    public BattleManager BattleManager;

    public AllyStats SelectedAllyUnit_AllyStats;
    public AllyMove SelectedAllyUnit_AllyMove;

    private void Start()
    {
        Cursor = GameObject.Find("Cursor").GetComponent<Cursor>();
        BattleManager = MapAndBattleManager.GetComponent<BattleManager>();
    }

    private void Update()
    {
        HoveringUnit = Cursor.GetCurrentUnit();
        if (HoveringUnit && HoveringUnit.GetComponent<TileMove>().selected)
        {
            SelectedAllyUnit = HoveringUnit;
            SelectedAllyUnit_AllyStats = SelectedAllyUnit.GetComponent<AllyStats>();
            SelectedAllyUnit_AllyMove = SelectedAllyUnit.GetComponent<AllyMove>();
        }
            
        if (SelectedAllyUnit && (!SelectedAllyUnit.GetComponent<AllyMove>().selected || SelectedAllyUnit.GetComponent<AllyMove>().finished))
            SelectedAllyUnit = null;
    }
}
