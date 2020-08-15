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

    private const int levelUpBarLength = 234;

    public void Init()
    {
        background = transform.Find("Background").gameObject;
        experienceBar = transform.Find("ExperienceBar").GetComponent<Image>();
        experienceCursor = transform.Find("ExperienceCursor").gameObject;
        currentLevel = transform.Find("CurrentLevel").GetComponent<TextMeshProUGUI>();
        nextLevel = transform.Find("NextLevel").GetComponent<TextMeshProUGUI>();
    }

    public IEnumerator FillExperienceBar(float startingExp, int expGain, int startingLevel)
    {

        SetExperienceCursorPosition(startingExp);
        experienceBar.fillAmount = (startingExp / 100);
        currentLevel.text = startingLevel.ToString();
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

        yield return null;
    }

    private void SetExperienceCursorPosition(float experience)
    {
        float xPosition = -117 + (levelUpBarLength * (experience / 100));
        float yPosition = experienceCursor.transform.localPosition.y;
        experienceCursor.transform.localPosition = new Vector2(xPosition, yPosition);
        experienceCursor.transform.GetComponentInChildren<TextMeshProUGUI>().text = experience.ToString();
    }
}
