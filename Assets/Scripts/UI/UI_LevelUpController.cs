using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_LevelUpController : MonoBehaviour
{
    [SerializeField]
    private GameObject levelUpUI;

    private void Start()
    {
        levelUpUI = transform.Find("LevelUpUI").gameObject;
        levelUpUI.GetComponent<UI_LevelUpDisplay>().Init();
    }

    public IEnumerator FillExperienceBar(float startingExp, int expGain, AllyStats unitStats, int[] previousStats, int startingLevel)
    {
        levelUpUI.SetActive(true);
        yield return levelUpUI.GetComponent<UI_LevelUpDisplay>().FillExperienceBar(startingExp, expGain, unitStats, previousStats, startingLevel);
        levelUpUI.SetActive(false);
    }
}
