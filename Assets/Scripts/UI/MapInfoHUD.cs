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
                battleForecastUI.SetActive(true);
                UpdateBattleFoecast(MapUIInfo.selectedAllyUnit, MapUIInfo.hoveringUnit);
            }
            else
            {
                battleForecastUI.SetActive(false);
            }
        }   
    }

    private void UpdateUnitStatsUI(GameObject UnitStatsUI,GameObject unit)
    {
        Stats unitStats = unit.GetComponent<Stats>();
        UnitStatsUI.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = unitStats.level.ToString();
        UnitStatsUI.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = unitStats.hp + "/" + unitStats.maxHP;
        UnitStatsUI.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = unitStats.equippedWeapon.name;
    }

    private void UpdateBattleFoecast(GameObject playerUnit, GameObject enemyUnit)
    {
        GameObject allyUnitInfo = battleForecastUI.transform.GetChild(0).gameObject;
        GameObject enemyUnitInfo = battleForecastUI.transform.GetChild(1).gameObject;

        allyUnitInfo.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = playerUnit.name;
        allyUnitInfo.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = playerUnit.GetComponent<AllyStats>().equippedWeapon.name;
        //GUH
        allyUnitInfo.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = playerUnit.GetComponent<AllyStats>().GetDmg(playerUnit, enemyUnit).ToString();
        allyUnitInfo.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = (playerUnit.GetComponent<AllyStats>().GetHitrate(playerUnit)
            - enemyUnit.GetComponent<EnemyStats>().GetAvoid(enemyUnit)).ToString();
        allyUnitInfo.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = playerUnit.GetComponent<AllyStats>().GetCritRate(playerUnit).ToString();

        enemyUnitInfo.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = enemyUnit.name;
        enemyUnitInfo.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = enemyUnit.GetComponent<EnemyStats>().equippedWeapon.name;
        enemyUnitInfo.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = enemyUnit.GetComponent<EnemyStats>().GetDmg(enemyUnit, playerUnit).ToString();
        enemyUnitInfo.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = (enemyUnit.GetComponent<EnemyStats>().GetHitrate(enemyUnit)
            - playerUnit.GetComponent<AllyStats>().GetAvoid(playerUnit)).ToString();
        enemyUnitInfo.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = enemyUnit.GetComponent<EnemyStats>().GetCritRate(enemyUnit).ToString();


    }
}
