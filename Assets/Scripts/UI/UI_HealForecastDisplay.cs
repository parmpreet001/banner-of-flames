using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HealForecastDisplay : MonoBehaviour
{
    private Image CurrentHpHealthBar; //How much health the unit currently has
    private Image HealedHpHealthBar; //How much health the unit will have after healing

    private MapUIInfo mapUIInfo;
    
    private void Start()
    {
        mapUIInfo = GetComponentInParent<MapUIInfo>();
        HealedHpHealthBar = transform.Find("HealedHP").GetComponent<Image>();
        CurrentHpHealthBar = transform.Find("CurrentHP").GetComponent<Image>();

        HealedHpHealthBar.fillAmount = 0;
    }

    private void Update()
    {
        if(mapUIInfo.selectedAllyUnit)
        {
            SetFillAmounts(mapUIInfo.selectedAllyUnit_AllyStats.hp, mapUIInfo.selectedAllyUnit_AllyStats.maxHP, 0);
        }
    }

    public void SetFillAmounts(float currentHP, float maxHP, int healAmount)
    {
        Debug.Log(currentHP / maxHP);
        CurrentHpHealthBar.fillAmount = (currentHP / maxHP);
    }
}
