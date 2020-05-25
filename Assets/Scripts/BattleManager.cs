﻿using System.Collections;
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

    public bool DU_inRange;

    public void Attack()
    {
        UpdateStats();
        if (CheckWeaponRange(defendingUnitStats, attackingUnitStats))
            StartCoroutine(AttackProcess());
    }

    IEnumerator AttackProcess()
    {
        UpdateStats();

        attackingUnit.GetComponent<TileMove>().findingTarget = false;
        attackingUnit.GetComponent<TileMove>().attacking = true;

        //Attacking Unit First Attack
        if(HitOrMiss(AU_accuracy))
        {
            int dmg = AU_dmg;
            if(CritChance(AU_crit))
            {
                dmg *= 2;
                battleLog += "Critical Hit! ";
            }

            battleLog += (attackingUnit.name + " attacked " + defendingUnit.transform.name + " for " + dmg + " damage.\n");
            defendingUnitStats.hp -= dmg;
            yield return new WaitForSeconds(1f);

            if(attackingUnitStats.equippedWeapon)
                attackingUnit.GetComponent<Stats>().equippedWeapon.currentUse--;

            if (CheckDead(defendingUnitStats))
            {
                yield return new WaitForSeconds(1f);
                EndAttack();
                yield break;
            }
            
        }
        else
        {
            battleLog += (attackingUnit.name + " tried to attack, but missed.\n");
            yield return new WaitForSeconds(1f);
        }


        //Defending Unit First Attack
        if (DU_inRange && HitOrMiss(DU_accuracy))
        {
            int dmg = DU_dmg;
            if (CritChance(DU_crit))
            {
                dmg *= 2;
                battleLog += "Critical Hit! ";
            }
            battleLog += (defendingUnit.name + " attacked " + attackingUnit.transform.name + " for " + dmg + " damage.\n");
            attackingUnitStats.hp -= dmg;
            yield return new WaitForSeconds(1f);

            //defendingUnit.GetComponent<Stats>().equippedWeapon.currentUse--;

            if (CheckDead(attackingUnitStats))
            {
                yield return new WaitForSeconds(1f);
                EndAttack();
                yield break;
            }
            
        }
        else if(DU_inRange)
        {
            battleLog += (defendingUnit.name + " tried to attack, but missed.\n");
            yield return new WaitForSeconds(1f);
        }

        //Attacking Unit Second Attack
        if (AU_attackTwice)
        {
            if (HitOrMiss(AU_accuracy))
            {
                int dmg = AU_dmg;
                if (CritChance(AU_crit))
                {
                    dmg *= 2;
                    battleLog += "Critical Hit! ";
                }

                battleLog += (attackingUnit.name + " attacked " + defendingUnit.transform.name + " for " + dmg + " damage.\n");
                defendingUnitStats.hp -= dmg;
                yield return new WaitForSeconds(1f);


                //TODO Remove durability
                //attackingUnit.GetComponent<Stats>().equippedWeapon.currentUse--;

                if (CheckDead(defendingUnitStats))
                {
                    yield return new WaitForSeconds(1f);
                    EndAttack();
                    yield break;
                }
            }
            else
            {
                battleLog += (attackingUnit.name + " tried to attack, but missed.\n");
                yield return new WaitForSeconds(1f);
            }
        }

        //Defending Unit Attack Twice
        if(DU_attackTwice)
        {
            if (DU_inRange && HitOrMiss(DU_accuracy))
            {
                int dmg = DU_dmg;
                if (CritChance(DU_crit))
                {
                    dmg *= 2;
                    battleLog += "Critical Hit! ";
                }
                battleLog += (defendingUnit.name + " attacked " + attackingUnit.transform.name + " for " + dmg + " damage.\n");
                attackingUnitStats.hp -= dmg;
                yield return new WaitForSeconds(1f);

                defendingUnit.GetComponent<Stats>().equippedWeapon.currentUse--;

                if (CheckDead(attackingUnitStats))
                {
                    yield return new WaitForSeconds(1f);
                    EndAttack();
                    yield break;
                }
            }
            else if(DU_inRange)
            {
                battleLog += (defendingUnit.name + " tried to attack, but missed.\n");
                yield return new WaitForSeconds(1f);
            }
        }

        Debug.Log("reached end of attack");
        yield return new WaitForSeconds(1f);

        EndAttack();

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

        DU_inRange = CheckWeaponRange(attackingUnitStats,defendingUnitStats);
    }

    //returns damage unit does to target
    private int GetDmg(Stats unit, Stats target)
    {
        int dmg = 0;
        //if unit is attacking with physical weapon
        if(unit.equippedWeapon)
        {
            dmg = unit.str + unit.equippedWeapon.dmg - target.def;
            //if target is also attacking with physical weapon 
            if(target.equippedWeapon)
            {
                switch (unit.equippedWeapon.weaponType)
                {
                    case WeaponType.SWORD:
                    {
                        if (target.equippedWeapon.weaponType == WeaponType.AXE)
                            dmg = (int)(dmg * 1.35);
                        break;
                    }
                    case WeaponType.AXE:
                    {
                        if (target.equippedWeapon.weaponType == WeaponType.LANCE)
                            dmg = (int)(dmg * 1.35);
                        break;
                    }
                    case WeaponType.LANCE:
                    {
                        if (target.equippedWeapon.weaponType == WeaponType.SWORD)
                            dmg = (int)(dmg * 1.35);
                        break;
                    }
                    default:
                        break;
                }
                if (dmg <= 0)
                    dmg = 1;
            }
        }

        //else if unit is attacking with black magic
        else if (unit.equippedBlackMagic && unit.equippedBlackMagic.GetType() == typeof(OffensiveMagic))
        {
            dmg = unit.mag + ((OffensiveMagic)unit.equippedBlackMagic).dmg - target.res;
        }

        return dmg;
    }

    //returns accuracy of unit when attacking target
    private int GetAccuracy(Stats unit, Stats target)
    {
        int accuracy = 0;

        if(unit.equippedWeapon)
            accuracy = unit.equippedWeapon.hitRate + (unit.skl * 2) - target.spd * 2;

        else if (unit.equippedBlackMagic)
            accuracy = unit.equippedBlackMagic.hitRate + (unit.skl * 2) - target.spd * 2;        

        if (accuracy > 100)
            accuracy = 100;
        return accuracy;
    }

    private int GetCrit(Stats unit)
    {
        if (unit.equippedWeapon)
            return unit.skl / 2;
        else if (unit.equippedBlackMagic)
            return 0;

        return 0;
    }

    //Hit or miss, guess they never miss huh
    private bool HitOrMiss(int accuracy)
    {
        float rnd = Random.Range(0, 100);
        return rnd <= accuracy;
    }
    
    private bool CritChance(int critRate)
    {
        float rnd = Random.Range(0, 100);
        return rnd <= critRate;
    }

    //returns true if unit1 is within unit2's weapon range. In otherwords, returns true if unit2 can attack
    private bool CheckWeaponRange(Stats unit1, Stats unit2)
    {
        int distance = unit1.GetComponent<TileMove>().GetDistanceBetweenTiles(unit1.transform.parent.gameObject, unit2.transform.parent.gameObject);
        //float distance = Vector2.Distance(unit1.transform.position, unit2.transform.position); //Distance between unit 1 and 2
        //distance = Mathf.Ceil(distance);

        if(unit2.equippedWeapon)
            return (distance >= unit2.equippedWeapon.minRange && distance <= unit2.equippedWeapon.maxRange);
        else if(unit2.equippedBlackMagic)
            return (distance >= unit2.equippedBlackMagic.minRange && distance <= unit2.equippedBlackMagic.maxRange);

        return true;
    }

    //checks if a unit fucking died
    private bool CheckDead(Stats unit)
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

    private void EndAttack()
    {
        battleLog = "";

        attackingUnit.GetComponent<TileMove>().attacking = false;
        attackingUnit.GetComponent<TileMove>().finished = true;
        attackingUnit.GetComponent<TileMove>().RemoveSelectableTiles();

        if (attackingUnit.tag == "PlayerUnit" && !attackingUnitStats.isDead && !attackingUnitStats.usingBlackMagic)
        {
            switch (attackingUnitStats.equippedWeapon.weaponType)
            {
                case WeaponType.SWORD:
                    AddWeaponExperience(attackingUnitStats, WeaponType.SWORD); break;
                case WeaponType.AXE:
                    AddWeaponExperience(attackingUnitStats, WeaponType.AXE); break;
                case WeaponType.LANCE:
                    AddWeaponExperience(attackingUnitStats, WeaponType.LANCE); break;
                case WeaponType.BOW:
                    AddWeaponExperience(attackingUnitStats, WeaponType.BOW); break;
                default:
                    break;
            }
        }
            
        if (defendingUnitStats.isDead)
            Destroy(defendingUnit);
    }
    private void AddWeaponExperience(Stats unit, WeaponType weaponType)
    {
        int weaponTypeIndex = (int)weaponType; //index of the weapon type the unit used during this attack

        //if the unit hasn't already reached the max skill level possible for their respective class, they gain 10 exp in that skill level
        if(unit.skillLevels.weaponLevels[weaponTypeIndex] < unit.classType.skillLevels.weaponLevels[weaponTypeIndex])
            unit.skillLevels.weaponLevelsExperience[weaponTypeIndex] += 10;

        //If the unit's skill level experience is equal to or more than 100, set experience to 0 and raise the skill level by one
        if(unit.skillLevels.weaponLevelsExperience[weaponTypeIndex] >= 100)
        {
            unit.skillLevels.weaponLevelsExperience[weaponTypeIndex] = 0;
            unit.skillLevels.weaponLevels[weaponTypeIndex]++;
        }
    }
}
