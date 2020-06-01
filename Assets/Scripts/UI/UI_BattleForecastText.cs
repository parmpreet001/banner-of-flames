using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_BattleForecastText : MonoBehaviour
{
    //Name, Weapon, Damage, HitRate, Crit
    private TextMeshProUGUI[] allyUnitInfoText = new TextMeshProUGUI[5];
    private TextMeshProUGUI[] enemyUnitInfoText = new TextMeshProUGUI[5];

    void Start()
    {
        Transform allyUnitInfo = transform.GetChild(0);
        Transform enemyUnitInfo = transform.GetChild(1);
        
        for(int i = 0; i < 5; i++)
        {
            allyUnitInfoText[i] = allyUnitInfo.GetChild(i).GetComponent<TextMeshProUGUI>();
            enemyUnitInfoText[i] = enemyUnitInfo.GetChild(i).GetComponent<TextMeshProUGUI>();
        }
    }

    public void UpdateAllyUnitInfoText(string name, string weapon, int damage, int hitRate, int crit)
    {
        allyUnitInfoText[0].text = name;
        allyUnitInfoText[1].text = weapon;
        allyUnitInfoText[2].text = damage.ToString();
        allyUnitInfoText[3].text = hitRate.ToString();
        allyUnitInfoText[4].text = crit.ToString();
    }
}
