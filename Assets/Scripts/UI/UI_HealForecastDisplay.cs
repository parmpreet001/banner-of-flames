using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_HealForecastDisplay : MonoBehaviour
{
    private Image CurrentHpBar; //How much health the unit currently has
    private Image HealedHpBar; //How much health the unit will have after healing
    private TextMeshProUGUI currentHP;
    private TextMeshProUGUI healedHP;

    private MapUIInfo mapUIInfo;
    
    private void Start()
    {
        mapUIInfo = GetComponentInParent<MapUIInfo>();

        HealedHpBar = transform.Find("HealedHpBar").GetComponent<Image>();
        CurrentHpBar = transform.Find("CurrentHpBar").GetComponent<Image>();
        currentHP = transform.Find("Text_CurrentHP").GetComponent<TextMeshProUGUI>();
        healedHP = transform.Find("Text_HealedHP").GetComponent<TextMeshProUGUI>();
    }

    public void UpdateInfo(float currentHP, float maxHP, int healAmount)
    {

        CurrentHpBar.fillAmount = (currentHP / maxHP);

    }
}
