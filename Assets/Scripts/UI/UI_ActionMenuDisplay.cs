using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_ActionMenuDisplay : MonoBehaviour
{
    [System.Serializable]
    public struct ItemSlot
    {
        public GameObject gameobject;
        public TextMeshProUGUI name;
        public TextMeshProUGUI uses;

        public ItemSlot(GameObject gameObject)
        {
            this.gameobject = gameObject;
            name = gameObject.transform.Find("Name").GetComponent<TextMeshProUGUI>();
            uses = gameObject.transform.Find("Usage").GetComponent<TextMeshProUGUI>();
        }
    }

    public struct ItemInfo
    {
        public GameObject gameObject;
        public TextMeshProUGUI damage, hitRate, critRate, range;
        public TextMeshProUGUI staticTextDmgHeal;

        public ItemInfo(GameObject gameObject)
        {
            this.gameObject = gameObject;
            damage = gameObject.transform.Find("Damage").GetComponent<TextMeshProUGUI>();
            hitRate = gameObject.transform.Find("HitRate").GetComponent<TextMeshProUGUI>();
            critRate = gameObject.transform.Find("CritRate").GetComponent<TextMeshProUGUI>();
            range = gameObject.transform.Find("Range").GetComponent<TextMeshProUGUI>();
            staticTextDmgHeal = gameObject.transform.Find("StaticText").transform.Find("Damage").GetComponent<TextMeshProUGUI>();
        }
    }

    public GameObject[] buttons = new GameObject[5];
    public ItemSlot[] itemSlots = new ItemSlot[5];
    public ItemInfo itemInfo;
    public GameObject itemMenu;
    public GameObject menuCursor;

    private void Start()
    {
        itemMenu = transform.Find("ItemMenu").gameObject;
        menuCursor = transform.Find("ActionMenuCursor").gameObject;
        itemInfo = new ItemInfo(GameObject.Find("ItemInfo"));

        buttons[0] = (transform.Find("WaitButton").gameObject);
        buttons[1] = (transform.Find("AttackButton").gameObject);
        buttons[2] = (transform.Find("ItemButton").gameObject);
        buttons[3] = (transform.Find("BlackMagicButton").gameObject);
        buttons[4] = (transform.Find("WhiteMagicButton").gameObject);
        buttons[5] = transform.Find("EndTurnButton").gameObject;

        for(int i = 0; i < 5; i++)
        {
            itemSlots[i] = new ItemSlot(itemMenu.transform.GetChild(i).gameObject);
        }
    }

    public void UpdateItemSlot(int index, string name, string uses)
    {
        itemSlots[index].name.text = name;
        itemSlots[index].uses.text = uses;
    }

    public void UpdateItemColor(int index, Color32 color)
    {
        itemSlots[index].name.color = color;
    }

    private GameObject GetButton(string name)
    {
        switch (name)
        {
            case "Wait":
                return buttons[0]; 
            case "Attack":
                return buttons[1];
            case "Item":
                return buttons[2];
            case "BlackMagic":
                return buttons[3];
            case "WhiteMagic":
                return buttons[4];
            case "EndTurn":
                return buttons[5];
            default:
                return null;
        }
    }

    public void SetButtonActive(string name, bool active)
    {
        GetButton(name).SetActive(active);
    }

    public void SetButtonPosition(string name, float x, float y)
    {
        GetButton(name).GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);
    }

    public void SetCursorPosition(float x, float y)
    {
        menuCursor.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);
    }

    public void MoveCursorPosition(float x, float y)
    {
        menuCursor.GetComponent<RectTransform>().anchoredPosition = new Vector2(menuCursor.GetComponent<RectTransform>().anchoredPosition.x + x,
            menuCursor.GetComponent<RectTransform>().anchoredPosition.y + y);
    }

    public void UpdateItemInfo(string damage, string hitRate, string critRate, string range)
    {
        itemInfo.damage.text = damage;
        itemInfo.hitRate.text = hitRate;
        itemInfo.critRate.text = critRate;
        itemInfo.range.text = range;
    }
}
