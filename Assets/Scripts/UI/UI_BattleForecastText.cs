using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_BattleForecastText : MonoBehaviour
{
    //Name, Weapon, Damage, HitRate, Crit
    private TextMeshProUGUI[] allyUnitInfoText = new TextMeshProUGUI[5];
    private TextMeshProUGUI[] enemyUnitInfoText = new TextMeshProUGUI[5];
    private GameObject allyUnitX2;
    private GameObject enemyUnitX2;

    void Start()
    {
        Transform allyUnitInfo = transform.GetChild(0);
        Transform enemyUnitInfo = transform.GetChild(1);
        
        for(int i = 0; i < 5; i++)
        {
            allyUnitInfoText[i] = allyUnitInfo.GetChild(i).GetComponent<TextMeshProUGUI>();
            enemyUnitInfoText[i] = enemyUnitInfo.GetChild(i).GetComponent<TextMeshProUGUI>();
        }

        allyUnitX2 = allyUnitInfo.GetChild(5).gameObject;
        enemyUnitX2 = enemyUnitInfo.GetChild(5).gameObject;
    }

    public void UpdateAllyUnitInfoText(string name, string weapon, string damage, string hitRate, string crit)
    {
        allyUnitInfoText[0].text = name;
        allyUnitInfoText[1].text = weapon;
        allyUnitInfoText[2].text = damage;
        allyUnitInfoText[3].text = hitRate;
        allyUnitInfoText[4].text = crit;
    }

    public void UpdateEnemyInfoText(string name, string weapon, string damage, string hitRate, string crit)
    {
        enemyUnitInfoText[0].text = name;
        enemyUnitInfoText[1].text = weapon;
        enemyUnitInfoText[2].text = damage;
        enemyUnitInfoText[3].text = hitRate;
        enemyUnitInfoText[4].text = crit;
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
