﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_HealForecastController : MonoBehaviour
{
    private MapUIInfo mapUIInfo;
    private UI_HealForecastDisplay healForecastDisplay;
    private Cursor cursor;
    void Start()
    {
        mapUIInfo = GetComponentInParent<MapUIInfo>();
        healForecastDisplay = transform.GetChild(0).GetComponent<UI_HealForecastDisplay>();
        cursor = GameObject.Find("Cursor").GetComponent<Cursor>();
    }

    // Update is called once per frame
    void Update()
    {
        if (mapUIInfo.mapManager.CheckUnitStates(UnitStates.FINDING_ALLY) && cursor.CurrentTileHasAllyUnit() 
            && cursor.GetCurrentUnit() != mapUIInfo.selectedAllyUnit)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            healForecastDisplay.UpdateInfo(cursor.GetCurrentUnit().GetComponent<Stats>().hp, cursor.GetCurrentUnit().GetComponent<Stats>().maxHP,
                mapUIInfo.battleManager.GetHeal(mapUIInfo.selectedAllyUnit_AllyStats));
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
