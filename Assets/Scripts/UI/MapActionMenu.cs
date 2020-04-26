using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MapActionMenu : MonoBehaviour
{
    private MapUIInfo MapUIInfo;
    private GameObject ItemMenu;

    private bool buttonsCreated = false; //Whether or not buttons have beencreated
    List<string> buttons = new List<string>(); //List of buttons
    
    public GameObject menuCursor; 
    private int menuCursorPosition = 1; //Position of the cursor, where 1 is at the top
    
    private bool selectingItems = false; //If true, player has selected the Items button and is now going through the list of items

    private RectTransform menuCursorRT; //menuCursor RectTransform

    private void Start()
    {
        MapUIInfo = GetComponentInParent<MapUIInfo>();
        ItemMenu = GameObject.Find("ItemMenu");
        menuCursorRT = menuCursor.GetComponent<RectTransform>();
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
        menuCursorRT.anchoredPosition = new Vector2(menuCursorRT.anchoredPosition.x, -35 * (menuCursorPosition - 1)); 
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
                {
                    menuCursorPosition++;
                    moveCursor(0, -35);
                }      
            }
            if(Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (menuCursorPosition > 1)
                {
                    menuCursorPosition--;
                    moveCursor(0, 35);
                }
                    
            }
            if(Input.GetKeyDown(KeyCode.Z))
            {
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
                    moveCursor(0, -24);
                }

            }
            if(Input.GetKeyDown(KeyCode.UpArrow))
            {
                if(menuCursorPosition >= 2)
                {
                    menuCursorPosition--;
                    moveCursor(0, 24);
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
        menuCursorRT.anchoredPosition = ItemMenu.GetComponent<RectTransform>().anchoredPosition;
        menuCursorRT.anchoredPosition = new Vector2(menuCursorRT.anchoredPosition.x + 180, menuCursorRT.anchoredPosition.y + 36);
        selectingItems = true;
        ItemMenu.SetActive(true);
        menuCursorPosition = 1;

        ItemMenu.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = MapUIInfo.selectedAllyUnit.GetComponent<AllyStats>().inventory[0].name;
        ItemMenu.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = MapUIInfo.selectedAllyUnit.GetComponent<AllyStats>().inventory[0].currentUse +
            "/" + MapUIInfo.selectedAllyUnit.GetComponent<AllyStats>().inventory[0].maxUse;
    }

    private void ResetActionMenu(bool removeSelectedAllyUnit)
    {
        selectingItems = false;
        buttonsCreated = false;
        menuCursorRT.anchoredPosition = new Vector2(menuCursorRT.anchoredPosition.x, 0);
        menuCursorPosition = 1;

        if (removeSelectedAllyUnit)
            MapUIInfo.selectedAllyUnit = null;
        
        buttons.Clear();

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.activeInHierarchy)
                transform.GetChild(i).gameObject.SetActive(false);
        }

        menuCursor.GetComponent<Transform>().position = new Vector2(GameObject.Find("ActionMenu").transform.position.x + 100,
            menuCursor.GetComponent<RectTransform>().anchoredPosition.y);
        
    }

    private void moveCursor(float x, float y)
    {
        menuCursorRT.anchoredPosition = new Vector2(menuCursorRT.anchoredPosition.x + x, menuCursorRT.anchoredPosition.y + y);
    }
}
