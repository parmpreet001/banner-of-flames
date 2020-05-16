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
    
    public Item[] inventory = new Item[maxInventorySize];
    public List<Magic> magicList = new List<Magic>(); 

    public int baseHP, baseSTR, baseMAG, baseDEF, baseRES, baseSKL, baseSPD; //The base stats of the unit. ie their stats at level 1
    public int hp, str, mag, def, res, skl, spd; //The units current stats
    public int hpGrowth, strGrowth, magGrowth, defGrowth, resGrowth, sklGrowth, spdGrowth; //Growth rates of the unit.
    public int maxHP;  
    
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
    }
}
