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

    public GameObject[] buttons = new GameObject[5];
    public ItemSlot[] itemSlots = new ItemSlot[5];
    public GameObject itemMenu;

    private void Start()
    {
        itemMenu = GameObject.Find("ItemMenu");

        buttons[0] = (transform.Find("WaitButton").gameObject);
        buttons[1] = (transform.Find("AttackButton").gameObject);
        buttons[2] = (transform.Find("ItemButton").gameObject);
        buttons[3] = (transform.Find("BlackMagicButton").gameObject);
        buttons[4] = (transform.Find("WhiteMagicButton").gameObject);

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
            default:
                return null;
        }
    }

    public void SetButtonActive(string name, bool active)
    {
        GetButton(name).SetActive(active);
    }

    public void SetButtonRectTransform(string name, float x, float y)
    {
        GetButton(name).GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);
    }
}
