using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class MapInfoHUD : MonoBehaviour
{
    private GameObject UIElements;
    [SerializeField]
    private GameObject selectedEnemyUnit;
    public GameObject cursor;

    public GameObject allyUnitStatsUI;
    public GameObject enemyUnitStatsUI;
    public GameObject battleForecastUI;
    // Start is called before the first frame update
    void Start()
    {
        UIElements = transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.LeftArrow)
        //    || Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.Z))
        {
            if (UIElements.GetComponent<MapUIInfo>().selectedAllyUnit)
            {
                allyUnitStatsUI.SetActive(true);
                UpdateUnitStatsUI(allyUnitStatsUI, UIElements.GetComponent<MapUIInfo>().selectedAllyUnit);
            }
            else if(UIElements.GetComponent<MapUIInfo>().hoveringUnit && UIElements.GetComponent<MapUIInfo>().hoveringUnit.tag == "PlayerUnit")
            {
                allyUnitStatsUI.SetActive(true);
                UpdateUnitStatsUI(allyUnitStatsUI, UIElements.GetComponent<MapUIInfo>().hoveringUnit);
            }
            else
            {
                allyUnitStatsUI.SetActive(false);
            }

            if(cursor.GetComponent<Cursor>().CurrentTileHasEnemyUnit())
            {
                selectedEnemyUnit = cursor.GetComponent<Cursor>().currentTile.transform.GetChild(0).gameObject;
                enemyUnitStatsUI.SetActive(true);
                UpdateUnitStatsUI(enemyUnitStatsUI, selectedEnemyUnit);
            }
            else
            {
                enemyUnitStatsUI.SetActive(false);
                selectedEnemyUnit = null;
            }

            if (UIElements.GetComponent<MapUIInfo>().selectedAllyUnit && selectedEnemyUnit)
            {
                battleForecastUI.SetActive(true);
            }
            else
            {
                battleForecastUI.SetActive(false);
            }
        }
        //if (!selectedEnemyUnit)
        //{
        //    selectedEnemyUnit = null;
        //    enemyUnitStatsUI.SetActive(false);
        //}
        //if (!UIElements.GetComponent<MapUIInfo>().selectedAllyUnit)
        //{
        //    allyUnitStatsUI.SetActive(false);
        //}
            
    }

    private void UpdateUnitStatsUI(GameObject UnitStatsUI,GameObject unit)
    {
        Stats unitStats = unit.GetComponent<Stats>();
        UnitStatsUI.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = unitStats.level.ToString();
        UnitStatsUI.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = unitStats.hp + "/" + unitStats.maxHP;
        UnitStatsUI.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = unitStats.equippedWeapon.name;
    }
}
