using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_LevelUpDisplay : MonoBehaviour
{
    private GameObject background;
    private Image experienceBar;
    private GameObject experienceCursor;
    private TextMeshProUGUI currentLevel;
    private TextMeshProUGUI nextLevel;

    private GameObject statsDisplay;
    private TextMeshProUGUI statsText1;
    private TextMeshProUGUI statsText2;
   


    private const int levelUpBarLength = 234;

    public void Init()
    {
        background = transform.Find("Background").gameObject;
        experienceBar = transform.Find("ExperienceBar").GetComponent<Image>();
        experienceCursor = transform.Find("ExperienceCursor").gameObject;
        currentLevel = transform.Find("CurrentLevel").GetComponent<TextMeshProUGUI>();
        nextLevel = transform.Find("NextLevel").GetComponent<TextMeshProUGUI>();

        statsDisplay = transform.Find("StatsDisplay").gameObject;
        statsText1 = statsDisplay.transform.Find("StatsText1").GetComponent<TextMeshProUGUI>();
        statsText2 = statsDisplay.transform.Find("StatsText2").GetComponent<TextMeshProUGUI>();
    }

    public IEnumerator FillExperienceBar(float startingExp, int expGain, AllyStats unitStats, int[] previouStats)
    {
        statsDisplay.SetActive(false);

        SetExperienceCursorPosition(startingExp);
        experienceBar.fillAmount = (startingExp / 100);
        currentLevel.text = (unitStats.level - 1).ToString();
        nextLevel.text = (unitStats.level).ToString();

        yield return new WaitForSeconds(0.5f);

        for(int i = 0; i <= expGain; i++)
        {
            if(startingExp + i >= 100)
            {
                SetExperienceCursorPosition(startingExp + i - 100);
                experienceBar.fillAmount = ((startingExp + i) / 100) - 1;
                currentLevel.text = (unitStats.level).ToString();
                nextLevel.text = (unitStats.level + 1).ToString();
            }
            else
            {

                SetExperienceCursorPosition(startingExp + i);
                experienceBar.fillAmount = (startingExp + i) / 100;
            }

            yield return new WaitForSeconds(0.02f);
        }

        yield return new WaitForSeconds(1f);

        if(expGain + startingExp >= 100)
        {
            UpdateStatsText(previouStats);
            statsDisplay.SetActive(true);
            yield return new WaitForSeconds(1f);
        }

        statsDisplay.SetActive(false);

        yield return null;
    }

    private void SetExperienceCursorPosition(float experience)
    {
        float xPosition = -117 + (levelUpBarLength * (experience / 100));
        float yPosition = experienceCursor.transform.localPosition.y;
        experienceCursor.transform.localPosition = new Vector2(xPosition, yPosition);
        experienceCursor.transform.GetComponentInChildren<TextMeshProUGUI>().text = experience.ToString();
    }

    private void UpdateStatsText(int[] stats)
    {
        statsText1.text = stats[0] + "\n" + stats[1] + "\n" + stats[2] + "\n" + stats[3];
        statsText2.text = stats[4] + "\n" + stats[5] + "\n" + stats[6] + "\n" + stats[7];
    }
}
