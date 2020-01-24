using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITest : MonoBehaviour
{
    public GameObject selectedAllyUnit;
    private GameObject actionMenu;
    private bool buttonsCreated = false;
    List<string> buttons = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        actionMenu = transform.Find("ActionMenu").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(selectedAllyUnit)
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
        actionMenu.SetActive(true);
        buttonsCreated = true;

        AllyMove am = selectedAllyUnit.GetComponent<AllyMove>();

        if (am.findingTarget)
            buttons.Add("Attack");
        buttons.Add("Wait");

        for(int i = 0; i < buttons.Count; i++)
        {
            if(buttons[i] == "Attack")
            {
                actionMenu.transform.Find("AttackButton").gameObject.SetActive(true);
                actionMenu.transform.Find("AttackButton").GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -35 * i);
            }
            else if(buttons[i] == "Wait")
            {
                actionMenu.transform.Find("WaitButton").gameObject.SetActive(true);
                actionMenu.transform.Find("WaitButton").GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -35 * i);
            }
        }

        actionMenu.GetComponent<RectTransform>().anchoredPosition = new Vector2(actionMenu.GetComponent<RectTransform>().anchoredPosition.x, 17.5f * (buttons.Count - 1));
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
        for(int i = 0; i < actionMenu.transform.childCount; i++)
        {
            if (actionMenu.transform.GetChild(i).gameObject.activeInHierarchy)
                actionMenu.transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}
