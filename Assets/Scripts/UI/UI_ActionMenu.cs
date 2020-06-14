using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_ActionMenu : MonoBehaviour
{
    //References to other shit
    private MapUIInfo MapUIInfo;
    private GameObject ItemMenu; //Holds and displays items
    public GameObject MenuCursor; //Cursor used for selecting items
    private RectTransform MenuCursor_RectTransform; //menuCursor RectTransform

    //Local variables
    private bool buttonsCreated = false; //Whether or not buttons have beencreated
    List<string> buttons = new List<string>(); //List of buttons
    private int menuCursorPosition = 1; //Position of the cursor, where 1 is at the top
    public bool itemMenuOpen = false; //If true, player has selected the Items button and is now going through the list of items
    public bool checkingItems, checkingBlackMagic, checkingWhiteMagic = false; //Whether the player is selecting from the item, black magic, or white magic list
    private UI_ActionMenuDisplay actionMenuDisplay;

    //Variables derived from MapUIInfo
    Item[] unitInventory; //inventory of the currently selected unit

    private void Start()
    {
        MenuCursor = transform.Find("ActionMenuCursor").gameObject;
        MapUIInfo = GetComponentInParent<MapUIInfo>();
        ItemMenu = GameObject.Find("ItemMenu");
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
        MapUIInfo.selectedAllyUnit_AllyMove.RemoveSelectableTiles();
        MenuCursor.SetActive(true);
        buttonsCreated = true;
        actionMenuDisplay.SetCursorPosition(MenuCursor_RectTransform.anchoredPosition.x, -35 * (menuCursorPosition - 1));

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
                    actionMenuDisplay.MoveCursorPosition(0, -24);
                }
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (menuCursorPosition >= 2)
                {
                    menuCursorPosition--;
                    actionMenuDisplay.MoveCursorPosition(0, 24);
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
                    actionMenuDisplay.MoveCursorPosition(0, -35);
                }
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (menuCursorPosition > 1)
                {
                    menuCursorPosition--;
                    actionMenuDisplay.MoveCursorPosition(0, 35);
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
        actionMenuDisplay.MoveCursorPosition(180, 36);
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
                    actionMenuDisplay.UpdateItemColor(i, new Color32(34, 170, 160, 255));
                else if (unitInventory[i].GetType() == typeof(Weapon) && !MapUIInfo.selectedAllyUnit_AllyStats.CanUseWeapon(i))
                    actionMenuDisplay.UpdateItemColor(i, Color.gray);
                else
                    actionMenuDisplay.UpdateItemColor(i, Color.black);
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
            actionMenuDisplay.itemInfo.staticTextDmgHeal.text = "Damage";
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
                actionMenuDisplay.UpdateItemInfo("-", "-", "-", "-");
        } 
        else if(checkingBlackMagic)
        {
            actionMenuDisplay.itemInfo.staticTextDmgHeal.text = "Damage";
            if (index < MapUIInfo.selectedAllyUnit_AllyStats.blackMagic.Count)
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
                actionMenuDisplay.UpdateItemInfo("-", "-", "-", "-");
        }
        else if(checkingWhiteMagic)
        {
            actionMenuDisplay.itemInfo.staticTextDmgHeal.text = "Heal";
            if(index < MapUIInfo.selectedAllyUnit_AllyStats.whiteMagic.Count)
            {

                HealingMagic whiteMagic = (HealingMagic)MapUIInfo.selectedAllyUnit_AllyStats.whiteMagic[index];
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
                actionMenuDisplay.UpdateItemInfo("-", "-", "-", "-");
        }
    }

    private void BlackMagic()
    {
        MenuCursor_RectTransform.anchoredPosition = ItemMenu.GetComponent<RectTransform>().anchoredPosition;
        actionMenuDisplay.MoveCursorPosition(180, 36);
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
                    actionMenuDisplay.UpdateItemColor(i, new Color32(34, 170, 160, 255));
                else
                    actionMenuDisplay.UpdateItemColor(i, Color.black);
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
        actionMenuDisplay.MoveCursorPosition(180, 36);
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
                    actionMenuDisplay.UpdateItemColor(i, new Color32(34, 170, 160, 255));
                else
                    actionMenuDisplay.UpdateItemColor(i, Color.black);
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
        if (checkingItems)
        {
            if (unitInventory[menuCursorPosition - 1] != null && unitInventory[menuCursorPosition - 1].GetType() == typeof(Weapon)
                && MapUIInfo.selectedAllyUnit_AllyStats.CanUseWeapon(menuCursorPosition - 1))
            {
                EquipWeapon();
            }
        }
        else if (checkingBlackMagic && (menuCursorPosition - 1) < MapUIInfo.selectedAllyUnit_AllyStats.blackMagic.Count)
        {
            EquipBlackMagic();

            if (MapUIInfo.selectedAllyUnit_AllyMove.EnemyInRange(MapUIInfo.selectedAllyUnit_AllyStats.equippedBlackMagic.minRange,
                MapUIInfo.selectedAllyUnit_AllyStats.equippedBlackMagic.maxRange))
            {
                MapUIInfo.selectedAllyUnit_AllyMove.ShowWeaponRange(MapUIInfo.selectedAllyUnit_AllyStats.equippedBlackMagic.minRange,
                    MapUIInfo.selectedAllyUnit_AllyStats.equippedBlackMagic.maxRange);
                Attack();
            }
        }
        else if (checkingWhiteMagic)
        {
            HealingMagic temp = (HealingMagic)MapUIInfo.selectedAllyUnit_AllyStats.whiteMagic[menuCursorPosition - 1];
            if(temp && MapUIInfo.selectedAllyUnit_AllyMove.AllyInRange(temp.minRange, temp.maxRange))
            {
                MapUIInfo.selectedAllyUnit_AllyMove.findingAlly = true;
                MapUIInfo.selectedAllyUnit_AllyMove.FindHealableTiles(temp.minRange, temp.maxRange);
                Attack();
            }
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
                actionMenuDisplay.UpdateItemColor(i, Color.black);
                i = 5;
            }
        }

        MapUIInfo.selectedAllyUnit_AllyStats.EquipWeapon(menuCursorPosition - 1);

        actionMenuDisplay.UpdateItemColor(menuCursorPosition - 1, new Color32(34, 170, 160, 255));
        Debug.Log("Equpped " + unitInventory[menuCursorPosition - 1].name);
    }

    private void EquipBlackMagic()
    {
        MapUIInfo.selectedAllyUnit_AllyStats.EquipBlackMagic(menuCursorPosition - 1);
        for(int i = 0; i < MapUIInfo.selectedAllyUnit_AllyStats.blackMagic.Count; i++)
        {
            if(MapUIInfo.selectedAllyUnit_AllyStats.blackMagic[i].equipped)
            {
                actionMenuDisplay.UpdateItemColor(i, new Color32(34, 170, 160, 255));
            }
            else
            {
                actionMenuDisplay.UpdateItemColor(i, Color.black);
            }
        }
    }

    private void ResetActionMenu(bool removeSelectedAllyUnit)
    {
        itemMenuOpen = false;
        buttonsCreated = false;
        itemMenuOpen = buttonsCreated = checkingItems = checkingBlackMagic = checkingWhiteMagic = false;
        actionMenuDisplay.SetCursorPosition(MenuCursor_RectTransform.anchoredPosition.x, 0);
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

    //Updates name and uses of item at inventory slot n
    private void UpdateItemSlot(int n, string name, int uses, int maxUses)
    {
        if (maxUses == 0)
            actionMenuDisplay.UpdateItemSlot(n, name, "");
        else
            actionMenuDisplay.UpdateItemSlot(n, name, uses + "/" + maxUses);
    }
}
