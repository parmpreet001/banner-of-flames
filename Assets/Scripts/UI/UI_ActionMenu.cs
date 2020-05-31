using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_ActionMenu : MonoBehaviour
{
    //References to other shit
    private MapUIInfo MapUIInfo; 
    private GameObject ItemMenu; //Holds and displays items
    private Transform WeaponInfo; //Displays info about the highlighted weapon 
    public GameObject menuCursor; //Cursor used for selecting items
    private RectTransform menuCursor_RectTransform; //menuCursor RectTransform
    
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
        WeaponInfo = GameObject.Find("WeaponInfo").transform;
        menuCursor_RectTransform = menuCursor.GetComponent<RectTransform>();
    }

    void Update()
    {
        //If an ally unit is selected
        if (MapUIInfo.selectedAllyUnit)
        {
            //If they're in an action menu state
            if (MapUIInfo.selectedAllyUnit_AllyMove.actionMenu)
            {
                //If buttons have not been created, create buttons and display the menu cursor
                if (!buttonsCreated)
                {
                    CreateButtons();
                }
                menuCursorInput();
                unitInventory = MapUIInfo.selectedAllyUnit_AllyStats.inventory;
            }
            //else, if the ally unit is not in an action menu state, but buttons have been created, then reset buttons
            else if (!MapUIInfo.selectedAllyUnit_AllyMove.actionMenu && buttonsCreated)
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
        //menuCursor_RectTransform.anchoredPosition = new Vector2(menuCursor_RectTransform.anchoredPosition.x, -35 * (menuCursorPosition - 1));
        SetCursor(menuCursor_RectTransform.anchoredPosition.x, -35 * (menuCursorPosition - 1));
        menuCursor.SetActive(true);

        if (MapUIInfo.selectedAllyUnit_AllyMove.findingTarget && MapUIInfo.selectedAllyUnit_AllyStats.classType.usesPhysicalAttacks)
            buttons.Add("Attack");
        if (MapUIInfo.selectedAllyUnit_AllyStats.blackMagic.Count > 0 && MapUIInfo.selectedAllyUnit_AllyStats.classType.usesBlackMagic)
            buttons.Add("BlackMagic");
        buttons.Add("Item");
        buttons.Add("Wait");

        for (int i = 0; i < buttons.Count; i++)
        {
            if (buttons[i] == "Attack")
            {
                transform.Find("AttackButton").gameObject.SetActive(true);
                transform.Find("AttackButton").GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -35 * i);
            }
            else if(buttons[i] == "BlackMagic")
            {
                transform.Find("BlackMagicButton").gameObject.SetActive(true);
                transform.Find("BlackMagicButton").GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -35 * i);
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

            UpdateWeaponInfo(menuCursorPosition-1);

            if (Input.GetKeyDown(KeyCode.Z))
            {
                selectItem();
            }
        }
    }

    public void Wait()
    {
        MapUIInfo.selectedAllyUnit_AllyMove.UnselectUnit();
        MapUIInfo.selectedAllyUnit_AllyMove.finished = true;
        ResetActionMenu(true);
    }

    public void Attack()
    {
        MapUIInfo.selectedAllyUnit_AllyMove.actionMenu = false;
        ResetActionMenu(false);
    }

    //Opens the item menu
    public void Item()
    {
        menuCursor_RectTransform.anchoredPosition = ItemMenu.GetComponent<RectTransform>().anchoredPosition;
        moveCursor(180, 36);
        //menuCursor_RectTransform.anchoredPosition = new Vector2(menuCursor_RectTransform.anchoredPosition.x + 180, menuCursor_RectTransform.anchoredPosition.y + 36);
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
                GetInventorySlot(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = unitInventory[i].currentUses + "/" + unitInventory[i].maxUses;
                if(unitInventory[i].GetType() == typeof(Weapon) && ((Weapon)unitInventory[i]).equipped)
                {
                    GetInventorySlot(i).GetComponent<TextMeshProUGUI>().color = new Color32(34, 170, 160, 255);
                }
                else if(unitInventory[i].GetType() == typeof(Weapon) && !MapUIInfo.selectedAllyUnit_AllyStats.CanUseWeapon(i))
                {
                    GetInventorySlot(i).GetComponent<TextMeshProUGUI>().color = Color.gray;
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

    private void UpdateWeaponInfo(int index)
    {
        MapUIInfo.selectedAllyUnit_AllyMove.RemoveSelectableTiles();
        Weapon tempWeapon = null;
        OffensiveMagic tempBlackMagic = null;

        if (MapUIInfo.selectedAllyUnit_AllyStats.usingBlackMagic)
        {
            if (index < MapUIInfo.selectedAllyUnit_AllyStats.blackMagic.Count)
            {
                tempBlackMagic = ((OffensiveMagic)MapUIInfo.selectedAllyUnit_AllyStats.blackMagic[index]);
                string range;

                if (tempBlackMagic.minRange == tempBlackMagic.maxRange)
                    range = tempBlackMagic.minRange.ToString();
                else
                    range = tempBlackMagic.minRange + " - " + tempBlackMagic.maxRange;

                UpdateWeaponInfoText(tempBlackMagic.dmg.ToString(), tempBlackMagic.hitRate.ToString(), "0", range);
                MapUIInfo.selectedAllyUnit_AllyMove.ShowWeaponRange(tempBlackMagic.minRange, tempBlackMagic.maxRange);
            }
            else
                UpdateWeaponInfoText("-", "-", "-", "-");
        }

        else if (tempWeapon != null)
        {
            tempWeapon = ((Weapon)unitInventory[index]);
            string range;

            if (tempWeapon.minRange == tempWeapon.maxRange)
                range = tempWeapon.minRange.ToString();
            else
                range = tempWeapon.minRange + " - " + tempWeapon.maxRange;

            UpdateWeaponInfoText(tempWeapon.dmg.ToString(), tempWeapon.hitRate.ToString(), tempWeapon.critRate.ToString(), range);
            MapUIInfo.selectedAllyUnit_AllyMove.ShowWeaponRange(tempWeapon.minRange, tempWeapon.maxRange);
        }
        else
            UpdateWeaponInfoText("-", "-", "-", "-");
    }

    private void UpdateWeaponInfoText(string dmg, string hitRate, string critRate, string range)
    {
        WeaponInfo.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = dmg;
        WeaponInfo.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = hitRate;
        WeaponInfo.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = critRate;
        WeaponInfo.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = range;
    }

    private void BlackMagic()
    {
        MapUIInfo.selectedAllyUnit_AllyStats.usingBlackMagic = true;
        menuCursor_RectTransform.anchoredPosition = ItemMenu.GetComponent<RectTransform>().anchoredPosition;
        moveCursor(180, 36);
        //menuCursor_RectTransform.anchoredPosition = new Vector2(menuCursor_RectTransform.anchoredPosition.x + 180, menuCursor_RectTransform.anchoredPosition.y + 36);
        selectingItems = true;
        ItemMenu.SetActive(true);
        menuCursorPosition = 1;

        //Updates every item in the inventory to match the currently selected unit's inventory
        for (int i = 0; i < 5; i++)
        {
            //If the unit's inventory slot is not empty, update name, durability, and text color
            if (i < MapUIInfo.selectedAllyUnit_AllyStats.blackMagic.Count)
            {
                GetInventorySlot(i).GetComponent<TextMeshProUGUI>().text = MapUIInfo.selectedAllyUnit_AllyStats.blackMagic[i].name;
                GetInventorySlot(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = MapUIInfo.selectedAllyUnit_AllyStats.blackMagic[i].currentUses + "/"
                    + MapUIInfo.selectedAllyUnit_AllyStats.blackMagic[i].maxUses[MapUIInfo.selectedAllyUnit_AllyStats.skillLevels.magicLevels[(int)MagicType.BLACK]];

                if(MapUIInfo.selectedAllyUnit_AllyStats.blackMagic[i].equipped)
                    GetInventorySlot(i).GetComponent<TextMeshProUGUI>().color = new Color32(34, 170, 160, 255);
                else
                    GetInventorySlot(i).GetComponent<TextMeshProUGUI>().color = Color.black;
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
        if(unitInventory[menuCursorPosition - 1] != null && MapUIInfo.selectedAllyUnit_AllyStats.CanUseWeapon(menuCursorPosition-1)
            && unitInventory[menuCursorPosition-1].GetType() == typeof(Weapon)
            && !((Weapon)unitInventory[menuCursorPosition-1]).equipped)
        {
            EquipWeapon();
        }
        else if(MapUIInfo.selectedAllyUnit_AllyStats.usingBlackMagic && (menuCursorPosition-1) < MapUIInfo.selectedAllyUnit_AllyStats.blackMagic.Count)
        {
            MapUIInfo.selectedAllyUnit_AllyStats.EquipBlackMagic(menuCursorPosition - 1);
            Attack();
        }
    }

    private void EquipWeapon()
    {
        //Iterates through each inventory slot
        for (int i = 0; i < 5; i++)
        {
            //If the item is not null and equal to the equipped weapon
            if (unitInventory[i] != null && unitInventory[i] == MapUIInfo.selectedAllyUnit_AllyStats.equippedWeapon)
            {
                GetInventorySlot(i).GetComponent<TextMeshProUGUI>().color = Color.black;
                i = 5;
            }
        }

        MapUIInfo.selectedAllyUnit_AllyStats.EquipWeapon(menuCursorPosition - 1);

        GetInventorySlot(menuCursorPosition - 1).GetComponent<TextMeshProUGUI>().color = new Color32(34, 170, 160, 255);
        Debug.Log("Equpped " + unitInventory[menuCursorPosition - 1].name);
    }

    private void ResetActionMenu(bool removeSelectedAllyUnit)
    {
        selectingItems = false;
        buttonsCreated = false;
        SetCursor(menuCursor_RectTransform.anchoredPosition.x, 0);
        //menuCursor_RectTransform.anchoredPosition = new Vector2(menuCursor_RectTransform.anchoredPosition.x, 0);
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
        menuCursor_RectTransform.anchoredPosition = new Vector2(menuCursor_RectTransform.anchoredPosition.x + x, menuCursor_RectTransform.anchoredPosition.y + y);
    }

    private void SetCursor(float x, float y)
    {
        menuCursor_RectTransform.anchoredPosition = new Vector2(x, y);
    }

    //Getter methods
    
    //returns Transform of the nth inventory slot
    private Transform GetInventorySlot(int n)
    {
        return ItemMenu.transform.GetChild(n);
    }
}
