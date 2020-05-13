using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    [System.Serializable]
    public class WeaponLevel
    {
        public WeaponType weaponType;
        public int weaponExperience;

        public WeaponLevel(WeaponType weaponType, int weaponExperience)
        {
            this.weaponType = weaponType;
            this.weaponExperience = weaponExperience;
        }
    }

    private const int maxInventorySize = 5;
    
    public List<WeaponLevel> weaponLevel = new List<WeaponLevel>();

    public int level = 1;
    public ClassType classType;
    public bool isDead = false;

    public int baseHP, baseSTR, baseMAG, baseDEF, baseRES, baseSKL, baseSPD; //The base stats of the unit. ie their stats at level 1
    public int hp, str, mag, def, res, skl, spd; //The units current stats
    public int hpGrowth, strGrowth, magGrowth, defGrowth, resGrowth, sklGrowth, spdGrowth; //Growth rates of the unit.
    public int maxHP;

    //public int attackRangeMin; //mininum attack range of the unit
    //public int attackRangeMax; //maximum attack range of the unit

    //Equipped weapon
    public Weapon equippedWeapon;

    public Item[] inventory = new Item[maxInventorySize];

    public void Init()
    {
        hp = baseHP; str = baseSTR; mag = baseMAG; def = baseDEF; res = baseRES; skl = baseSKL; spd = baseSPD;
        maxHP = hp;

        weaponLevel.Add(new WeaponLevel(WeaponType.SWORD, 0));
        weaponLevel.Add(new WeaponLevel(WeaponType.AXE, 0));
        weaponLevel.Add(new WeaponLevel(WeaponType.LANCE, 0));
        weaponLevel.Add(new WeaponLevel(WeaponType.BOW, 0));

        for(int i = 0; i < 5; i++)
        {
            if(inventory[i] != null)
                inventory[i] = Instantiate(inventory[i]);
        }
        ((Weapon)inventory[0]).equipped = true;
        equippedWeapon = ((Weapon)inventory[0]);
    }
}
