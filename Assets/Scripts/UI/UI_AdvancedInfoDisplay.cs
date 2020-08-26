using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_AdvancedInfoDisplay : MonoBehaviour
{
    private TextMeshProUGUI statsText1;
    private TextMeshProUGUI statsText2;
    private TextMeshProUGUI basicInfo;

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

    public ItemSlot[] itemSlots = new ItemSlot[5];
    public ItemSlot[] magicSlots = new ItemSlot[5];

    void Start()
    {
        GameObject statsDisplay = transform.Find("StatsDisplay").gameObject;
        GameObject itemDisplay = transform.Find("ItemDisplay").gameObject;
        GameObject magicDisplay = transform.Find("MagicDisplay").gameObject;

        statsText1 = statsDisplay.transform.Find("StatsText1").GetComponent<TextMeshProUGUI>();
        statsText2 = statsDisplay.transform.Find("StatsText2").GetComponent<TextMeshProUGUI>();
        basicInfo = transform.Find("BasicInfo").GetComponent<TextMeshProUGUI>();

        for (int i = 0; i < 5; i++)
        {
            itemSlots[i] = new ItemSlot(itemDisplay.transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < 5; i++)
        {
            magicSlots[i] = new ItemSlot(magicDisplay.transform.GetChild(i).gameObject);
        }
    }
}
