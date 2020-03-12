using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    public int level = 1;

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

    public void Attack(GameObject target)
    {
        StartCoroutine(AttackProcess(target));
    }

    IEnumerator AttackProcess(GameObject target)
    {
        bool attackTwice = false;
        bool enemyAttackTwice = false;

        int dmg = GetDmg(gameObject, target);
        int hitrate = GetHitrate(gameObject);
        int avoid = GetAvoid(gameObject);
        int targetDmg = GetDmg(target, gameObject);
        int targetHitrate = GetHitrate(target);
        int targetAvoid = GetAvoid(target);

        int accuracy = (hitrate - targetAvoid);
        int targetAccuracy = (targetHitrate - avoid);

        if(spd >= target.GetComponent<Stats>().spd + 5)
            attackTwice = true;
        else if(target.GetComponent<Stats>().spd >+ spd + 5)
            enemyAttackTwice = true;

        GetComponent<TileMove>().findingTarget = false;
        GetComponent<TileMove>().attacking = true;

        if(HitOrMiss(accuracy))
        {
            Debug.Log(transform.name + " attacked " + target.transform.name + " for " + dmg + "damage");
            target.GetComponent<Stats>().hp -= dmg;
        }
        else
        {
            Debug.Log(transform.name + " missed!");
        }

        yield return new WaitForSeconds(1f);

        if(attackTwice)
        {
            if (HitOrMiss(accuracy))
            {
                Debug.Log(transform.name + " attacked " + target.transform.name + " for " + dmg + "damage");
                target.GetComponent<Stats>().hp -= dmg;
            }
            else
            {
                Debug.Log(transform.name + " missed!");
            }
            yield return new WaitForSeconds(1f);
        }

        if (target.GetComponent<Stats>().hp <= 0)
        {
            Debug.Log(target.transform.name + " fucking died.");
            yield return new WaitForSeconds(1f);
            Destroy(target.gameObject);
        }

        Debug.Log("reached end of attack");
        
        GetComponent<TileMove>().attacking = false;
        GetComponent<TileMove>().finished = true;
        GetComponent<TileMove>().RemoveSelectableTiles();
        yield return null;
    }

    public int GetDmg(GameObject unit, GameObject target)
    {
        int dmg = unit.GetComponent<Stats>().str + unit.GetComponent<Stats>().equippedWeapon.dmg - target.GetComponent<Stats>().def;
        if (dmg <= 0)
            dmg = 1;
        return dmg;
    }

    public int GetHitrate(GameObject unit)
    {
        return unit.GetComponent<Stats>().equippedWeapon.accuracy + (unit.GetComponent<Stats>().skl * 2);
    }

    public int GetAvoid(GameObject unit)
    {
        return unit.GetComponent<Stats>().spd * 2;
    }

    public bool HitOrMiss(int accuracy)
    {
        float rnd = Random.Range(0, 100);
        return rnd <= accuracy;
    }
}
