using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UI_UnitInfoController : MonoBehaviour
{
    [SerializeField]
    private MapUIInfo mapUIInfo;
    private GameObject unitInfoUI;
    private UI_UnitInfoDisplay unitInfoDisplay;    

    void Start()
    {
        unitInfoUI = transform.GetChild(0).gameObject;
        unitInfoDisplay = unitInfoUI.GetComponent<UI_UnitInfoDisplay>();
        mapUIInfo = GetComponentInParent<MapUIInfo>();

    }
    void Update()
    {
        
        {
            if (mapUIInfo.selectedAllyUnit)
            {
                unitInfoDisplay.SetAllyUnitInfoActive(true);
                UpdateAllyStatsInfoText(mapUIInfo.selectedAllyUnit);
            }
            else if (mapUIInfo.hoveringUnit && mapUIInfo.hoveringUnit.tag == "PlayerUnit")
            {
                unitInfoDisplay.SetAllyUnitInfoActive(true);
                UpdateAllyStatsInfoText(mapUIInfo.hoveringUnit);
            }
            else
            {
                unitInfoDisplay.SetAllyUnitInfoActive(false);
            }


            if (mapUIInfo.hoveringUnit && mapUIInfo.hoveringUnit.tag == "EnemyUnit")
            {
                unitInfoDisplay.SetEnemyUnitInfoActive(true);
                UpdateEnemyStatsInfoText(mapUIInfo.hoveringUnit);
            }
            else
            {
                unitInfoDisplay.SetEnemyUnitInfoActive(false);
            }
        }
       
    }

    private void UpdateAllyStatsInfoText(GameObject unit)
    {
        Stats unitStats = unit.GetComponent<Stats>();

        string level, hp, weapon;
        level = unitStats.level.ToString();
        hp = unitStats.hp + "/" + unitStats.maxHP;
        if (unitStats.attackMethod == AttackMethod.PHYSICAL)
            weapon = unitStats.equippedWeapon.name;
        else
            weapon = unitStats.equippedBlackMagic.name;

        unitInfoDisplay.UpdateAllyInfoText(level, hp, weapon);
    }

    private void UpdateEnemyStatsInfoText(GameObject unit)
    {
        Stats unitStats = unit.GetComponent<Stats>();

        string level, hp, weapon;
        level = unitStats.level.ToString();
        hp = unitStats.hp + "/" + unitStats.maxHP;
        if (unitStats.attackMethod == AttackMethod.PHYSICAL)
            weapon = unitStats.equippedWeapon.name;
        else
            weapon = unitStats.equippedBlackMagic.name;

        unitInfoDisplay.UpdateEnemyInfoText(level, hp, weapon);
    }
}
