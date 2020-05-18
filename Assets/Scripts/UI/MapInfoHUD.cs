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
                !MapUIInfo.selectedAllyUnit.GetComponent<AllyMove>().attacking)
            {
                if(MapUIInfo.selectedAllyUnit.GetComponent<AllyStats>().usingBlackMagic)
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
        {
            Debug.Log("Pressed A");
            MapUIInfo.selectedAllyUnit.GetComponent<AllyStats>().EquipPreviousWeapon();
        }
        else if(Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("Pressed S");
            MapUIInfo.selectedAllyUnit.GetComponent<AllyStats>().EquipNextWeapon();
        }
        battleForecastUI.SetActive(true);
        UpdateBattleFoecast();
    }

    private void ChangeBlackMagic()
    {

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
        GameObject allyUnitInfo = battleForecastUI.transform.GetChild(0).gameObject;
        GameObject enemyUnitInfo = battleForecastUI.transform.GetChild(1).gameObject;

        allyUnitInfo.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = MapUIInfo.selectedAllyUnit.name;
        if(MapUIInfo.selectedAllyUnit.GetComponent<AllyStats>().usingBlackMagic)
            allyUnitInfo.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = MapUIInfo.selectedAllyUnit.GetComponent<AllyStats>().equppedBlackMagic.name.ToString();
        else
            allyUnitInfo.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = MapUIInfo.selectedAllyUnit.GetComponent<AllyStats>().equippedWeapon.name.ToString();
        allyUnitInfo.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = MapUIInfo.mapAndBattleManager.GetComponent<BattleManager>().AU_dmg.ToString();
        allyUnitInfo.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = MapUIInfo.mapAndBattleManager.GetComponent<BattleManager>().AU_accuracy.ToString();
        allyUnitInfo.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = MapUIInfo.mapAndBattleManager.GetComponent<BattleManager>().AU_crit.ToString();
        if (MapUIInfo.mapAndBattleManager.GetComponent<BattleManager>().AU_attackTwice)
            allyUnitInfo.transform.GetChild(5).gameObject.SetActive(true);
        else
            allyUnitInfo.transform.GetChild(5).gameObject.SetActive(false);

        enemyUnitInfo.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = MapUIInfo.hoveringUnit.name;
        enemyUnitInfo.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = MapUIInfo.hoveringUnit.GetComponent<EnemyStats>().equippedWeapon.name.ToString();
        if(MapUIInfo.mapAndBattleManager.GetComponent<BattleManager>().DU_inRange)
        {
            enemyUnitInfo.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = MapUIInfo.mapAndBattleManager.GetComponent<BattleManager>().DU_dmg.ToString();
            enemyUnitInfo.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = MapUIInfo.mapAndBattleManager.GetComponent<BattleManager>().DU_accuracy.ToString();
            enemyUnitInfo.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = MapUIInfo.mapAndBattleManager.GetComponent<BattleManager>().DU_crit.ToString();
            if (MapUIInfo.mapAndBattleManager.GetComponent<BattleManager>().DU_attackTwice)
                enemyUnitInfo.transform.GetChild(5).gameObject.SetActive(true);
            else
                enemyUnitInfo.transform.GetChild(5).gameObject.SetActive(false);
        }
        else
        {
            enemyUnitInfo.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "-";
            enemyUnitInfo.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "-";
            enemyUnitInfo.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "-";
        }



    }
}
