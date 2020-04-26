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
    private bool selectingItems = false;

    private void Start()
    {
        MapUIInfo = GetComponentInParent<MapUIInfo>();
    }

    void Update()
    {
        //If an ally unit is selected
        if (MapUIInfo.selectedAllyUnit)
        {
            //If they're in an action menu state
            if (MapUIInfo.selectedAllyUnit.GetComponent<AllyMove>().actionMenu)
            {
                //If buttons have not been created, create buttons and display the menu cursor
                if (!buttonsCreated)
                {
                    CreateButtons();
                }
                menuCursorInput();
            }
            //else, if the ally unit is not in an action menu state, but buttons have been created, then reset buttons
            else if (!MapUIInfo.selectedAllyUnit.GetComponent<AllyMove>().actionMenu && buttonsCreated)
            {
                ResetActionMenu(false);
            }
        }
        //else, reset buttons
        else
        {
            ResetActionMenu(false);
        }
    }

    private void CreateButtons()
    {
        buttonsCreated = true;
        menuCursor.SetActive(true);

        AllyMove am = MapUIInfo.selectedAllyUnit.GetComponent<AllyMove>();

        if (am.findingTarget)
            buttons.Add("Attack");
        buttons.Add("Item");
        buttons.Add("Wait");

        for (int i = 0; i < buttons.Count; i++)
        {
            if (buttons[i] == "Attack")
            {
                transform.Find("AttackButton").gameObject.SetActive(true);
                transform.Find("AttackButton").GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -35 * i);
            }
            else if(buttons[i] == "Item")
            {
                transform.Find("ItemButton").gameObject.SetActive(true);
                transform.Find("ItemButton").GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -35 * i);
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
        if(!selectingItems)
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
        else
        {
            if(Input.GetKeyDown(KeyCode.DownArrow))
            {
                if(menuCursorPosition <= 4)
                {
                    menuCursorPosition++;
                    menuCursor.GetComponent<RectTransform>().anchoredPosition = new Vector2(menuCursor.GetComponent<RectTransform>().anchoredPosition.x, 
                    menuCursor.GetComponent<RectTransform>().anchoredPosition.y - 24);
                }

            }
            if(Input.GetKeyDown(KeyCode.UpArrow))
            {
                if(menuCursorPosition >= 2)
                {
                    menuCursorPosition--;
                    menuCursor.GetComponent<RectTransform>().anchoredPosition = new Vector2(menuCursor.GetComponent<RectTransform>().anchoredPosition.x,
                    menuCursor.GetComponent<RectTransform>().anchoredPosition.y + 24);
                }
            }

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

    public void Item()
    {
        menuCursor.GetComponent<RectTransform>().anchoredPosition = transform.Find("ItemMenu").GetComponent<RectTransform>().anchoredPosition;
        menuCursor.GetComponent<RectTransform>().anchoredPosition = new Vector2(menuCursor.GetComponent<RectTransform>().anchoredPosition.x + 180,
            menuCursor.GetComponent<RectTransform>().anchoredPosition.y + 36);
        selectingItems = true;
        transform.Find("ItemMenu").gameObject.SetActive(true);
        menuCursorPosition = 1;
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
        menuCursor.GetComponent<Transform>().position = new Vector2(GameObject.Find("ActionMenu").transform.position.x + 100, 
            menuCursor.GetComponent<RectTransform>().anchoredPosition.y);
        selectingItems = false;
    }
}
