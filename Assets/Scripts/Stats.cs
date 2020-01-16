using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    public int level = 1;

    public int baseHP, baseSTR, baseMAG, baseDEF, baseRES, baseSKL, baseSPD; //The base stats of the unit. ie their stats at level 1
    public int hp, str, mag, def, res, skl, spd; //The units current stats
    public double hpGrowth, strGrowth, magGrowth, defGrowth, resGrowth, sklGrowth, spdGrowth; //Growth rates of the unit.

    public int mov; //Movement speed of the unit
    public int attackRangeMin; //mininum attack range of the unit
    public int attackRangeMax; //maximum attack range of the unit

    //Equipped weapon



    public void Init()
    {
        hp = baseHP; str = baseSTR; mag = baseMAG; def = baseDEF; res = baseRES; skl = baseSKL; spd = baseSPD;
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
    }

    public void Attack(GameObject target)
    {
        int dmg = str - target.GetComponent<Stats>().def;
        if (dmg <= 0)
            dmg = 1;
        Debug.Log(transform.name + " attacked " + target.transform.name + " for " + dmg + "damage");
        target.GetComponent<Stats>().hp -= dmg;

        if(target.GetComponent<Stats>().hp <= 0)
        {
            Debug.Log(target.transform.name + " fucking died.");
            target.gameObject.SetActive(false);
        }
    }
}
