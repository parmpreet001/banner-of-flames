using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public GameObject attackingUnit;
    public GameObject defendingUnit;

    private Stats attackingUnitStats;
    private Stats defendingUnitStats;
    // Start is called before the first frame update
    void Start()
    {
        attackingUnitStats = attackingUnit.GetComponent<Stats>();
        defendingUnitStats = defendingUnit.GetComponent<Stats>();
    }

    // Update is called once per frame
    void Update()
    {
        
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
            Debug.Log(attackingUnit.name + " attacked " + defendingUnit.transform.name + " for " + AU_dmg + "damage");
            defendingUnitStats.hp -= AU_dmg;
        }
        else
        {
            Debug.Log(transform.name + " missed!");
        }

        yield return new WaitForSeconds(1f);

        //TODO: Enemy attack

        if (AU_attackTwice)
        {
            if (HitOrMiss(AU_accuracy))
            {
                Debug.Log(attackingUnit.name + " attacked " + defendingUnit.transform.name + " for " + AU_dmg + "damage");
                defendingUnitStats.hp -= AU_dmg;
            }
            else
            {
                Debug.Log(transform.name + " missed!");
            }
            yield return new WaitForSeconds(1f);
        }

        //TODO: Enemy counter attack
        Debug.Log("reached end of attack");

        attackingUnit.GetComponent<TileMove>().attacking = false;
        attackingUnit.GetComponent<TileMove>().finished = true;
        attackingUnit.GetComponent<TileMove>().RemoveSelectableTiles();

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
}
