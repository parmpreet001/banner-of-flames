using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class UI_ActionMenuDisplay : MonoBehaviour
{
    private GameObject waitButton;
    private GameObject attackButton;
    private GameObject itemButton;
    private GameObject blackMagicButton;

    public GameObject menuCursor;
    public GameObject itemMenu;
    private GameObject weaponInfo;

    public RectTransform menuCursor_RectTransform;

    public int menuCursorPosition = 1;

    private struct ItemSlot
    {   
        public GameObject gameObject;
        public TextMeshProUGUI name;
        public TextMeshProUGUI usage;

        public ItemSlot(GameObject gameObject)
        {
            this.gameObject = gameObject;
            this.name = gameObject.transform.Find("Name").GetComponent<TextMeshProUGUI>();
            this.usage = gameObject.transform.Find("Usage").GetComponent<TextMeshProUGUI>();
        }
    }

    private ItemSlot[] itemSlots = new ItemSlot[5];

    private void Start()
    {
        waitButton = transform.Find("WaitButton").gameObject;
        attackButton = transform.Find("AttackButton").gameObject;
        itemButton = transform.Find("ItemButton").gameObject;
        blackMagicButton = transform.Find("BlackMagicButton").gameObject;

        menuCursor = transform.Find("ActionMenuCursor").gameObject;
        itemMenu = transform.Find("ItemMenu").gameObject;
        weaponInfo = itemMenu.transform.Find("WeaponInfo").gameObject;

        for(int i = 0; i < 5; i++)
            itemSlots[i] = new ItemSlot(itemMenu.transform.Find("Item" + (i + 1)).gameObject);

        menuCursor_RectTransform = menuCursor.GetComponent<RectTransform>();
    }

    public void UpdateWeaponInfoText(string dmg, string hitRate, string critRate, string range)
    {
        weaponInfo.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = dmg;
        weaponInfo.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = hitRate;
        weaponInfo.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = critRate;
        weaponInfo.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = range;
    }

    public void SetCursorPosition(int x, int y)
    {
        menuCursor_RectTransform.anchoredPosition = new Vector2(x, y);
    }

    public void MoveCursor(int x, int y)
    {
        menuCursor_RectTransform.anchoredPosition = new Vector2(menuCursor_RectTransform.anchoredPosition.x + x,
            menuCursor_RectTransform.anchoredPosition.y + y);
    }

    public Transform GetInventorySlot(int n)
    {
        return itemMenu.transform.GetChild(n);
    }

    public void UpdateInventorySlot(int n, string name, int uses, int maxUses)
    {
        GetInventorySlot(n).GetChild(0).GetComponent<TextMeshProUGUI>().text = name;
        if (maxUses == 0)
            GetInventorySlot(n).GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
        else
            GetInventorySlot(n).GetChild(1).GetComponent<TextMeshProUGUI>().text = uses + "/" + maxUses;
    }

    public void SetItemNameColor(int n, Color color)
    {
        GetInventorySlot(n).GetChild(0).GetComponent<TextMeshProUGUI>().color = color;
    }

    public void SetMenuCursorActive(bool active)
    {
        menuCursor.SetActive(active);
    }



    public void SetItemMenuActive(bool active)
    {
        itemMenu.SetActive(active);
    }




}
