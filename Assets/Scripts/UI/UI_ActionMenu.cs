using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_ActionMenu : MonoBehaviour
{
    //References to other shit
    private MapUIInfo MapUIInfo;
    private GameObject ItemMenu; //Holds and displays items
    private Transform WeaponInfo; //Displays info about the highlighted weapon 
    public GameObject MenuCursor; //Cursor used for selecting items
    private RectTransform MenuCursor_RectTransform; //menuCursor RectTransform

    //Local variables
    private bool buttonsCreated = false; //Whether or not buttons have beencreated
    List<string> buttons = new List<string>(); //List of buttons
    private int menuCursorPosition = 1; //Position of the cursor, where 1 is at the top
    public bool itemMenuOpen = false; //If true, player has selected the Items button and is now going through the list of items
    public bool checkingItems, checkingBlackMagic, checkingWhiteMagic = false; //Whether the player is selecting from the item, black magic, or white magic list
    private UI_ActionMenuDisplay actionMenuDisplay;

    //Shorthand variables to make this shit more fucking readable
    Item[] unitInventory; //inventory of the currently selected unit

    private void Start()
    {
        MenuCursor = transform.Find("ActionMenuCursor").gameObject;
        MapUIInfo = GetComponentInParent<MapUIInfo>();
        ItemMenu = GameObject.Find("ItemMenu");
        WeaponInfo = GameObject.Find("WeaponInfo").transform;
        MenuCursor_RectTransform = MenuCursor.GetComponent<RectTransform>();
        actionMenuDisplay = GetComponent<UI_ActionMenuDisplay>();
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
        MenuCursor.SetActive(true);
        buttonsCreated = true;
        SetCursorPosition(MenuCursor_RectTransform.anchoredPosition.x, -35 * (menuCursorPosition - 1));

        //If unit is finding a target and can use physical attacks
        if (MapUIInfo.selectedAllyUnit_AllyMove.findingTarget && MapUIInfo.selectedAllyUnit_AllyStats.classType.usesPhysicalAttacks)
            buttons.Add("Attack");
        //if unit can use black magic
        if (MapUIInfo.selectedAllyUnit_AllyStats.blackMagic.Count > 0 && MapUIInfo.selectedAllyUnit_AllyStats.classType.usesBlackMagic)
            buttons.Add("BlackMagic");
        if (MapUIInfo.selectedAllyUnit_AllyStats.whiteMagic.Count > 0 && MapUIInfo.selectedAllyUnit_AllyStats.classType.usesWhiteMagic)
            buttons.Add("WhiteMagic");
        buttons.Add("Item");
        buttons.Add("Wait");

        //Activates buttons and positions them in descending order
        for (int i = 0; i < buttons.Count; i++)
        {
            actionMenuDisplay.SetButtonActive(buttons[i], true);
            actionMenuDisplay.SetButtonPosition(buttons[i], 0, -35 * i);
        }
        GetComponent<RectTransform>().anchoredPosition = new Vector2(GetComponent<RectTransform>().anchoredPosition.x, 17.5f * (buttons.Count - 1));
    }

    private void MenuCursorInput()
    {
        if (itemMenuOpen)
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

            UpdateItemSlotInfo(menuCursorPosition - 1);

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
        MenuCursor_RectTransform.anchoredPosition = ItemMenu.GetComponent<RectTransform>().anchoredPosition;
        MoveCursor(180, 36);
        itemMenuOpen = true;
        checkingItems = true;
        ItemMenu.SetActive(true);
        menuCursorPosition = 1;

        //Updates every item in the inventory to match the currently selected unit's inventory
        for (int i = 0; i < 5; i++)
        {
            //If the unit's inventory slot is not empty, update name, durability, and text color
            if (unitInventory[i] != null)
            {
                UpdateItemSlot(i, unitInventory[i].name, unitInventory[i].currentUses, unitInventory[i].maxUses);

                if (unitInventory[i].GetType() == typeof(Weapon) && ((Weapon)unitInventory[i]).equipped)
                    SetItemNameColor(i, new Color32(34, 170, 160, 255));
                else if (unitInventory[i].GetType() == typeof(Weapon) && !MapUIInfo.selectedAllyUnit_AllyStats.CanUseWeapon(i))
                    SetItemNameColor(i, Color.gray);
                else
                    SetItemNameColor(i, Color.black);
            }
            //else, set blank values
            else
                UpdateItemSlot(i, "", 0, 0);
        }
    }

    private void UpdateItemSlotInfo(int index)
    {
        string damage, hitRate, critRate, range, heal;
        if(checkingItems)
        {
            if(unitInventory[index] && unitInventory[index].GetType() == typeof(Weapon))
            {
                Weapon weapon = ((Weapon)unitInventory[index]);
                damage = weapon.dmg.ToString();
                hitRate = weapon.hitRate.ToString();
                critRate = weapon.critRate.ToString();
                if (weapon.minRange == weapon.maxRange)
                    range = weapon.minRange.ToString();
                else
                    range = weapon.minRange + "-" + weapon.maxRange;

                actionMenuDisplay.UpdateItemInfo(damage, hitRate, critRate, range);
            }
            else
            {
                actionMenuDisplay.UpdateItemInfo("-", "-", "-", "-");
            }
        } 
        else if(checkingBlackMagic)
        {
            if(index < MapUIInfo.selectedAllyUnit_AllyStats.blackMagic.Count)
            {
                OffensiveMagic blackMagic = (OffensiveMagic)MapUIInfo.selectedAllyUnit_AllyStats.blackMagic[index];
                damage = blackMagic.dmg.ToString();
                hitRate = blackMagic.hitRate.ToString();
                critRate = "0";
                if (blackMagic.minRange == blackMagic.maxRange)
                    range = blackMagic.minRange.ToString();
                else
                    range = blackMagic.minRange + "-" + blackMagic.maxRange;

                actionMenuDisplay.UpdateItemInfo(damage, hitRate, critRate, range);
            }
            else
            {
                actionMenuDisplay.UpdateItemInfo("-", "-", "-", "-");
            }
        }
        else if(checkingWhiteMagic)
        {
            if(index < MapUIInfo.selectedAllyUnit_AllyStats.whiteMagic.Count)
            {

                HealingMagic whiteMagic = (HealingMagic)MapUIInfo.selectedAllyUnit_AllyStats.blackMagic[index];
                heal = whiteMagic.heal.ToString();
                hitRate = whiteMagic.hitRate.ToString();
                critRate = "0";
                if (whiteMagic.minRange == whiteMagic.maxRange)
                    range = whiteMagic.minRange.ToString();
                else
                    range = whiteMagic.minRange + "-" + whiteMagic.maxRange;

                actionMenuDisplay.UpdateItemInfo(heal, hitRate, critRate, range);
            }
            else
            {
                actionMenuDisplay.UpdateItemInfo("-", "-", "-", "-");
            }
        }
        /*
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
            */
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
        MenuCursor_RectTransform.anchoredPosition = ItemMenu.GetComponent<RectTransform>().anchoredPosition;
        MoveCursor(180, 36);
        itemMenuOpen = true;
        checkingBlackMagic = true;
        ItemMenu.SetActive(true);
        menuCursorPosition = 1;

        //Updates every item in the inventory to match the currently selected unit's inventory
        for (int i = 0; i < 5; i++)
        {
            //If the unit's inventory slot is not empty, update name, durability, and text color
            if (i < MapUIInfo.selectedAllyUnit_AllyStats.blackMagic.Count)
            {
                UpdateItemSlot(i, MapUIInfo.selectedAllyUnit_AllyStats.blackMagic[i].name, MapUIInfo.selectedAllyUnit_AllyStats.blackMagic[i].currentUses,
                    MapUIInfo.selectedAllyUnit_AllyStats.blackMagic[i].maxUses[MapUIInfo.selectedAllyUnit_AllyStats.skillLevels.magicLevels[(int)MagicType.BLACK]]);

                if (MapUIInfo.selectedAllyUnit_AllyStats.blackMagic[i].equipped)
                    SetItemNameColor(i, new Color32(34, 170, 160, 255));
                else
                    SetItemNameColor(i, Color.black);
            }
            //else, set blank values
            else
            {
                UpdateItemSlot(i, "", 0, 0);
            }
        }
    }

    private void WhiteMagic()
    {
        Debug.Log("selected white magic");
        MenuCursor_RectTransform.anchoredPosition = ItemMenu.GetComponent<RectTransform>().anchoredPosition;
        MoveCursor(180, 36);
        itemMenuOpen = true;
        checkingWhiteMagic = true;
        ItemMenu.SetActive(true);
        menuCursorPosition = 1;

        for (int i = 0; i < 5; i++)
        {
            //If the unit's inventory slot is not empty, update name, durability, and text color
            if (i < MapUIInfo.selectedAllyUnit_AllyStats.whiteMagic.Count)
            {
                UpdateItemSlot(i, MapUIInfo.selectedAllyUnit_AllyStats.whiteMagic[i].name, MapUIInfo.selectedAllyUnit_AllyStats.whiteMagic[i].currentUses,
                    MapUIInfo.selectedAllyUnit_AllyStats.whiteMagic[i].maxUses[MapUIInfo.selectedAllyUnit_AllyStats.skillLevels.magicLevels[(int)MagicType.WHITE]]);

                if (MapUIInfo.selectedAllyUnit_AllyStats.whiteMagic[i].equipped)
                    SetItemNameColor(i, new Color32(34, 170, 160, 255));
                else
                    SetItemNameColor(i, Color.black);
            }
            //else, set blank values
            else
            {
                UpdateItemSlot(i, "", 0, 0);
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
        itemMenuOpen = false;
        buttonsCreated = false;
        itemMenuOpen = buttonsCreated = checkingItems = checkingBlackMagic = checkingWhiteMagic = false;
        SetCursorPosition(MenuCursor_RectTransform.anchoredPosition.x, 0);
        menuCursorPosition = 1;

        if (removeSelectedAllyUnit)
            MapUIInfo.selectedAllyUnit = null;

        buttons.Clear();

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.activeInHierarchy)
                transform.GetChild(i).gameObject.SetActive(false);
        }

        MenuCursor.GetComponent<Transform>().position = new Vector2(GameObject.Find("ActionMenu").transform.position.x + 100,
            MenuCursor.GetComponent<RectTransform>().anchoredPosition.y);

    }

    //moves the menu cursor
    private void MoveCursor(float x, float y)
    {
        actionMenuDisplay.MoveCursorPosition(x, y);
    }

    //Sets position of the menu cursor
    private void SetCursorPosition(float x, float y)
    {
        actionMenuDisplay.SetCursorPosition(x, y);
        //MenuCursor_RectTransform.anchoredPosition = new Vector2(x, y);
    }
    //Updates name and uses of item at inventory slot n
    private void UpdateItemSlot(int n, string name, int uses, int maxUses)
    {
        if (maxUses == 0)
            actionMenuDisplay.UpdateItemSlot(n, name, "");
        else
            actionMenuDisplay.UpdateItemSlot(n, name, uses + "/" + maxUses);
    }
    //Updates color of the item's name at inventory slot n
    private void SetItemNameColor(int n, Color color)
    {
        actionMenuDisplay.UpdateItemColor(n, color);
    }


}
