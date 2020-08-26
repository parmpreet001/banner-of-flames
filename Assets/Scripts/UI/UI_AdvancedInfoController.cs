using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_AdvancedInfoController : MonoBehaviour
{
    private UI_AdvancedInfoDisplay advancedInfoDisplay;
    private MapUIInfo mapUIInfo;

    private void Start()
    {
        advancedInfoDisplay = GetComponentInChildren<UI_AdvancedInfoDisplay>();
        mapUIInfo = GetComponentInParent<MapUIInfo>();
    }

    private void Update()
    {
        if(mapUIInfo.selectedAllyUnit)
        {
            advancedInfoDisplay.gameObject.SetActive(true);
            advancedInfoDisplay.UpdateStatsText(mapUIInfo.selectedAllyUnit_AllyStats.GetBattleStats());
            advancedInfoDisplay.UpdateInventory(mapUIInfo.selectedAllyUnit_AllyStats.inventory);
            advancedInfoDisplay.UpdateBasicInfo(mapUIInfo.selectedAllyUnit_AllyStats);
        }
        else
        {
            advancedInfoDisplay.gameObject.SetActive(false);
        }
    }

    public void UpdateInfo()
    {

    }
}
