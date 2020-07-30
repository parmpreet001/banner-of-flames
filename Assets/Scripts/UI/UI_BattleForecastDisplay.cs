using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_BattleForecastDisplay : MonoBehaviour
{
    //Name, Weapon, Damage, HitRate, Crit
    private TextMeshProUGUI[] AllyUnitInfoText = new TextMeshProUGUI[5];
    private TextMeshProUGUI[] EnemyUnitInfoText = new TextMeshProUGUI[5];
    private GameObject AllyUnitX2;
    private GameObject EnemyUnitX2;
    private GameObject redArrow;
    private GameObject greenArrow;

    void Start()
    {
        Transform allyUnitInfo = transform.GetChild(0);
        Transform enemyUnitInfo = transform.GetChild(1);
        
        for(int i = 0; i < 5; i++)
        {
            AllyUnitInfoText[i] = allyUnitInfo.GetChild(i).GetComponent<TextMeshProUGUI>();
            EnemyUnitInfoText[i] = enemyUnitInfo.GetChild(i).GetComponent<TextMeshProUGUI>();
        }

        AllyUnitX2 = allyUnitInfo.GetChild(5).gameObject;
        EnemyUnitX2 = enemyUnitInfo.GetChild(5).gameObject;

        redArrow = transform.GetChild(4).gameObject;
        greenArrow = transform.GetChild(5).gameObject;
    }

    public void UpdateAllyUnitInfoText(string name, string weapon, string damage, string hitRate, string crit)
    {
        AllyUnitInfoText[0].text = name;
        AllyUnitInfoText[1].text = weapon;
        AllyUnitInfoText[2].text = damage;
        AllyUnitInfoText[3].text = hitRate;
        AllyUnitInfoText[4].text = crit;
    }

    public void UpdateEnemyInfoText(string name, string weapon, string damage, string hitRate, string crit)
    {
        EnemyUnitInfoText[0].text = name;
        EnemyUnitInfoText[1].text = weapon;
        EnemyUnitInfoText[2].text = damage;
        EnemyUnitInfoText[3].text = hitRate;
        EnemyUnitInfoText[4].text = crit;
    }

    public void SetAllyUnitX2Active(bool active)
    {
        AllyUnitX2.SetActive(active);
    }

    public void SetEnemyUnitX2Active(bool active)
    {
        EnemyUnitX2.SetActive(active);
    }

    public void SetAllyUnitGreenArrow()
    {
        greenArrow.transform.localPosition = new Vector2(-62.5f, 98f);
    }

    public void SetAllyUnitRedArrow()
    {
        redArrow.transform.localPosition = new Vector2(-62.5f, 98f);
    }

    public void SetEnemyUnitGreenArrow()
    {
        greenArrow.transform.localPosition = new Vector2(62.5f, 98f);
    }

    public void SetEnemyUnitRedArrow()
    {
        redArrow.transform.localPosition = new Vector2(62.5f, 98f);
    }

    public void SetGreenArrowActive(bool active)
    {
        greenArrow.SetActive(active);
    }

    public void SetRedArrowActive(bool active)
    {
        redArrow.SetActive(active);
    }
}
