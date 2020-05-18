using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    public bool isDead = false;
    
    protected const int maxInventorySize = 5;
    public int level = 1;
    public ClassType classType;
    public SkillLevels skillLevels;    
    public Weapon equippedWeapon;
    public Magic equippedBlackMagic;
    
    public Item[] inventory = new Item[maxInventorySize];
    public List<Magic> magicList = new List<Magic>(); //List of magic skills this unit can potentionally learn

    public List<Magic> blackMagic = new List<Magic>(); //List of black magic skills the unit currently knows

    public int baseHP, baseSTR, baseMAG, baseDEF, baseRES, baseSKL, baseSPD; //The base stats of the unit. ie their stats at level 1
    public int hp, str, mag, def, res, skl, spd; //The units current stats
    public int hpGrowth, strGrowth, magGrowth, defGrowth, resGrowth, sklGrowth, spdGrowth; //Growth rates of the unit.
    public int maxHP;

    public bool usingBlackMagic;
    
    public void Init()
    {
        hp = baseHP; str = baseSTR; mag = baseMAG; def = baseDEF; res = baseRES; skl = baseSKL; spd = baseSPD;
        maxHP = hp;

        //Instanties each item in the unit's inventory. Items are scriptable objects
        for(int i = 0; i < 5; i++)
        {
            if(inventory[i] != null)
                inventory[i] = Instantiate(inventory[i]);
        }

        //Equip first weapon in inventory, if there is one
        if(inventory[0] != null && inventory[0].GetType() == typeof(Weapon))
        {
            ((Weapon)inventory[0]).equipped = true;
            equippedWeapon = ((Weapon)inventory[0]);
        }

        for(int i = 0; i < magicList.Count; i++)
        {
            magicList[i] = Instantiate(magicList[i]);
        }

        for(int i = magicList.Count-1;i >= 0; i--)
        {
            if(magicList[i].magicType == MagicType.BLACK)
            {
                if(magicList[i].minRequirement <= skillLevels.magicLevels[(int)MagicType.BLACK])
                {
                    
                    magicList[i].durability = magicList[i].numberOfUses[skillLevels.magicLevels[(int)MagicType.BLACK]];
                    Debug.Log("Added " + magicList[i].name + ". Durability: " + magicList[i].durability);
                    blackMagic.Add(magicList[i]);
                    magicList.RemoveAt(i);
                    
                }
            }
        }
        if(blackMagic.Count > 0)
        {
            equippedBlackMagic = blackMagic[0];
        }
    }
}
