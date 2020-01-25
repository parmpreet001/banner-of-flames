using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapActionMenu : MonoBehaviour
{
    private GameObject UIElements;
    private GameObject selectedAllyUnit;
    private bool buttonsCreated = false;
    List<string> buttons = new List<string>();
    // Start is called before the first frame update
    void Start()
    {
        UIElements = transform.parent.gameObject;    
    }

    // Update is called once per frame
    void Update()
    {
        selectedAllyUnit = UIElements.GetComponent<MapUIInfo>().selectedAllyUnit;
        if (selectedAllyUnit)
        {
            if (selectedAllyUnit.GetComponent<AllyMove>().actionMenu)
            {
                if (!buttonsCreated)
                {
                    CreateButtons();
                }
            }
            else if (!selectedAllyUnit.GetComponent<AllyMove>().actionMenu && buttonsCreated)
            {
                ResetActionMenu(false);
            }
        }
    }

    private void CreateButtons()
    {
        buttonsCreated = true;

        AllyMove am = selectedAllyUnit.GetComponent<AllyMove>();

        if (am.findingTarget)
            buttons.Add("Attack");
        buttons.Add("Wait");

        for (int i = 0; i < buttons.Count; i++)
        {
            if (buttons[i] == "Attack")
            {
                transform.Find("AttackButton").gameObject.SetActive(true);
                transform.Find("AttackButton").GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -35 * i);
            }
            else if (buttons[i] == "Wait")
            {
                transform.Find("WaitButton").gameObject.SetActive(true);
                transform.Find("WaitButton").GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -35 * i);
            }
        }

        GetComponent<RectTransform>().anchoredPosition = new Vector2(GetComponent<RectTransform>().anchoredPosition.x, 17.5f * (buttons.Count - 1));
    }
    public void Wait()
    {
        selectedAllyUnit.GetComponent<AllyMove>().UnselectUnit();
        selectedAllyUnit.GetComponent<AllyMove>().finished = true;
        ResetActionMenu(true);
    }

    public void Attack()
    {
        selectedAllyUnit.GetComponent<AllyMove>().actionMenu = false;
        ResetActionMenu(false);
    }

    private void ResetActionMenu(bool removeSelectedAllyUnit)
    {
        if (removeSelectedAllyUnit)
            selectedAllyUnit = null;
        buttonsCreated = false;
        buttons.Clear();
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.activeInHierarchy)
                transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}
