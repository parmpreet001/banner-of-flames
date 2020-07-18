using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_HealForecastDisplay : MonoBehaviour
{
    private Image CurrentHpBar; //How much health the unit currently has
    private Image HealedHpBar; //How much health the unit will have after healing
    private TextMeshProUGUI currentHPText;
    private TextMeshProUGUI healedHPText;

    private MapUIInfo mapUIInfo;
    
    private void Start()
    {
        mapUIInfo = GetComponentInParent<MapUIInfo>();

        HealedHpBar = transform.Find("HealedHpBar").GetComponent<Image>();
        CurrentHpBar = transform.Find("CurrentHpBar").GetComponent<Image>();
        currentHPText = transform.Find("Text_CurrentHP").GetComponent<TextMeshProUGUI>();
        healedHPText = transform.Find("Text_HealedHP").GetComponent<TextMeshProUGUI>();
    }

    public void UpdateInfo(float currentHP, float maxHP, int healAmount)
    {
        CurrentHpBar.fillAmount = (currentHP / maxHP);
        HealedHpBar.fillAmount = (currentHP + healAmount) / maxHP;

        currentHPText.text = currentHP.ToString();

        if((currentHP + healAmount) > maxHP)
            healedHPText.text = maxHP.ToString();
        else
            healedHPText.text = (currentHP + healAmount).ToString();
    }
}
