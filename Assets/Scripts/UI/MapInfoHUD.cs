using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class MapInfoHUD : MonoBehaviour
{
    public GameObject allyUnitStatsUI;
    public GameObject enemyUnitStatsUI;
    public GameObject battleForecastUI;

    private MapUIInfo MapUIInfo;
    // Start is called before the first frame update
    void Start()
    {
        MapUIInfo = GetComponentInParent<MapUIInfo>();
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
                if (MapUIInfo.selectedAllyUnit_AllyStats.usingBlackMagic)
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
        Transform allyUnitInfo = battleForecastUI.transform.GetChild(0).transform;
        Transform enemyUnitInfo = battleForecastUI.transform.GetChild(1).transform;

        allyUnitInfo.GetChild(0).GetComponent<TextMeshProUGUI>().text = MapUIInfo.selectedAllyUnit.name;
        if(MapUIInfo.selectedAllyUnit_AllyStats.usingBlackMagic)
            allyUnitInfo.GetChild(1).GetComponent<TextMeshProUGUI>().text = MapUIInfo.selectedAllyUnit_AllyStats.equippedBlackMagic.name.ToString();
        else
            allyUnitInfo.GetChild(1).GetComponent<TextMeshProUGUI>().text = MapUIInfo.selectedAllyUnit_AllyStats.equippedWeapon.name.ToString();
        allyUnitInfo.GetChild(2).GetComponent<TextMeshProUGUI>().text = MapUIInfo.mapAndBattleManager.GetComponent<BattleManager>().AU_dmg.ToString();
        allyUnitInfo.GetChild(3).GetComponent<TextMeshProUGUI>().text = MapUIInfo.mapAndBattleManager.GetComponent<BattleManager>().AU_accuracy.ToString();
        allyUnitInfo.GetChild(4).GetComponent<TextMeshProUGUI>().text = MapUIInfo.mapAndBattleManager.GetComponent<BattleManager>().AU_crit.ToString();
        if (MapUIInfo.mapAndBattleManager.GetComponent<BattleManager>().AU_attackTwice)
            allyUnitInfo.GetChild(5).gameObject.SetActive(true);
        else
            allyUnitInfo.GetChild(5).gameObject.SetActive(false);

       enemyUnitInfo.GetChild(0).GetComponent<TextMeshProUGUI>().text = MapUIInfo.hoveringUnit.name;
       enemyUnitInfo.GetChild(1).GetComponent<TextMeshProUGUI>().text = MapUIInfo.hoveringUnit.GetComponent<EnemyStats>().equippedWeapon.name.ToString();
        if(MapUIInfo.mapAndBattleManager.GetComponent<BattleManager>().DU_inRange)
        {
           enemyUnitInfo.GetChild(2).GetComponent<TextMeshProUGUI>().text = MapUIInfo.mapAndBattleManager.GetComponent<BattleManager>().DU_dmg.ToString();
           enemyUnitInfo.GetChild(3).GetComponent<TextMeshProUGUI>().text = MapUIInfo.mapAndBattleManager.GetComponent<BattleManager>().DU_accuracy.ToString();
           enemyUnitInfo.GetChild(4).GetComponent<TextMeshProUGUI>().text = MapUIInfo.mapAndBattleManager.GetComponent<BattleManager>().DU_crit.ToString();
            if (MapUIInfo.mapAndBattleManager.GetComponent<BattleManager>().DU_attackTwice)
               enemyUnitInfo.GetChild(5).gameObject.SetActive(true);
            else
               enemyUnitInfo.GetChild(5).gameObject.SetActive(false);
        }
        else
        {
           enemyUnitInfo.GetChild(2).GetComponent<TextMeshProUGUI>().text = "-";
           enemyUnitInfo.GetChild(3).GetComponent<TextMeshProUGUI>().text = "-";
           enemyUnitInfo.GetChild(4).GetComponent<TextMeshProUGUI>().text = "-";
        }
    }
}
