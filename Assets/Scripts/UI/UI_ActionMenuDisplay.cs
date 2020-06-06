using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_ActionMenuDisplay : MonoBehaviour
{
    private GameObject waitButton;
    private GameObject attackButton;
    private GameObject itemButton;
    private GameObject blackMagicButton;

    private GameObject actionMenuCursor;
    private GameObject itemMenu;
    private GameObject weaponInfo;

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

        actionMenuCursor = transform.Find("ActionMenuCursor").gameObject;
        itemMenu = transform.Find("ItemMenu").gameObject;
        weaponInfo = itemMenu.transform.Find("WeaponInfo").gameObject;

        for(int i = 0; i < 5; i++)
            itemSlots[i] = new ItemSlot(itemMenu.transform.Find("Item" + (i + 1)).gameObject);
    }
}
