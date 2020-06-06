using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_UnitInfoDisplay : MonoBehaviour
{
    //level, hp, weapon
    private TextMeshProUGUI[] allyUnitInfoText = new TextMeshProUGUI[3];
    private TextMeshProUGUI[] enemyUnitInfoText = new TextMeshProUGUI[3];
    Transform allyUnitInfo;
    Transform enemyUnitInfo;

    private void Start()
    {
        allyUnitInfo = transform.GetChild(0);
        enemyUnitInfo = transform.GetChild(1);

        for(int i = 0; i < 3; i++)
        {
            allyUnitInfoText[i] = allyUnitInfo.GetChild(i).GetComponent<TextMeshProUGUI>();
            enemyUnitInfoText[i] = enemyUnitInfo.GetChild(i).GetComponent<TextMeshProUGUI>();
        }
    }

    public void UpdateAllyInfoText(string level, string hp, string weapon)
    {
        allyUnitInfoText[0].text = level;
        allyUnitInfoText[1].text = hp;
        allyUnitInfoText[2].text = weapon;
    }

    public void UpdateEnemyInfoText(string level, string hp, string weapon)
    {
        enemyUnitInfoText[0].text = level;
        enemyUnitInfoText[1].text = hp;
        enemyUnitInfoText[2].text = weapon;
    }

    public void SetAllyUnitInfoActive(bool active)
    {
        allyUnitInfo.gameObject.SetActive(active);
    }

    public void SetEnemyUnitInfoActive(bool active)
    {
        enemyUnitInfo.gameObject.SetActive(active);
    }
}
