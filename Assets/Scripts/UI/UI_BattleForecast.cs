using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UI_BattleForecast : MonoBehaviour
{
    public GameObject allyUnitStatsUI;
    public GameObject enemyUnitStatsUI;
    public GameObject battleForecastUI;

    private MapUIInfo MapUIInfo;

    private UI_BattleForecastText battleForecastText;

    // Start is called before the first frame update
    void Start()
    {
        MapUIInfo = GetComponentInParent<MapUIInfo>();
        battleForecastText = GameObject.Find("BattleForecast").GetComponent<UI_BattleForecastText>();
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
                !MapUIInfo.selectedAllyUnit_AllyMove.attacking)
            {
                battleForecastUI.SetActive(true);
                UpdateBattleFoecast();
                if (MapUIInfo.selectedAllyUnit_AllyStats.UsingOffensiveMagic())
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
            MapUIInfo.selectedAllyUnit_AllyStats.EquipPreviousWeapon();
        else if(Input.GetKeyDown(KeyCode.S))
            MapUIInfo.selectedAllyUnit_AllyStats.EquipNextWeapon();
    }

    private void ChangeBlackMagic()
    {
        if (Input.GetKeyDown(KeyCode.A))
            MapUIInfo.selectedAllyUnit_AllyStats.EquipPreviousBlackMagic();
        else if (Input.GetKeyDown(KeyCode.S))
            MapUIInfo.selectedAllyUnit_AllyStats.EquipNextBlackMagic();
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
        string name, weaponName = "", damage, hitRate, critRate;
        int minRange = 0, maxRange = 0;

        //Updating values for ally unit
        name = MapUIInfo.selectedAllyUnit.name;
        if (MapUIInfo.selectedAllyUnit_AllyStats.attackMethod == AttackMethod.PHYSICAL)
        {
            weaponName = MapUIInfo.selectedAllyUnit_AllyStats.equippedWeapon.name;
            minRange = MapUIInfo.selectedAllyUnit_AllyStats.equippedWeapon.minRange;
            maxRange = MapUIInfo.selectedAllyUnit_AllyStats.equippedWeapon.maxRange;
        }    
        else if (MapUIInfo.selectedAllyUnit_AllyStats.attackMethod == AttackMethod.OFFENSIVE_MAGIC)
        {
            weaponName = MapUIInfo.selectedAllyUnit_AllyStats.equippedBlackMagic.name;
            minRange = MapUIInfo.selectedAllyUnit_AllyStats.equippedBlackMagic.minRange;
            maxRange = MapUIInfo.selectedAllyUnit_AllyStats.equippedBlackMagic.maxRange;
            MapUIInfo.selectedAllyUnit_AllyMove.ShowWeaponRange(MapUIInfo.selectedAllyUnit_AllyStats.equippedBlackMagic.minRange,
            MapUIInfo.selectedAllyUnit_AllyStats.equippedBlackMagic.maxRange);
        }
            
        damage = MapUIInfo.mapAndBattleManager.GetComponent<BattleManager>().AU_dmg.ToString();
        hitRate = MapUIInfo.mapAndBattleManager.GetComponent<BattleManager>().AU_accuracy.ToString();
        critRate = MapUIInfo.mapAndBattleManager.GetComponent<BattleManager>().AU_crit.ToString();

        battleForecastText.UpdateAllyUnitInfoText(name, weaponName, damage, hitRate, critRate);
        battleForecastText.SetAllyUnitX2(MapUIInfo.mapAndBattleManager.GetComponent<BattleManager>().AU_attackTwice);

        MapUIInfo.selectedAllyUnit_AllyMove.RemoveSelectableTiles();
        MapUIInfo.selectedAllyUnit_AllyMove.ShowWeaponRange(minRange, maxRange);

        //Updating values forenemy Unit
        name = MapUIInfo.hoveringUnit.name;
        if (MapUIInfo.hoveringUnit.GetComponent<EnemyStats>().attackMethod == AttackMethod.PHYSICAL)
            weaponName = MapUIInfo.hoveringUnit.GetComponent<EnemyStats>().equippedWeapon.name;
        else if (MapUIInfo.hoveringUnit.GetComponent<EnemyStats>().attackMethod == AttackMethod.OFFENSIVE_MAGIC)
            weaponName = MapUIInfo.hoveringUnit.GetComponent<EnemyStats>().equippedBlackMagic.name;

        if(MapUIInfo.mapAndBattleManager.GetComponent<BattleManager>().DU_inRange)
        {
            damage = MapUIInfo.mapAndBattleManager.GetComponent<BattleManager>().DU_dmg.ToString();
            hitRate = MapUIInfo.mapAndBattleManager.GetComponent<BattleManager>().DU_accuracy.ToString();
            critRate = MapUIInfo.mapAndBattleManager.GetComponent<BattleManager>().DU_crit.ToString();
        }
        else
            damage = hitRate = critRate = "-";

        battleForecastText.UpdateEnemyInfoText(name, weaponName, damage, hitRate, critRate);
        battleForecastText.SetAllyUnitX2(MapUIInfo.mapAndBattleManager.GetComponent<BattleManager>().DU_attackTwice);
    }
}
