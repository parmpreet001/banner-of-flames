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

    private List<GameObject> levelUpArrows;
   


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

        levelUpArrows = new List<GameObject>();
        for(int i = 0; i < transform.Find("StatsDisplay").Find("Arrows1").childCount; i++ )
        {
            levelUpArrows.Add(transform.Find("StatsDisplay").Find("Arrows1").GetChild(i).gameObject);
        }
        for (int i = 0; i < transform.Find("StatsDisplay").Find("Arrows2").childCount; i++)
        {
            levelUpArrows.Add(transform.Find("StatsDisplay").Find("Arrows2").GetChild(i).gameObject);
        }
    }

    public IEnumerator FillExperienceBar(float startingExp, int expGain, AllyStats unitStats, int[] previouStats, int startingLevel)
    {
        statsDisplay.SetActive(false);

        SetExperienceCursorPosition(startingExp);
        experienceBar.fillAmount = (startingExp / 100);
        currentLevel.text = (startingLevel).ToString();
        nextLevel.text = (startingLevel + 1).ToString();

        yield return new WaitForSeconds(0.5f);

        for(int i = 0; i <= expGain; i++)
        {
            if(startingExp + i >= 100)
            {
                SetExperienceCursorPosition(startingExp + i - 100);
                experienceBar.fillAmount = ((startingExp + i) / 100) - 1;
                currentLevel.text = (startingLevel + 1).ToString();
                nextLevel.text = (startingLevel + 2).ToString();
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
            yield return new WaitForSeconds(0.5f);
            int[] currentStats = unitStats.GetBattleStats();
            for(int i = 0; i < previouStats.Length; i++)
            {
                if(previouStats[i] != currentStats[i])
                {
                    previouStats[i] = currentStats[i];
                    UpdateStatsText(previouStats);
                    levelUpArrows[i].SetActive(true);
                    yield return new WaitForSeconds(0.5f);
                }
            }
            yield return new WaitForSeconds(1f);
            foreach(GameObject arrow in levelUpArrows)
            {
                arrow.SetActive(false);
            }
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
