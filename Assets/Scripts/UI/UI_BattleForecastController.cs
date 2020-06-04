using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UI_BattleForecastController : MonoBehaviour
{
    public GameObject allyUnitStatsUI;
    public GameObject enemyUnitStatsUI;
    public GameObject battleForecastUI;

    private MapUIInfo MapUIInfo;

    private UI_BattleForecastDisplay BattleForecastDisplay;

    // Start is called before the first frame update
    void Start()
    {
        MapUIInfo = GetComponentInParent<MapUIInfo>();
        BattleForecastDisplay = GameObject.Find("BattleForecast").GetComponent<UI_BattleForecastDisplay>();
    }

    // Update is called once per frame
    void Update()
    { 
        {
            if (MapUIInfo.selectedAllyUnit)
            {
                allyUnitStatsUI.SetActive(true);
                UpdateUnitStatsUI(allyUnitStatsUI, MapUIInfo.selectedAllyUnit);
            }
            else if(MapUIInfo.hoveringUnit && MapUIInfo.hoveringUnit.tag == "PlayerUnit")
            {
                allyUnitStatsUI.SetActive(true);
                UpdateUnitStatsUI(allyUnitStatsUI, MapUIInfo.hoveringUnit);
            }
            else
            {
                allyUnitStatsUI.SetActive(false);
            }

            if(MapUIInfo.hoveringUnit && MapUIInfo.hoveringUnit.tag == "EnemyUnit")
            {
                enemyUnitStatsUI.SetActive(true);
                UpdateUnitStatsUI(enemyUnitStatsUI, MapUIInfo.hoveringUnit);
            }
            else
            {
                enemyUnitStatsUI.SetActive(false);
            }

            if (MapUIInfo.selectedAllyUnit && MapUIInfo.hoveringUnit && MapUIInfo.hoveringUnit.tag == "EnemyUnit" && 
                !MapUIInfo.SelectedAllyUnit_AllyMove.attacking)
            {
                battleForecastUI.SetActive(true);
                UpdateBattleFoecast();
                if (MapUIInfo.SelectedAllyUnit_AllyStats.UsingOffensiveMagic())
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
            MapUIInfo.SelectedAllyUnit_AllyStats.EquipPreviousWeapon();
        else if(Input.GetKeyDown(KeyCode.S))
            MapUIInfo.SelectedAllyUnit_AllyStats.EquipNextWeapon();
    }

    private void ChangeBlackMagic()
    {
        if (Input.GetKeyDown(KeyCode.A))
            MapUIInfo.SelectedAllyUnit_AllyStats.EquipPreviousBlackMagic();
        else if (Input.GetKeyDown(KeyCode.S))
            MapUIInfo.SelectedAllyUnit_AllyStats.EquipNextBlackMagic();
    }

    private void UpdateUnitStatsUI(GameObject UnitStatsUI,GameObject unit)
    {
        Stats unitStats = unit.GetComponent<Stats>();
        UnitStatsUI.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = unitStats.level.ToString();
        UnitStatsUI.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = unitStats.hp + "/" + unitStats.maxHP;
        if (unitStats.equippedWeapon != null)
            UnitStatsUI.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = unitStats.equippedWeapon.name;
        else
            UnitStatsUI.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "-";
    }

    private void UpdateBattleFoecast()
    {
        EnemyStats hoveringUnit_enemyStats = MapUIInfo.hoveringUnit.GetComponent<EnemyStats>();
        string name, weaponName = "", damage, hitRate, critRate;
        int minRange = 0, maxRange = 0;

        //Updating values for ally unit
        name = MapUIInfo.selectedAllyUnit.name;
        if (MapUIInfo.SelectedAllyUnit_AllyStats.attackMethod == AttackMethod.PHYSICAL)
        {
            weaponName = MapUIInfo.SelectedAllyUnit_AllyStats.equippedWeapon.name;
            minRange = MapUIInfo.SelectedAllyUnit_AllyStats.equippedWeapon.minRange;
            maxRange = MapUIInfo.SelectedAllyUnit_AllyStats.equippedWeapon.maxRange;
        }    
        else if (MapUIInfo.SelectedAllyUnit_AllyStats.attackMethod == AttackMethod.OFFENSIVE_MAGIC)
        {
            weaponName = MapUIInfo.SelectedAllyUnit_AllyStats.equippedBlackMagic.name;
            minRange = MapUIInfo.SelectedAllyUnit_AllyStats.equippedBlackMagic.minRange;
            maxRange = MapUIInfo.SelectedAllyUnit_AllyStats.equippedBlackMagic.maxRange;
            MapUIInfo.SelectedAllyUnit_AllyMove.ShowWeaponRange(MapUIInfo.SelectedAllyUnit_AllyStats.equippedBlackMagic.minRange,
            MapUIInfo.SelectedAllyUnit_AllyStats.equippedBlackMagic.maxRange);
        }
            
        damage = MapUIInfo.battleManager.AU_dmg.ToString();
        hitRate = MapUIInfo.battleManager.AU_accuracy.ToString();
        critRate = MapUIInfo.battleManager.AU_crit.ToString();

        BattleForecastDisplay.UpdateAllyUnitInfoText(name, weaponName, damage, hitRate, critRate);
        BattleForecastDisplay.SetAllyUnitX2(MapUIInfo.battleManager.AU_attackTwice);

        MapUIInfo.SelectedAllyUnit_AllyMove.RemoveSelectableTiles();
        MapUIInfo.SelectedAllyUnit_AllyMove.ShowWeaponRange(minRange, maxRange);

        //Updating values forenemy Unit
        name = MapUIInfo.hoveringUnit.name;
        if (hoveringUnit_enemyStats.attackMethod == AttackMethod.PHYSICAL)
            weaponName = hoveringUnit_enemyStats.equippedWeapon.name;
        else if (hoveringUnit_enemyStats.attackMethod == AttackMethod.OFFENSIVE_MAGIC)
            weaponName = hoveringUnit_enemyStats.equippedBlackMagic.name;

        if(MapUIInfo.battleManager.DU_inRange)
        {
            damage = MapUIInfo.battleManager.DU_dmg.ToString();
            hitRate = MapUIInfo.battleManager.DU_accuracy.ToString();
            critRate = MapUIInfo.battleManager.DU_crit.ToString();
        }
        else
            damage = hitRate = critRate = "-";

        BattleForecastDisplay.UpdateEnemyInfoText(name, weaponName, damage, hitRate, critRate);
        BattleForecastDisplay.SetEnemyUnitX2(MapUIInfo.battleManager.DU_attackTwice);
    }
}
