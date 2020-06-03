using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_BattleForecastDisplay : MonoBehaviour
{
    //Name, Weapon, Damage, HitRate, Crit
    private TextMeshProUGUI[] AllyUnitInfoText = new TextMeshProUGUI[5];
    private TextMeshProUGUI[] EnemyUnitInfoText = new TextMeshProUGUI[5];
    private GameObject allyUnitX2;
    private GameObject enemyUnitX2;

    void Start()
    {
        Transform allyUnitInfo = transform.GetChild(0);
        Transform enemyUnitInfo = transform.GetChild(1);
        
        for(int i = 0; i < 5; i++)
        {
            AllyUnitInfoText[i] = allyUnitInfo.GetChild(i).GetComponent<TextMeshProUGUI>();
            EnemyUnitInfoText[i] = enemyUnitInfo.GetChild(i).GetComponent<TextMeshProUGUI>();
        }

        allyUnitX2 = allyUnitInfo.GetChild(5).gameObject;
        enemyUnitX2 = enemyUnitInfo.GetChild(5).gameObject;
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

    public void SetAllyUnitX2(bool active)
    {
        allyUnitX2.SetActive(active);
    }

    public void SetEnemyUnitX2(bool active)
    {
        enemyUnitX2.SetActive(active);
    }
}
