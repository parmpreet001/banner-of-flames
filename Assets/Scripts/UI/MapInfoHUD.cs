using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class MapInfoHUD : MonoBehaviour
{
    private GameObject UIElements;
    private GameObject selectedAllyUnit;
    [SerializeField]
    private GameObject selectedEnemyUnit;
    public GameObject cursor;

    public GameObject allyUnitStatsUI;
    public GameObject enemyUnitStatsUI;
    // Start is called before the first frame update
    void Start()
    {
        UIElements = transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (UIElements.GetComponent<MapUIInfo>().selectedAllyUnit)
                selectedAllyUnit = UIElements.GetComponent<MapUIInfo>().selectedAllyUnit;
            else if (cursor.GetComponent<Cursor>().CurrentTileHasAllyUnit())
                selectedAllyUnit = cursor.GetComponent<Cursor>().currentTile.transform.GetChild(0).gameObject;
            else
                selectedAllyUnit = null;

            if(selectedAllyUnit)
            {
                allyUnitStatsUI.SetActive(true);
                UpdateUnitStatsUI(allyUnitStatsUI, selectedAllyUnit);
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
        }
        if (!selectedEnemyUnit)
        {
            selectedEnemyUnit = null;
            enemyUnitStatsUI.SetActive(false);
        }
        if (selectedAllyUnit && selectedAllyUnit.GetComponent<AllyMove>().finished)
        {
            selectedAllyUnit = null;
            allyUnitStatsUI.SetActive(false);
        }
            
    }

    private void UpdateUnitStatsUI(GameObject UnitStatsUI,GameObject unit)
    {
        Stats unitStats = unit.GetComponent<Stats>();
        UnitStatsUI.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = unitStats.level.ToString();
        UnitStatsUI.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = unitStats.hp + "/" + unitStats.maxHP;
        UnitStatsUI.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = unitStats.equippedWeapon.name;
    }
}
