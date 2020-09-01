using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_AdvancedInfoController : MonoBehaviour
{
    private UI_AdvancedInfoDisplay advancedInfoDisplay;
    private MapUIInfo mapUIInfo;
    private Cursor cursor;
    private Stats currentUnitStats;

    private void Start()
    {
        advancedInfoDisplay = GetComponentInChildren<UI_AdvancedInfoDisplay>(true);
        advancedInfoDisplay.Init();
        advancedInfoDisplay.gameObject.SetActive(false);
        mapUIInfo = GetComponentInParent<MapUIInfo>();
        cursor = GameObject.Find("Cursor").GetComponent<Cursor>();
    }

    private void Update()
    {
        //TODO this
        if (advancedInfoDisplay.gameObject.activeInHierarchy && (Input.GetKeyDown(KeyCode.C) || !cursor.GetCurrentUnit() ||
            cursor.GetCurrentUnit().GetComponent<Stats>() != currentUnitStats) || !mapUIInfo.mapManager.CheckUnitStates(UnitStates.UNSELECTED))
        {
            advancedInfoDisplay.gameObject.SetActive(false);

        }
        else if(!advancedInfoDisplay.gameObject.activeInHierarchy && cursor.GetCurrentUnit() != null && Input.GetKeyDown(KeyCode.C)
            && mapUIInfo.mapManager.CheckUnitStates(UnitStates.UNSELECTED))
        {
            currentUnitStats = cursor.GetCurrentUnit().GetComponent<Stats>();
            advancedInfoDisplay.gameObject.SetActive(true);
            advancedInfoDisplay.UpdateStatsText(currentUnitStats.GetBattleStats());
            advancedInfoDisplay.UpdateInventory(currentUnitStats.inventory);
            advancedInfoDisplay.UpdateBasicInfo(currentUnitStats);
            advancedInfoDisplay.UpdateMagic(currentUnitStats.blackMagic, currentUnitStats.whiteMagic);
            advancedInfoDisplay.UpdateWeaponSkillExperience(currentUnitStats.skillLevels);
        }

    }

    public void UpdateInfo()
    {

    }
}
