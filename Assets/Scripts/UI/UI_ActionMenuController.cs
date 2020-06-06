using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_ActionMenuController : MonoBehaviour
{
    private MapUIInfo MapUIInfo;
    //private RectTransform MenuCursor_RectTransform; //menuCursor RectTransform

    //Local variables
    private bool buttonsCreated = false; //Whether or not buttons have beencreated
    List<string> buttons = new List<string>(); //List of buttons
    private bool selectingItems = false; //If true, player has selected the Items button and is now going through the list of items

    private GameObject actionMenuUI;
    private UI_ActionMenuDisplay actionMenuDisplay;

    //Shorthand variables to make this shit more fucking readable
    Item[] unitInventory; //inventory of the currently selected unit

    private void Start()
    {
        MapUIInfo = GetComponentInParent<MapUIInfo>();
        actionMenuUI = transform.GetChild(0).gameObject;
        actionMenuUI.SetActive(true);
        actionMenuDisplay =  actionMenuUI.GetComponent<UI_ActionMenuDisplay>();
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
        actionMenuDisplay.SetMenuCursorActive(true);
        buttonsCreated = true;
        actionMenuDisplay.SetCursorPosition((int)actionMenuDisplay.menuCursor_RectTransform.anchoredPosition.x, -35 * (actionMenuDisplay.menuCursorPosition - 1));

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
            switch (buttons[i])
            {
                case "Attack":
                    transform.Find("AttackButton").gameObject.SetActive(true);
                    transform.Find("AttackButton").GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -35 * i); break;
                case "BlackMagic":
                    transform.Find("BlackMagicButton").gameObject.SetActive(true);
                    transform.Find("BlackMagicButton").GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -35 * i); break;
                case "Item":
                    transform.Find("ItemButton").gameObject.SetActive(true);
                    transform.Find("ItemButton").GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -35 * i); break;
                case "Wait":
                    transform.Find("WaitButton").gameObject.SetActive(true);
                    transform.Find("WaitButton").GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -35 * i); break;
                default:
                    break;
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
                if (actionMenuDisplay.menuCursorPosition <= 4)
                {
                    actionMenuDisplay.menuCursorPosition++;
                    actionMenuDisplay.MoveCursor(0, -24);
                }

            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (actionMenuDisplay.menuCursorPosition >= 2)
                {
                    actionMenuDisplay.menuCursorPosition--;
                    actionMenuDisplay.MoveCursor(0, 24);
                }
            }

            UpdateWeaponInfo(actionMenuDisplay.menuCursorPosition - 1);

            if (Input.GetKeyDown(KeyCode.Z))
            {
                SelectItem();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (actionMenuDisplay.menuCursorPosition < buttons.Count)
                {
                    actionMenuDisplay.menuCursorPosition++;
                    actionMenuDisplay.MoveCursor(0, -35);
                }
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (actionMenuDisplay.menuCursorPosition > 1)
                {
                    actionMenuDisplay.menuCursorPosition--;
                    actionMenuDisplay.MoveCursor(0, 35);
                }

            }
            if (Input.GetKeyDown(KeyCode.Z))
            {
                string methodName = buttons[actionMenuDisplay.menuCursorPosition - 1];
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
        actionMenuDisplay.menuCursor_RectTransform.anchoredPosition = actionMenuDisplay.itemMenu.GetComponent<RectTransform>().anchoredPosition;
        //MenuCursor_RectTransform.anchoredPosition 
        actionMenuDisplay.MoveCursor(180, 36);
        selectingItems = true;
        actionMenuDisplay.SetItemMenuActive(true);
        actionMenuDisplay.menuCursorPosition = 1;

        //Updates every item in the inventory to match the currently selected unit's inventory
        for (int i = 0; i < 5; i++)
        {
            //If the unit's inventory slot is not empty, update name, durability, and text color
            if (unitInventory[i] != null)
            {
                actionMenuDisplay.UpdateInventorySlot(i, unitInventory[i].name, unitInventory[i].currentUses, unitInventory[i].maxUses);

                if (unitInventory[i].GetType() == typeof(Weapon) && ((Weapon)unitInventory[i]).equipped)
                    actionMenuDisplay.SetItemNameColor(i, new Color32(34, 170, 160, 255));
                else if (unitInventory[i].GetType() == typeof(Weapon) && !MapUIInfo.selectedAllyUnit_AllyStats.CanUseWeapon(i))
                    actionMenuDisplay.SetItemNameColor(i, Color.gray);
                else
                    actionMenuDisplay.SetItemNameColor(i, Color.black);
            }
            //else, set blank values
            else
                actionMenuDisplay.UpdateInventorySlot(i, "", 0, 0);
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

                actionMenuDisplay.UpdateWeaponInfoText(tempBlackMagic.dmg.ToString(), tempBlackMagic.hitRate.ToString(), "0", range);
                MapUIInfo.selectedAllyUnit_AllyMove.ShowWeaponRange(tempBlackMagic.minRange, tempBlackMagic.maxRange);
            }
            else
                actionMenuDisplay.UpdateWeaponInfoText("-", "-", "-", "-");
        }

        else if (tempWeapon != null)
        {
            string range;

            if (tempWeapon.minRange == tempWeapon.maxRange)
                range = tempWeapon.minRange.ToString();
            else
                range = tempWeapon.minRange + " - " + tempWeapon.maxRange;

            actionMenuDisplay.UpdateWeaponInfoText(tempWeapon.dmg.ToString(), tempWeapon.hitRate.ToString(), tempWeapon.critRate.ToString(), range);
            MapUIInfo.selectedAllyUnit_AllyMove.ShowWeaponRange(tempWeapon.minRange, tempWeapon.maxRange);
        }
        else
            actionMenuDisplay.UpdateWeaponInfoText("-", "-", "-", "-");
    }

    /*
    private void UpdateWeaponInfoText(string dmg, string hitRate, string critRate, string range)
    {
        WeaponInfo.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = dmg;
        WeaponInfo.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = hitRate;
        WeaponInfo.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = critRate;
        WeaponInfo.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = range;
    }
    */

    private void BlackMagic()
    {
        actionMenuDisplay.menuCursor_RectTransform.anchoredPosition = actionMenuDisplay.itemMenu.GetComponent<RectTransform>().anchoredPosition;
        actionMenuDisplay.MoveCursor(180, 36);
        selectingItems = true;
        actionMenuDisplay.SetItemMenuActive(true);
        actionMenuDisplay.menuCursorPosition = 1;

        //Updates every item in the inventory to match the currently selected unit's inventory
        for (int i = 0; i < 5; i++)
        {
            //If the unit's inventory slot is not empty, update name, durability, and text color
            if (i < MapUIInfo.selectedAllyUnit_AllyStats.blackMagic.Count)
            {
                actionMenuDisplay.UpdateInventorySlot(i, MapUIInfo.selectedAllyUnit_AllyStats.blackMagic[i].name, MapUIInfo.selectedAllyUnit_AllyStats.blackMagic[i].currentUses,
                    MapUIInfo.selectedAllyUnit_AllyStats.blackMagic[i].maxUses[MapUIInfo.selectedAllyUnit_AllyStats.skillLevels.magicLevels[(int)MagicType.BLACK]]);

                if (MapUIInfo.selectedAllyUnit_AllyStats.blackMagic[i].equipped)
                    actionMenuDisplay.SetItemNameColor(i, new Color32(34, 170, 160, 255));
                else
                    actionMenuDisplay.SetItemNameColor(i, Color.black);
            }
            //else, set blank values
            else
            {
                actionMenuDisplay.UpdateInventorySlot(i, "", 0, 0);
            }
        }
    }

    private void SelectItem()
    {
        //If the selected item was a weapon, and the unit doesn't already have it equipped
        if (unitInventory[actionMenuDisplay.menuCursorPosition - 1] != null && MapUIInfo.selectedAllyUnit_AllyStats.CanUseWeapon(actionMenuDisplay.menuCursorPosition - 1)
            && unitInventory[actionMenuDisplay.menuCursorPosition - 1].GetType() == typeof(Weapon)
            && !((Weapon)unitInventory[actionMenuDisplay.menuCursorPosition - 1]).equipped)
        {
            EquipWeapon();
        }
        else if (MapUIInfo.selectedAllyUnit_AllyStats.UsingOffensiveMagic() && (actionMenuDisplay.menuCursorPosition - 1) < MapUIInfo.selectedAllyUnit_AllyStats.blackMagic.Count)
        {
            MapUIInfo.selectedAllyUnit_AllyStats.EquipBlackMagic(actionMenuDisplay.menuCursorPosition - 1);
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
                actionMenuDisplay.SetItemNameColor(i, Color.black);
                i = 5;
            }
        }

        MapUIInfo.selectedAllyUnit_AllyStats.EquipWeapon(actionMenuDisplay.menuCursorPosition - 1);

        actionMenuDisplay.SetItemNameColor(actionMenuDisplay.menuCursorPosition - 1, new Color32(34, 170, 160, 255));
        Debug.Log("Equpped " + unitInventory[actionMenuDisplay.menuCursorPosition - 1].name);
    }

    private void ResetActionMenu(bool removeSelectedAllyUnit)
    {
        selectingItems = false;
        buttonsCreated = false;
        actionMenuDisplay.SetCursorPosition((int)actionMenuDisplay.menuCursor_RectTransform.anchoredPosition.x, 0);
        actionMenuDisplay.menuCursorPosition = 1;

        if (removeSelectedAllyUnit)
            MapUIInfo.selectedAllyUnit = null;

        buttons.Clear();

        for (int i = 0; i < transform.childCount; i++)
        {
            //if (transform.GetChild(i).gameObject.activeInHierarchy)
                //transform.GetChild(i).gameObject.SetActive(false);
        }

       actionMenuDisplay.menuCursor.GetComponent<Transform>().position = new Vector2(GameObject.Find("ActionMenu").transform.position.x + 100,
            actionMenuDisplay.menuCursor.GetComponent<RectTransform>().anchoredPosition.y);

    }
    /*
    //moves the menu cursor
    private void MoveCursor(float x, float y)
    {
        MenuCursor_RectTransform.anchoredPosition = new Vector2(MenuCursor_RectTransform.anchoredPosition.x + x, MenuCursor_RectTransform.anchoredPosition.y + y);
    }
    */

        /*
    //Sets position of the menu cursor
    private void SetCursorPosition(float x, float y)
    {
        MenuCursor_RectTransform.anchoredPosition = new Vector2(x, y);
    }
    */

    //returns Transform of the nth inventory slot
    /*
    private Transform GetInventorySlot(int n)
    {
        return ItemMenu.transform.GetChild(n);
    }

    */
    //Updates name and uses of item at inventory slot n
    /*
    private void UpdateInventorySlot(int n, string name, int uses, int maxUses)
    {
        GetInventorySlot(n).GetChild(0).GetComponent<TextMeshProUGUI>().text = name;
        if (maxUses == 0)
            GetInventorySlot(n).GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
        else
            GetInventorySlot(n).GetChild(1).GetComponent<TextMeshProUGUI>().text = uses + "/" + maxUses;
    }
    */
    //Updates color of the item's name at inventory slot n
    /*
    private void SetItemNameColor(int n, Color color)
    {
        GetInventorySlot(n).GetChild(0).GetComponent<TextMeshProUGUI>().color = color;
    }
    */

}
