using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_AdvancedInfoController : MonoBehaviour
{
    private UI_AdvancedInfoDisplay advancedInfoDisplay;
    private MapUIInfo mapUIInfo;
    private Cursor cursor;
    private AllyStats allyStats;

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
        if (advancedInfoDisplay.gameObject.activeInHierarchy && (Input.GetKeyDown(KeyCode.C) || !cursor.CurrentTileHasAllyUnit()))
        {
            advancedInfoDisplay.gameObject.SetActive(false);

        }
        else if(!advancedInfoDisplay.gameObject.activeInHierarchy && cursor.CurrentTileHasAllyUnit() && Input.GetKeyDown(KeyCode.C))
        {
            allyStats = cursor.GetCurrentUnit().GetComponent<AllyStats>();
            advancedInfoDisplay.gameObject.SetActive(true);
            advancedInfoDisplay.UpdateStatsText(allyStats.GetBattleStats());
            advancedInfoDisplay.UpdateInventory(allyStats.inventory);
            advancedInfoDisplay.UpdateBasicInfo(allyStats);
            advancedInfoDisplay.UpdateMagic(allyStats.blackMagic, allyStats.whiteMagic);
            advancedInfoDisplay.UpdateWeaponSkillExperience(allyStats.skillLevels);
        }

    }

    public void UpdateInfo()
    {

    }
}
