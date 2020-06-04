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
            if (mapUIInfo.selectedAllyUnit && mapUIInfo.HoveringUnit && mapUIInfo.HoveringUnit.tag == "EnemyUnit" && 
                !mapUIInfo.SelectedAllyUnit_AllyMove.attacking)
            {
                battleForecastUI.SetActive(true);
                UpdateBattleFoecastDisplay();
                if (mapUIInfo.SelectedAllyUnit_AllyStats.UsingOffensiveMagic())
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
            mapUIInfo.SelectedAllyUnit_AllyStats.EquipPreviousWeapon();
        else if(Input.GetKeyDown(KeyCode.S))
            mapUIInfo.SelectedAllyUnit_AllyStats.EquipNextWeapon();
    }

    private void ChangeBlackMagic()
    {
        if (Input.GetKeyDown(KeyCode.A))
            mapUIInfo.SelectedAllyUnit_AllyStats.EquipPreviousBlackMagic();
        else if (Input.GetKeyDown(KeyCode.S))
            mapUIInfo.SelectedAllyUnit_AllyStats.EquipNextBlackMagic();
    }

    private void UpdateBattleFoecastDisplay()
    {
        EnemyStats hoveringUnit_enemyStats = mapUIInfo.HoveringUnit.GetComponent<EnemyStats>();
        string name, weaponName = "", damage, hitRate, critRate;
        int minRange = 0, maxRange = 0;

        //Updating values for ally unit
        name = mapUIInfo.selectedAllyUnit.name;
        if (mapUIInfo.SelectedAllyUnit_AllyStats.attackMethod == AttackMethod.PHYSICAL)
        {
            weaponName = mapUIInfo.SelectedAllyUnit_AllyStats.equippedWeapon.name;
            minRange = mapUIInfo.SelectedAllyUnit_AllyStats.equippedWeapon.minRange;
            maxRange = mapUIInfo.SelectedAllyUnit_AllyStats.equippedWeapon.maxRange;
        }    
        else if (mapUIInfo.SelectedAllyUnit_AllyStats.attackMethod == AttackMethod.OFFENSIVE_MAGIC)
        {
            weaponName = mapUIInfo.SelectedAllyUnit_AllyStats.equippedBlackMagic.name;
            minRange = mapUIInfo.SelectedAllyUnit_AllyStats.equippedBlackMagic.minRange;
            maxRange = mapUIInfo.SelectedAllyUnit_AllyStats.equippedBlackMagic.maxRange;
            mapUIInfo.SelectedAllyUnit_AllyMove.ShowWeaponRange(mapUIInfo.SelectedAllyUnit_AllyStats.equippedBlackMagic.minRange,
            mapUIInfo.SelectedAllyUnit_AllyStats.equippedBlackMagic.maxRange);
        }
            
        damage = mapUIInfo.BattleManager.AU_dmg.ToString();
        hitRate = mapUIInfo.BattleManager.AU_accuracy.ToString();
        critRate = mapUIInfo.BattleManager.AU_crit.ToString();

        battleForecastDisplay.UpdateAllyUnitInfoText(name, weaponName, damage, hitRate, critRate);
        battleForecastDisplay.SetAllyUnitX2(mapUIInfo.BattleManager.AU_attackTwice);

        mapUIInfo.SelectedAllyUnit_AllyMove.RemoveSelectableTiles();
        mapUIInfo.SelectedAllyUnit_AllyMove.ShowWeaponRange(minRange, maxRange);

        //Updating values forenemy Unit
        name = mapUIInfo.HoveringUnit.name;
        if (hoveringUnit_enemyStats.attackMethod == AttackMethod.PHYSICAL)
            weaponName = hoveringUnit_enemyStats.equippedWeapon.name;
        else if (hoveringUnit_enemyStats.attackMethod == AttackMethod.OFFENSIVE_MAGIC)
            weaponName = hoveringUnit_enemyStats.equippedBlackMagic.name;

        if(mapUIInfo.BattleManager.DU_inRange)
        {
            damage = mapUIInfo.BattleManager.DU_dmg.ToString();
            hitRate = mapUIInfo.BattleManager.DU_accuracy.ToString();
            critRate = mapUIInfo.BattleManager.DU_crit.ToString();
        }
        else
            damage = hitRate = critRate = "-";

        battleForecastDisplay.UpdateEnemyInfoText(name, weaponName, damage, hitRate, critRate);
        battleForecastDisplay.SetEnemyUnitX2(mapUIInfo.BattleManager.DU_attackTwice);
    }
}
