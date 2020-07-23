using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_TileInfoController : MonoBehaviour
{
    private MapUIInfo mapUIInfo;
    private UI_TileInfoDisplay tileInfoDisplay;
    private Cursor cursor;

    private void Start()
    {
        tileInfoDisplay = transform.GetChild(0).GetComponent<UI_TileInfoDisplay>();
        mapUIInfo = GetComponentInParent<MapUIInfo>();
        cursor = GameObject.Find("Cursor").GetComponent<Cursor>();
    }

    private void Update()
    {
        if(mapUIInfo.mapManager.unitState == UnitStates.ATTACKING || !mapUIInfo.mapManager.playerPhase)
        {
            SetTileInfoActive(false);
        }
        else
        {
            SetTileInfoActive(true);
            if(cursor.currentTile)
            {
                tileInfoDisplay.SetTileName(cursor.currentTile.terrainEffect.name);
                tileInfoDisplay.SetTileInfo(cursor.currentTile.terrainEffect.description);
            }
        }


    }

    private void SetTileInfoActive(bool active)
    {
        tileInfoDisplay.gameObject.SetActive(active);
    }
}
