using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public GameObject attackingUnit;
    public GameObject defendingUnit;

    private Stats attackingUnitStats;
    private Stats defendingUnitStats;

    public string battleLog;

    public int AU_dmg;
    public int AU_accuracy;
    public int AU_crit;
    public int DU_dmg;
    public int DU_accuracy;
    public int DU_crit;

    public bool AU_attackTwice;
    public bool DU_attackTwice;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Attack()
    {
        UpdateStats();
        StartCoroutine(AttackProcess());
    }

    IEnumerator AttackProcess()
    {
        UpdateStats();

        attackingUnit.GetComponent<TileMove>().findingTarget = false;
        attackingUnit.GetComponent<TileMove>().attacking = true;

        if(HitOrMiss(AU_accuracy))
        {
            battleLog += (attackingUnit.name + " attacked " + defendingUnit.transform.name + " for " + AU_dmg + " damage.\n");
            yield return new WaitForSeconds(1f);
            defendingUnitStats.hp -= AU_dmg;
            if (checkDead(defendingUnitStats))
            {
                yield return new WaitForSeconds(1f);
                endAttack();
                yield break;
            }
        }
        else
        {
            battleLog += (attackingUnit.name + " tried to attack, but missed.\n");
        }


        if (HitOrMiss(DU_accuracy))
        {
            battleLog += (defendingUnit.name + " attacked " + attackingUnit.transform.name + " for " + DU_dmg + " damage.\n");
            attackingUnitStats.hp -= DU_dmg;
            if (checkDead(attackingUnitStats))
            {
                yield return new WaitForSeconds(1f);
                endAttack();
                yield break;
            }
        }

        yield return new WaitForSeconds(1f);

        //TODO: Enemy attack
        Debug.Log("checking second attack");
        if (AU_attackTwice)
        {
            if (HitOrMiss(AU_accuracy))
            {
                battleLog += (attackingUnit.name + " attacked " + defendingUnit.transform.name + " for " + AU_dmg + " damage.\n");
                defendingUnitStats.hp -= AU_dmg;
                
                if (checkDead(defendingUnitStats))
                {
                    yield return new WaitForSeconds(1f);
                    endAttack();
                    yield break;
                }
            }
            else
            {
                battleLog += (attackingUnit.name + " tried to attack, but missed.\n");
            }
            yield return new WaitForSeconds(1f);
        }

        Debug.Log("reached end of attack");
        yield return new WaitForSeconds(1f);

        endAttack();

        yield return null;
    }

    public void UpdateStats()
    {
        attackingUnitStats = attackingUnit.GetComponent<Stats>();
        defendingUnitStats = defendingUnit.GetComponent<Stats>();

        AU_attackTwice = false;
        DU_attackTwice = false;

        AU_dmg = GetDmg(attackingUnitStats, defendingUnitStats);
        AU_accuracy = GetAccuracy(attackingUnitStats, defendingUnitStats);
        AU_crit = GetCrit(attackingUnitStats);

        DU_dmg = GetDmg(defendingUnitStats, attackingUnitStats);
        DU_accuracy = GetAccuracy(defendingUnitStats, attackingUnitStats);
        DU_crit = GetCrit(defendingUnitStats);

        if (attackingUnitStats.spd >= defendingUnitStats.spd + 5)
            AU_attackTwice = true;
        else if (defendingUnitStats.spd >= attackingUnitStats.spd + 5)
            DU_attackTwice = true;
    }

    //returns damage unit does to target
    private int GetDmg(Stats unit, Stats target)
    {
        int dmg = unit.str + unit.equippedWeapon.dmg - target.def;
        if (dmg <= 0)
            dmg = 1;
        return dmg;
    }

    //returns accuracy of unit when attacking target
    private int GetAccuracy(Stats unit, Stats target)
    {
        //Accuracy = UnitHitRate - TargetAvoidRate
        int accuracy = unit.equippedWeapon.accuracy + (unit.skl * 2) - target.spd * 2;
        if (accuracy > 100)
            accuracy = 100;
        return accuracy;
    }

    private int GetCrit(Stats unit)
    {
        return unit.skl / 2;
    }

    //Hit or miss, guess they never miss huh
    private bool HitOrMiss(int accuracy)
    {
        float rnd = Random.Range(0, 100);
        return rnd <= accuracy;
    }

    //checks if a unit fucking died
    private bool checkDead(Stats unit)
    {
        if(unit.hp <= 0)
        {
            //Destroy(unit.gameObject);
            unit.isDead = true;
            battleLog += (unit.name + " is kill.");
            return true;
        }
        return false;
    }

    private void endAttack()
    {
        battleLog = "";

        attackingUnit.GetComponent<TileMove>().attacking = false;
        attackingUnit.GetComponent<TileMove>().finished = true;
        attackingUnit.GetComponent<TileMove>().RemoveSelectableTiles();

        //if (attackingUnitStats.isDead)
        //    Destroy(attackingUnit);
        if (defendingUnitStats.isDead)
            Destroy(defendingUnit);
    }
}
