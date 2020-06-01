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
                MenuCursorInput();
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
        SetCursorPosition(menuCursor_RectTransform.anchoredPosition.x, -35 * (menuCursorPosition - 1));
        menuCursor.SetActive(true);

        //If unit is finding a target and can use physical attacks
        if (MapUIInfo.selectedAllyUnit_AllyMove.findingTarget && MapUIInfo.selectedAllyUnit_AllyStats.classType.usesPhysicalAttacks)
            buttons.Add("Attack");
        //if unit can use black magic
        if (MapUIInfo.selectedAllyUnit_AllyStats.blackMagic.Count > 0 && MapUIInfo.selectedAllyUnit_AllyStats.classType.usesBlackMagic)
            buttons.Add("BlackMagic");
        buttons.Add("Item");
        buttons.Add("Wait");

        //Activates buttons and positions them in descending order
        for (int i = 0; i < buttons.Count; i++)
        {
            if (buttons[i] == "Attack")
            {
                transform.Find("AttackButton").gameObject.SetActive(true);
                transform.Find("AttackButton").GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -35 * i);
            }
            else if (buttons[i] == "BlackMagic")
            {
                transform.Find("BlackMagicButton").gameObject.SetActive(true);
                transform.Find("BlackMagicButton").GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -35 * i);
            }
            else if (buttons[i] == "Item")
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

    private void MenuCursorInput()
    {
        if (selectingItems)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (menuCursorPosition <= 4)
                {
                    menuCursorPosition++;
                    MoveCursor(0, -24);
                }

            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (menuCursorPosition >= 2)
                {
                    menuCursorPosition--;
                    MoveCursor(0, 24);
                }
            }

            UpdateWeaponInfo(menuCursorPosition - 1);

            if (Input.GetKeyDown(KeyCode.Z))
            {
                SelectItem();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (menuCursorPosition < buttons.Count)
                {
                    menuCursorPosition++;
                    MoveCursor(0, -35);
                }
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (menuCursorPosition > 1)
                {
                    menuCursorPosition--;
                    MoveCursor(0, 35);
                }

            }
            if (Input.GetKeyDown(KeyCode.Z))
            {
                string methodName = buttons[menuCursorPosition - 1];
                Invoke(methodName, 0);
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
        MoveCursor(180, 36);
        selectingItems = true;
        ItemMenu.SetActive(true);
        menuCursorPosition = 1;

        //Updates every item in the inventory to match the currently selected unit's inventory
        for (int i = 0; i < 5; i++)
        {
            //If the unit's inventory slot is not empty, update name, durability, and text color
            if (unitInventory[i] != null)
            {
                UpdateInventorySlot(i, unitInventory[i].name, unitInventory[i].currentUses, unitInventory[i].maxUses);

                if (unitInventory[i].GetType() == typeof(Weapon) && ((Weapon)unitInventory[i]).equipped)
                    SetItemNameColor(i, new Color32(34, 170, 160, 255));
                else if (unitInventory[i].GetType() == typeof(Weapon) && !MapUIInfo.selectedAllyUnit_AllyStats.CanUseWeapon(i))
                    SetItemNameColor(i, Color.gray);
                else
                    SetItemNameColor(i, Color.black);
            }
            //else, set blank values
            else
                UpdateInventorySlot(i, "", 0, 0);
        }
    }

    private void UpdateWeaponInfo(int index)
    {     
        MapUIInfo.selectedAllyUnit_AllyMove.RemoveSelectableTiles(); //TODO figure out why the fuck this is here
        Weapon tempWeapon = (Weapon)(unitInventory[index]);
        OffensiveMagic tempBlackMagic = null;

        if (MapUIInfo.selectedAllyUnit_AllyStats.UsingOffensiveMagic())
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
        menuCursor_RectTransform.anchoredPosition = ItemMenu.GetComponent<RectTransform>().anchoredPosition;
        MoveCursor(180, 36);
        selectingItems = true;
        ItemMenu.SetActive(true);
        menuCursorPosition = 1;

        //Updates every item in the inventory to match the currently selected unit's inventory
        for (int i = 0; i < 5; i++)
        {
            //If the unit's inventory slot is not empty, update name, durability, and text color
            if (i < MapUIInfo.selectedAllyUnit_AllyStats.blackMagic.Count)
            {
                UpdateInventorySlot(i, MapUIInfo.selectedAllyUnit_AllyStats.blackMagic[i].name, MapUIInfo.selectedAllyUnit_AllyStats.blackMagic[i].currentUses,
                    MapUIInfo.selectedAllyUnit_AllyStats.blackMagic[i].maxUses[MapUIInfo.selectedAllyUnit_AllyStats.skillLevels.magicLevels[(int)MagicType.BLACK]]);

                if (MapUIInfo.selectedAllyUnit_AllyStats.blackMagic[i].equipped)
                    SetItemNameColor(i, new Color32(34, 170, 160, 255));
                else
                    SetItemNameColor(i, Color.black);
            }
            //else, set blank values
            else
            {
                UpdateInventorySlot(i, "", 0, 0);
            }
        }
    }

    private void SelectItem()
    {
        //If the selected item was a weapon, and the unit doesn't already have it equipped
        if (unitInventory[menuCursorPosition - 1] != null && MapUIInfo.selectedAllyUnit_AllyStats.CanUseWeapon(menuCursorPosition - 1)
            && unitInventory[menuCursorPosition - 1].GetType() == typeof(Weapon)
            && !((Weapon)unitInventory[menuCursorPosition - 1]).equipped)
        {
            EquipWeapon();
        }
        else if (MapUIInfo.selectedAllyUnit_AllyStats.UsingOffensiveMagic() && (menuCursorPosition - 1) < MapUIInfo.selectedAllyUnit_AllyStats.blackMagic.Count)
        {
            MapUIInfo.selectedAllyUnit_AllyStats.EquipBlackMagic(menuCursorPosition - 1);
            Attack();
        }
    }

    //Equips the current highlighted weapon in the inventory
    private void EquipWeapon()
    {
        //Iterates through each inventory slot
        for (int i = 0; i < 5; i++)
        {
            //If the item is not null and equal to the equipped weapon
            if (unitInventory[i] != null && unitInventory[i] == MapUIInfo.selectedAllyUnit_AllyStats.equippedWeapon)
            {
                SetItemNameColor(i, Color.black);
                i = 5;
            }
        }

        MapUIInfo.selectedAllyUnit_AllyStats.EquipWeapon(menuCursorPosition - 1);

        SetItemNameColor(menuCursorPosition - 1, new Color32(34, 170, 160, 255));
        Debug.Log("Equpped " + unitInventory[menuCursorPosition - 1].name);
    }

    private void ResetActionMenu(bool removeSelectedAllyUnit)
    {
        selectingItems = false;
        buttonsCreated = false;
        SetCursorPosition(menuCursor_RectTransform.anchoredPosition.x, 0);
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

    //moves the menu cursor
    private void MoveCursor(float x, float y)
    {
        menuCursor_RectTransform.anchoredPosition = new Vector2(menuCursor_RectTransform.anchoredPosition.x + x, menuCursor_RectTransform.anchoredPosition.y + y);
    }

    //Sets position of the menu cursor
    private void SetCursorPosition(float x, float y)
    {
        menuCursor_RectTransform.anchoredPosition = new Vector2(x, y);
    }

    //returns Transform of the nth inventory slot
    private Transform GetInventorySlot(int n)
    {
        return ItemMenu.transform.GetChild(n);
    }
    //Updates name and uses of item at inventory slot n
    private void UpdateInventorySlot(int n, string name, int uses, int maxUses)
    {
        GetInventorySlot(n).GetChild(0).GetComponent<TextMeshProUGUI>().text = name;
        if (maxUses == 0)
            GetInventorySlot(n).GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
        else
            GetInventorySlot(n).GetChild(1).GetComponent<TextMeshProUGUI>().text = uses + "/" + maxUses;
    }
    //Updates color of the item's name at inventory slot n
    private void SetItemNameColor(int n, Color color)
    {
        GetInventorySlot(n).GetChild(0).GetComponent<TextMeshProUGUI>().color = color;
    }


}
