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

    public void UpdateStatsText(int[] stats)
    {
        statsText1.text = stats[0] + "\n" + stats[1] + "\n" + stats[2] + "\n" + stats[3];
        statsText2.text = stats[4] + "\n" + stats[5] + "\n" + stats[6] + "\n" + stats[7];
    }

    public void UpdateInventory(Item[] inventory)
    {
        for (int i = 0; i < 5; i++)
        {
            if (inventory[i])
            {
                itemSlots[i].name.text = inventory[i].name;
                if(inventory[i].GetType() == typeof(Weapon))
                {
                    if((inventory[i] as Weapon).equipped)
                        itemSlots[i].name.color = new Color32(34, 170, 160, 255);
                    else
                        itemSlots[i].name.color = Color.black;
                }
                else if(inventory[i].GetType() == typeof(HealingItem))
                {
                    itemSlots[i].uses.text = (inventory[i] as HealingItem).currentUses + "/" + (inventory[i] as HealingItem).maxUses;
                }
                else
                {
                    itemSlots[i].uses.text = "";
                }
            }
            else
            {
                itemSlots[i].name.text = itemSlots[i].uses.text = "";
            }
        }
    }

    public void UpdateMagic(List<Magic> blackMagic, List<Magic> whiteMagic)
    {
        int index = 0;
        foreach(Magic magic in blackMagic)
        {
            magicSlots[index].name.text = magic.name;
            magicSlots[index].uses.text = magic.currentUses + "/" + magic.maxUses;
            index++;
        }
        foreach(Magic magic in whiteMagic)
        {
            magicSlots[index].name.text = magic.name;
            magicSlots[index].uses.text = magic.currentUses + "/" + magic.maxUses;
            index++;
        }
        while(index < 5)
        {
            magicSlots[index].name.text = magicSlots[index].uses.text = "";
            index++;
        }
    }

    public void UpdateBasicInfo(AllyStats stats)
    {
        basicInfo.text = "Level: " + stats.level + "\n" + "Exerpience: " + stats.experience + "\n" + "Class: " + stats.classType.name;
    }
}
