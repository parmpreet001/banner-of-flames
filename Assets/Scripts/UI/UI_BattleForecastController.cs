using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UI_BattleForecastController : MonoBehaviour
{
    private MapUIInfo mapUIInfo;
    private GameObject battleForecastUI;
    private UI_BattleForecastDisplay battleForecastDisplay;

    void Start()
    {
        mapUIInfo = GetComponentInParent<MapUIInfo>();
        battleForecastUI = transform.GetChild(0).gameObject;
        battleForecastDisplay = battleForecastUI.GetComponent<UI_BattleForecastDisplay>();
    }

    void Update()
    { 
        {
            if (mapUIInfo.mapManager.CheckUnitStates(UnitStates.SELECTED, UnitStates.FINDING_TARGET) && mapUIInfo.hoveringUnit 
                && mapUIInfo.hoveringUnit.tag == "EnemyUnit")
            {
                battleForecastUI.SetActive(true);
                UpdateBattleFoecastDisplay();
                if (mapUIInfo.selectedAllyUnit_AllyStats.UsingOffensiveMagic())
                {
                    ChangeBlackMagic();
                }
                else
                {
                    ChangeWeapons();
                }
            }
            else
            {
                battleForecastUI.SetActive(false);
            }
        }   
    }

    private void ChangeWeapons()
    {
        if(Input.GetKeyDown(KeyCode.A))
            mapUIInfo.selectedAllyUnit_AllyStats.EquipPreviousWeapon();
        else if(Input.GetKeyDown(KeyCode.S))
            mapUIInfo.selectedAllyUnit_AllyStats.EquipNextWeapon();
    }

    private void ChangeBlackMagic()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            mapUIInfo.selectedAllyUnit_AllyStats.EquipPreviousBlackMagic();
            //mapUIInfo.selectedAllyUnit_AllyMove.RemoveSelectableTiles();
            //mapUIInfo.selectedAllyUnit_AllyMove.ShowWeaponRange(mapUIInfo.selectedAllyUnit_AllyStats.equippedBlackMagic.minRange, 
           //     mapUIInfo.selectedAllyUnit_AllyStats.equippedBlackMagic.maxRange);
        }

        else if (Input.GetKeyDown(KeyCode.S))
        {
            mapUIInfo.selectedAllyUnit_AllyStats.EquipNextBlackMagic();
            //mapUIInfo.selectedAllyUnit_AllyMove.RemoveSelectableTiles();
          //  mapUIInfo.selectedAllyUnit_AllyMove.ShowWeaponRange(mapUIInfo.selectedAllyUnit_AllyStats.equippedBlackMagic.minRange,
          //      mapUIInfo.selectedAllyUnit_AllyStats.equippedBlackMagic.maxRange);
        }
    }

    private void UpdateBattleFoecastDisplay()
    {
        EnemyStats hoveringUnit_enemyStats = mapUIInfo.hoveringUnit.GetComponent<EnemyStats>();
        string name, weaponName = "", damage, hitRate, critRate;
        int minRange = 0, maxRange = 0;

        //Updating values for ally unit
        name = mapUIInfo.selectedAllyUnit.name;
        if (mapUIInfo.selectedAllyUnit_AllyStats.attackMethod == AttackMethod.PHYSICAL)
        {
            weaponName = mapUIInfo.selectedAllyUnit_AllyStats.equippedWeapon.name;
            minRange = mapUIInfo.selectedAllyUnit_AllyStats.equippedWeapon.minRange;
            maxRange = mapUIInfo.selectedAllyUnit_AllyStats.equippedWeapon.maxRange;
        }    
        else if (mapUIInfo.selectedAllyUnit_AllyStats.attackMethod == AttackMethod.OFFENSIVE_MAGIC)
        {
            weaponName = mapUIInfo.selectedAllyUnit_AllyStats.equippedBlackMagic.name;
            minRange = mapUIInfo.selectedAllyUnit_AllyStats.equippedBlackMagic.minRange;
            maxRange = mapUIInfo.selectedAllyUnit_AllyStats.equippedBlackMagic.maxRange;
            //mapUIInfo.selectedAllyUnit_AllyMove.ShowWeaponRange(mapUIInfo.selectedAllyUnit_AllyStats.equippedBlackMagic.minRange,
            //mapUIInfo.selectedAllyUnit_AllyStats.equippedBlackMagic.maxRange);
        }
            
        damage = mapUIInfo.battleManager.AU_dmg.ToString();
        hitRate = mapUIInfo.battleManager.AU_accuracy.ToString();
        critRate = mapUIInfo.battleManager.AU_crit.ToString();

        battleForecastDisplay.UpdateAllyUnitInfoText(name, weaponName, damage, hitRate, critRate);
        battleForecastDisplay.SetAllyUnitX2Active(mapUIInfo.battleManager.AU_attackTwice);

        //mapUIInfo.selectedAllyUnit_AllyMove.RemoveSelectableTiles();
        //mapUIInfo.selectedAllyUnit_AllyMove.ShowWeaponRange(minRange, maxRange);

        //Updating values forenemy Unit
        name = mapUIInfo.hoveringUnit.name;
        if (hoveringUnit_enemyStats.attackMethod == AttackMethod.PHYSICAL)
            weaponName = hoveringUnit_enemyStats.equippedWeapon.name;
        else if (hoveringUnit_enemyStats.attackMethod == AttackMethod.OFFENSIVE_MAGIC)
            weaponName = hoveringUnit_enemyStats.equippedBlackMagic.name;

        if(mapUIInfo.battleManager.DU_inRange)
        {
            damage = mapUIInfo.battleManager.DU_dmg.ToString();
            hitRate = mapUIInfo.battleManager.DU_accuracy.ToString();
            critRate = mapUIInfo.battleManager.DU_crit.ToString();
        }
        else
            damage = hitRate = critRate = "-";

        battleForecastDisplay.UpdateEnemyInfoText(name, weaponName, damage, hitRate, critRate);
        battleForecastDisplay.SetEnemyUnitX2Active(mapUIInfo.battleManager.DU_attackTwice);
    }
}
