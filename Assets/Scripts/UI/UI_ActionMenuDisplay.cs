using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_ActionMenuDisplay : MonoBehaviour
{
    [System.Serializable]
    public struct ItemSlot
    {
        public GameObject gameObject;
        public TextMeshProUGUI name;
        public TextMeshProUGUI usage;

        public ItemSlot(GameObject gameObject)
        {
            this.gameObject = gameObject;
            name = gameObject.transform.Find("Name").GetComponent<TextMeshProUGUI>();
            usage = gameObject.transform.Find("Usage").GetComponent<TextMeshProUGUI>();
        }
    }

    [System.Serializable]
    public struct WeaponInfo
    {
        public GameObject gameObject;
        public TextMeshProUGUI damage, hitRate, critRate, range;

        public WeaponInfo(GameObject gameObject)
        {
            this.gameObject = gameObject;
            damage = gameObject.transform.Find("Damage").GetComponent<TextMeshProUGUI>();
            hitRate = gameObject.transform.Find("HitRate").GetComponent<TextMeshProUGUI>();
            critRate = gameObject.transform.Find("CritRate").GetComponent<TextMeshProUGUI>();
            range = gameObject.transform.Find("Range").GetComponent<TextMeshProUGUI>();
        }
    }

    public GameObject waitButton;
    public GameObject attackButton;
    public GameObject itemButton;
    public GameObject blackMagicButton;
    public GameObject actionMenuCursor;
    public GameObject itemMenu;
    public ItemSlot[] itemSlots = new ItemSlot[5];
    public WeaponInfo weaponInfo;
    public int actionMenuPosition;

    private void Start()
    {
        waitButton = transform.Find("WaitButton").gameObject;
        attackButton = transform.Find("AttackButton").gameObject;
        itemButton = transform.Find("ItemButton").gameObject;
        blackMagicButton = transform.Find("BlackMagicButton").gameObject;
        actionMenuCursor = transform.Find("ActionMenuCursor").gameObject;
        itemMenu = transform.Find("ItemMenu").gameObject;
        weaponInfo = new WeaponInfo(itemMenu.transform.Find("WeaponInfo").gameObject);

        for(int i = 0; i < 5; i++)
        {
            itemSlots[i] = new ItemSlot(itemMenu.transform.GetChild(i).gameObject);
        }
    }

    public void UpdateWeaponInfo(string damage, string hitRate, string critRate, string range)
    {
        weaponInfo.damage.text = damage;
        weaponInfo.hitRate.text = hitRate;
        weaponInfo.critRate.text = critRate;
        weaponInfo.range.text = range;
    }
}
