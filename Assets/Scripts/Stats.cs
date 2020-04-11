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

    public int swordExperience, axeExperience, lanceExperience, bowExperience;

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
    }

    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Space))
        {
            LevelUp(1);
        }
    }

    public void LevelUp(int levels)
    {
        level++;
        double[] rand = new double[7];
        for (int i = 0; i < 7; i++)
            rand[i] = Random.value * 100;
        
        while(levels > 0)
        {
            if (rand[0] <= hpGrowth + classType.hpGrowth)
                hp++;
            if (rand[1] <= strGrowth + classType.strGrowth)
                str++;
            if (rand[2] <= magGrowth + classType.magGrowth)
                mag++;
            if (rand[3] <= defGrowth + classType.defGrowth)
                def++;
            if (rand[4] <= resGrowth + classType.resGrowth)
                res++;
            if (rand[5] <= sklGrowth + classType.sklGrowth)
                skl++;
            if (rand[6] <= spdGrowth + classType.spdGrowth)
                spd++;
            levels--;
        }
        maxHP = hp;
    }   
}
