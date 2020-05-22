using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MapActionMenu : MonoBehaviour
{
    //References to other shit
    private MapUIInfo MapUIInfo;
    private GameObject ItemMenu;
    private Transform WeaponInfo;
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
        WeaponInfo = GameObject.Find("WeaponInfo").transform;
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
        
        
        buttonsCreated = true;
        menuCursorRT.anchoredPosition = new Vector2(menuCursorRT.anchoredPosition.x, -35 * (menuCursorPosition - 1)); 
        menuCursor.SetActive(true);

        if (MapUIInfo.selectedAllyUnit.GetComponent<AllyMove>().findingTarget && MapUIInfo.selectedAllyUnitStats.classType.usesPhysicalAttacks)
            buttons.Add("Attack");
        if (MapUIInfo.selectedAllyUnitStats.blackMagic.Count > 0 && MapUIInfo.selectedAllyUnitStats.classType.usesBlackMagic)
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
                else if(unitInventory[i].GetType() == typeof(Weapon) && !MapUIInfo.selectedAllyUnitStats.CanUseWeapon(i))
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
        Weapon tempWeapon = ((Weapon)unitInventory[index]);
        OffensiveMagic tempBlackMagic = null;

        if(MapUIInfo.selectedAllyUnitStats.usingBlackMagic)
        {
            if(index < MapUIInfo.selectedAllyUnitStats.blackMagic.Count)
            {
                tempBlackMagic = ((OffensiveMagic)MapUIInfo.selectedAllyUnitStats.blackMagic[index]);
                WeaponInfo.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = tempBlackMagic.dmg.ToString();
                WeaponInfo.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = tempBlackMagic.hitRate.ToString();
                WeaponInfo.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "0";
                if (tempBlackMagic.minRange == tempBlackMagic.maxRange)
                    WeaponInfo.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = tempBlackMagic.minRange.ToString();
                else
                    WeaponInfo.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = tempBlackMagic.minRange + " - " + tempBlackMagic.maxRange;
            }
            else
                for (int i = 0; i <= 3; i++)
                    WeaponInfo.transform.GetChild(i).GetComponent<TextMeshProUGUI>().text = "-";
            
        }

        else if(tempWeapon != null)
        {
            WeaponInfo.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = tempWeapon.dmg.ToString();
            WeaponInfo.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = tempWeapon.hitRate.ToString();
            WeaponInfo.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = tempWeapon.critRate.ToString();
            if (tempWeapon.minRange == tempWeapon.maxRange)
                WeaponInfo.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = tempWeapon.minRange.ToString();
            else
                WeaponInfo.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = tempWeapon.minRange + " - " + tempWeapon.maxRange;
        }
        else
            for(int i = 0; i <= 3; i++)
                WeaponInfo.transform.GetChild(i).GetComponent<TextMeshProUGUI>().text = "-";
    }

    private void BlackMagic()
    {
        MapUIInfo.selectedAllyUnitStats.usingBlackMagic = true;
        //Attack();
        menuCursorRT.anchoredPosition = ItemMenu.GetComponent<RectTransform>().anchoredPosition;
        menuCursorRT.anchoredPosition = new Vector2(menuCursorRT.anchoredPosition.x + 180, menuCursorRT.anchoredPosition.y + 36);
        selectingItems = true;
        ItemMenu.SetActive(true);
        menuCursorPosition = 1;

        //Updates every item in the inventory to match the currently selected unit's inventory
        for (int i = 0; i < 5; i++)
        {
            //If the unit's inventory slot is not empty, update name, durability, and text color
            if (i < MapUIInfo.selectedAllyUnitStats.blackMagic.Count)
            {
                GetInventorySlot(i).GetComponent<TextMeshProUGUI>().text = MapUIInfo.selectedAllyUnitStats.blackMagic[i].name;
                GetInventorySlot(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = MapUIInfo.selectedAllyUnitStats.blackMagic[i].durability + "/"
                    + MapUIInfo.selectedAllyUnitStats.blackMagic[i].numberOfUses[MapUIInfo.selectedAllyUnitStats.skillLevels.magicLevels[(int)MagicType.BLACK]];
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
        if(unitInventory[menuCursorPosition - 1] != null && MapUIInfo.selectedAllyUnitStats.CanUseWeapon(menuCursorPosition-1)
            && unitInventory[menuCursorPosition-1].GetType() == typeof(Weapon)
            && !((Weapon)unitInventory[menuCursorPosition-1]).equipped)
        {
            EquipWeapon();
        }
        else if(MapUIInfo.selectedAllyUnitStats.usingBlackMagic && (menuCursorPosition-1) < MapUIInfo.selectedAllyUnitStats.blackMagic.Count)
        {
            MapUIInfo.selectedAllyUnitStats.equippedBlackMagic = MapUIInfo.selectedAllyUnitStats.blackMagic[menuCursorPosition - 1];
            Attack();
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

        MapUIInfo.selectedAllyUnitStats.EquipWeapon(menuCursorPosition - 1);

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
