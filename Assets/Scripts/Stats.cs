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
    public Weapon equippedWeapon;


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
        StartCoroutine(AttackProcess(target));
    }

    IEnumerator AttackProcess(GameObject target)
    {
        bool hit = false;
        bool attackTwice = false;
        bool enemyAttackTwice = false;
        int dmg = str + equippedWeapon.dmg - target.GetComponent<Stats>().def;
        int hitrate = equippedWeapon.accuracy + (skl * 2);
        int avoid = spd * 2;
        int targetHitrate = target.GetComponent<Stats>().equippedWeapon.accuracy + (skl * 2);
        int targetAvoid = target.GetComponent<Stats>().spd * 2;
        int accuracy = (hitrate - targetAvoid);
        int targetAccuracy = (targetHitrate - avoid);

        Debug.Log("Accuracy is " + accuracy);

        if (dmg <= 0)
            dmg = 1;

        float rnd = Random.Range(0, 100);
        if(rnd <= accuracy)
        {
            hit = true;
        }

        Debug.Log("rnd is " + rnd);


        GetComponent<TileMove>().findingTarget = false;
        GetComponent<TileMove>().attacking = true;

        if(hit)
        {
            Debug.Log(transform.name + " attacked " + target.transform.name + " for " + dmg + "damage");
            target.GetComponent<Stats>().hp -= dmg;
        }
        else
        {
            Debug.Log(transform.name + " missed!");
        }

        yield return new WaitForSeconds(1f);

        if (target.GetComponent<Stats>().hp <= 0)
        {
            Debug.Log(target.transform.name + " fucking died.");
            yield return new WaitForSeconds(1f);
            Destroy(target.gameObject);

            GetComponent<TileMove>().attacking = false;
            GetComponent<TileMove>().RemoveSelectableTiles();
            yield return null;
        }

        Debug.Log("reached end of attack");
        GetComponent<TileMove>().attacking = false;
        GetComponent<TileMove>().RemoveSelectableTiles();
        yield return null;
    }
}
