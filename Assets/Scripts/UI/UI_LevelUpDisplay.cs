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

    private const int levelUpBarLength = 234;

    private void Start()
    {
        background = transform.Find("Background").gameObject;
        experienceBar = transform.Find("ExperienceBar").GetComponent<Image>();
        experienceCursor = transform.Find("ExperienceCursor").gameObject;
    }

    public IEnumerator FillExperienceBar(float startingExp, int expGain, int startingLevel)
    {
        background.SetActive(true);
        experienceBar.gameObject.SetActive(true);
        experienceCursor.SetActive(true);

        SetExperienceCursorPosition(startingExp);
        experienceBar.fillAmount = (startingExp / 100);
        yield return new WaitForSeconds(0.5f);

        for(int i = 0; i <= expGain; i++)
        {
            SetExperienceCursorPosition(startingExp + i);
            experienceBar.fillAmount = (startingExp + i) / 100;
            yield return new WaitForSeconds(0.02f);
        }

        yield return new WaitForSeconds(1f);

        background.SetActive(false);
        experienceBar.gameObject.SetActive(false);
        experienceCursor.SetActive(false);

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
