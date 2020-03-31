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

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Attack()
    {
        attackingUnitStats = attackingUnit.GetComponent<Stats>();
        defendingUnitStats = defendingUnit.GetComponent<Stats>();
        StartCoroutine(AttackProcess());
    }

    IEnumerator AttackProcess()
    {
        bool AU_attackTwice = false;
        bool DU_attackTwice = false;

        int AU_dmg = GetDmg(attackingUnitStats, defendingUnitStats);
        int AU_accuracy = GetAccuracy(attackingUnitStats, defendingUnitStats);

        int DU_dmg = GetDmg(defendingUnitStats, attackingUnitStats);
        int DU_accuracy = GetAccuracy(defendingUnitStats, attackingUnitStats);

        if (attackingUnitStats.spd >= defendingUnitStats.spd + 5)
            AU_attackTwice = true;
        else if (defendingUnitStats.spd >= attackingUnitStats.spd + 5)
            DU_attackTwice = true;

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
        int unitHitRate = unit.equippedWeapon.accuracy + (unit.skl * 2);
        int targetAvoid = target.spd * 2;
        return unitHitRate - targetAvoid;
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
