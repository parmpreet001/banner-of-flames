using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_LevelUpDisplay : MonoBehaviour
{
    private GameObject background;
    private Image experienceBar;

    private void Start()
    {
        background = transform.Find("Background").gameObject;
        experienceBar = transform.Find("ExperienceBar").GetComponent<Image>();
    }

    public IEnumerator FillExperienceBar(float startingExp, int expGain, int startingLevel)
    {
        background.SetActive(true);
        experienceBar.gameObject.SetActive(true);

        experienceBar.fillAmount = (startingExp / 100);
        yield return new WaitForSeconds(0.5f);

        for(int i = 0; i <= expGain; i++)
        {
            experienceBar.fillAmount = (startingExp + i) / 100;
            yield return new WaitForSeconds(0.02f);
        }

        yield return new WaitForSeconds(0.75f);

        background.SetActive(false);
        experienceBar.gameObject.SetActive(false);

        yield return null;
    }
}
