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
    public GameObject cursor;

    public AllyStats selectedAllyUnitStats;
    // Update is called once per frame
    private void Update()
    {
        hoveringUnit = cursor.GetComponent<Cursor>().GetCurrentUnit();
        if (hoveringUnit && hoveringUnit.GetComponent<TileMove>().selected)
        {
            selectedAllyUnit = hoveringUnit;
            selectedAllyUnitStats = selectedAllyUnit.GetComponent<AllyStats>();
        }
            
        if (selectedAllyUnit && (!selectedAllyUnit.GetComponent<AllyMove>().selected || selectedAllyUnit.GetComponent<AllyMove>().finished))
            selectedAllyUnit = null;
    }
}
