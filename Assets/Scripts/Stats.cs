using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    public int level = 1;
    public bool isDead = false;

    public int baseHP, baseSTR, baseMAG, baseDEF, baseRES, baseSKL, baseSPD; //The base stats of the unit. ie their stats at level 1
    public int hp, str, mag, def, res, skl, spd; //The units current stats
    public double hpGrowth, strGrowth, magGrowth, defGrowth, resGrowth, sklGrowth, spdGrowth; //Growth rates of the unit.

    public int maxHP;

    public int mov; //Movement speed of the unit
    public int attackRangeMin; //mininum attack range of the unit
    public int attackRangeMax; //maximum attack range of the unit

    //Equipped weapon
    public Weapon equippedWeapon;


    public void Init()
    {
        hp = baseHP; str = baseSTR; mag = baseMAG; def = baseDEF; res = baseRES; skl = baseSKL; spd = baseSPD;
        maxHP = hp;
    }

    
    public void LevelUp(int levels)
    {
        level++;
        double[] rand = new double[7];
        for (int i = 0; i < 7; i++)
            rand[i] = Random.value;
        
        while(levels > 0)
        {
            if (rand[0] <= hpGrowth)
                hp++;
            if (rand[1] <= strGrowth)
                str++;
            if (rand[2] <= magGrowth)
                mag++;
            if (rand[3] <= defGrowth)
                def++;
            if (rand[4] <= resGrowth)
                res++;
            if (rand[5] <= sklGrowth)
                skl++;
            if (rand[6] <= spdGrowth)
                spd++;
            levels--;
        }
        maxHP = hp;
    }   
}
