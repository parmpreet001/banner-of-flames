using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MapActionMenu : MonoBehaviour
{
    //References to other shit
    private MapUIInfo MapUIInfo;
    private GameObject ItemMenu;
    public GameObject menuCursor; 
    private RectTransform menuCursorRT; //menuCursor RectTransform
    
    //Local variables
    private bool buttonsCreated = false; //Whether or not buttons have beencreated
    List<string> buttons = new List<string>(); //List of buttons
    private int menuCursorPosition = 1; //Position of the cursor, where 1 is at the top
    private bool selectingItems = false; //If true, player has selected the Items button and is now going through the list of items

    //Shorthand variables to make this shit more fucking readable
    Item[] unitInventory; //inventory of the currently selected unit

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
                unitInventory = MapUIInfo.selectedAllyUnit.GetComponent<Stats>().inventory;
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
        //Method variables
        AllyMove am = MapUIInfo.selectedAllyUnit.GetComponent<AllyMove>();
        
        buttonsCreated = true;
        menuCursorRT.anchoredPosition = new Vector2(menuCursorRT.anchoredPosition.x, -35 * (menuCursorPosition - 1)); 
        menuCursor.SetActive(true);

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

            if (Input.GetKeyDown(KeyCode.Z))
            {
                selectItem();
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

    //Opens the item menu
    public void Item()
    {
        menuCursorRT.anchoredPosition = ItemMenu.GetComponent<RectTransform>().anchoredPosition;
        menuCursorRT.anchoredPosition = new Vector2(menuCursorRT.anchoredPosition.x + 180, menuCursorRT.anchoredPosition.y + 36);
        selectingItems = true;
        ItemMenu.SetActive(true);
        menuCursorPosition = 1;

        //Updates every item in the inventory to match the currently selected unit's inventory
        for(int i = 0; i < 5; i++)
        {
            //If the unit's inventory slot is not empty, update name, durability, and text color
            if(unitInventory[i] != null)
            {
                GetInventorySlot(i).GetComponent<TextMeshProUGUI>().text = unitInventory[i].name;
                GetInventorySlot(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = unitInventory[i].currentUse + "/" + unitInventory[i].maxUse;
                if(unitInventory[i].GetType() == typeof(Weapon) && ((Weapon)unitInventory[i]).equipped)
                {
                    GetInventorySlot(i).GetComponent<TextMeshProUGUI>().color = new Color32(34, 170, 160, 255);
                }
                else
                {
                    GetInventorySlot(i).GetComponent<TextMeshProUGUI>().color = Color.black;
                }
            }
            //else, set blank values
            else
            {
                GetInventorySlot(i).GetComponent<TextMeshProUGUI>().text = "";
                GetInventorySlot(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = "";
            }
        }
    }

    private void selectItem()
    {
        //If the selected item was a weapon, and the unit doesn't already have it equipped
        if(unitInventory[menuCursorPosition - 1] != null && unitInventory[menuCursorPosition-1].GetType() == typeof(Weapon)
            && !((Weapon)unitInventory[menuCursorPosition-1]).equipped)
        {
            EquipWeapon();
        }
    }

    private void EquipWeapon()
    {
        //Iterates through each inventory slot
        for (int i = 0; i < 5; i++)
        {
            //If the item is not null and equal to the equipped weapon
            if (unitInventory[i] != null && unitInventory[i] == MapUIInfo.selectedAllyUnit.GetComponent<Stats>().equippedWeapon)
            {
                GetInventorySlot(i).GetComponent<TextMeshProUGUI>().color = Color.black;
                i = 5;
            }
        }

        MapUIInfo.selectedAllyUnit.GetComponent<Stats>().EquipWeapon(menuCursorPosition - 1);

        GetInventorySlot(menuCursorPosition - 1).GetComponent<TextMeshProUGUI>().color = new Color32(34, 170, 160, 255);
        Debug.Log("Equpped " + unitInventory[menuCursorPosition - 1].name);
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

    //Getter methods
    
    //returns Transform of the nth inventory slot
    private Transform GetInventorySlot(int n)
    {
        return ItemMenu.transform.GetChild(n);
    }
}
