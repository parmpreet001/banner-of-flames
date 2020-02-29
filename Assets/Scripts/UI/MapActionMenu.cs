using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapActionMenu : MonoBehaviour
{
    private bool buttonsCreated = false;
    List<string> buttons = new List<string>();

    public int menuCursorPosition = 1;
    public GameObject menuCursor;

    private MapUIInfo MapUIInfo;

    private void Start()
    {
        MapUIInfo = GetComponentInParent<MapUIInfo>();
    }

    void Update()
    {
        if (MapUIInfo.selectedAllyUnit)
        {
            if (MapUIInfo.selectedAllyUnit.GetComponent<AllyMove>().actionMenu)
            {
                if (!buttonsCreated)
                {
                    CreateButtons();
                    menuCursor.SetActive(true);
                }
                menuCursorInput();
            }
            else if (!MapUIInfo.selectedAllyUnit.GetComponent<AllyMove>().actionMenu && buttonsCreated)
            {
                ResetActionMenu(false);
            }
        }
        else
        {
            ResetActionMenu(false);
        }
    }

    private void CreateButtons()
    {
        buttonsCreated = true;

        AllyMove am = MapUIInfo.selectedAllyUnit.GetComponent<AllyMove>();

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

    private void menuCursorInput()
    {
        if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (menuCursorPosition < buttons.Count)
                menuCursorPosition++;
        }
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (menuCursorPosition > 1)
                menuCursorPosition--;
        }
        menuCursor.GetComponent<RectTransform>().anchoredPosition = new Vector2(menuCursor.GetComponent<RectTransform>().anchoredPosition.x, -35 * (menuCursorPosition - 1));
        if(Input.GetKeyDown(KeyCode.Z))
        {
            //Debug.Log(transform.Find(buttons[menuCursorPosition-1] + "Button").ToString());
            string methodName = buttons[menuCursorPosition - 1];
            Invoke(methodName, 0);

        }
    }

    public void Wait()
    {
        MapUIInfo.selectedAllyUnit.GetComponent<AllyMove>().UnselectUnit();
        MapUIInfo.selectedAllyUnit.GetComponent<AllyMove>().finished = true;
        ResetActionMenu(true);
    }

    public void Attack()
    {
        MapUIInfo.selectedAllyUnit.GetComponent<AllyMove>().actionMenu = false;
        ResetActionMenu(false);
    }

    private void ResetActionMenu(bool removeSelectedAllyUnit)
    {
        if (removeSelectedAllyUnit)
            MapUIInfo.selectedAllyUnit = null;
        buttonsCreated = false;
        buttons.Clear();
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.activeInHierarchy)
                transform.GetChild(i).gameObject.SetActive(false);
        }
        menuCursor.GetComponent<RectTransform>().anchoredPosition = new Vector2(menuCursor.GetComponent<RectTransform>().anchoredPosition.x, 0);
        menuCursorPosition = 1;
    }
}
