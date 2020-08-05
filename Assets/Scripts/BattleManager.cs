using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public GameObject activeUnit; //unit that is doing an action
    public GameObject receivingUnit; //unit that is being acted upon

    private Stats activeUnitStats;
    private Stats receivingUnitStats;

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
        if (CheckWeaponRange(receivingUnitStats, activeUnitStats))
            StartCoroutine(AttackProcess());
    }

    public IEnumerator AttackProcess()
    {
        UpdateStats();
        if (!CheckWeaponRange(receivingUnitStats, activeUnitStats))
            yield return null;

        //Attacking Unit First Attack
        if(GetHitChance(AU_accuracy))
        {
            int dmg = AU_dmg;
            if(GetRandomCrit(AU_crit))
            {
                dmg *= 2;
                battleLog += "Critical Hit! ";
            }

            battleLog += (activeUnit.name + " attacked " + receivingUnit.transform.name + " for " + dmg + " damage.\n");
            receivingUnitStats.hp -= dmg;
            yield return new WaitForSeconds(1f);

            if (activeUnitStats.UsingPhysicalWeapon())
                activeUnitStats.equippedWeapon.currentUses--;
            else if (activeUnitStats.UsingOffensiveMagic())
                activeUnitStats.equippedBlackMagic.currentUses--;

            if (CheckDead(receivingUnitStats))
            {
                yield return new WaitForSeconds(1f);
                EndAttack();
                yield break;
            }
            
        }
        else
        {
            battleLog += (activeUnit.name + " tried to attack, but missed.\n");
            yield return new WaitForSeconds(1f);
        }


        //Defending Unit First Attack
        if (DU_inRange && GetHitChance(DU_accuracy))
        {
            int dmg = DU_dmg;
            if (GetRandomCrit(DU_crit))
            {
                dmg *= 2;
                battleLog += "Critical Hit! ";
            }
            battleLog += (receivingUnit.name + " attacked " + activeUnit.transform.name + " for " + dmg + " damage.\n");
            activeUnitStats.hp -= dmg;
            yield return new WaitForSeconds(1f);

            if (receivingUnitStats.UsingPhysicalWeapon())
                receivingUnitStats.equippedWeapon.currentUses--;
            else if (receivingUnitStats.UsingOffensiveMagic())
                receivingUnitStats.equippedBlackMagic.currentUses--;

            if (CheckDead(activeUnitStats))
            {
                yield return new WaitForSeconds(1f);
                EndAttack();
                yield break;
            }
            
        }
        else if(DU_inRange)
        {
            battleLog += (receivingUnit.name + " tried to attack, but missed.\n");
            yield return new WaitForSeconds(1f);
        }

        //Attacking Unit Second Attack
        if (AU_attackTwice)
        {
            if (GetHitChance(AU_accuracy))
            {
                int dmg = AU_dmg;
                if (GetRandomCrit(AU_crit))
                {
                    dmg *= 2;
                    battleLog += "Critical Hit! ";
                }

                battleLog += (activeUnit.name + " attacked " + receivingUnit.transform.name + " for " + dmg + " damage.\n");
                receivingUnitStats.hp -= dmg;
                yield return new WaitForSeconds(1f);

                if (activeUnitStats.UsingPhysicalWeapon())
                    activeUnitStats.equippedWeapon.currentUses--;
                else if (activeUnitStats.UsingOffensiveMagic())
                    activeUnitStats.equippedBlackMagic.currentUses--;

                if (CheckDead(receivingUnitStats))
                {
                    yield return new WaitForSeconds(1f);
                    EndAttack();
                    yield break;
                }
            }
            else
            {
                battleLog += (activeUnit.name + " tried to attack, but missed.\n");
                yield return new WaitForSeconds(1f);
            }
        }

        //Defending Unit Attack Twice
        if(DU_attackTwice)
        {
            if (DU_inRange && GetHitChance(DU_accuracy))
            {
                int dmg = DU_dmg;
                if (GetRandomCrit(DU_crit))
                {
                    dmg *= 2;
                    battleLog += "Critical Hit! ";
                }
                battleLog += (receivingUnit.name + " attacked " + activeUnit.transform.name + " for " + dmg + " damage.\n");
                activeUnitStats.hp -= dmg;
                yield return new WaitForSeconds(1f);

                if (receivingUnitStats.UsingPhysicalWeapon())
                    receivingUnitStats.equippedWeapon.currentUses--;
                else if (receivingUnitStats.UsingOffensiveMagic())
                    receivingUnitStats.equippedBlackMagic.currentUses--;

                if (CheckDead(activeUnitStats))
                {
                    yield return new WaitForSeconds(1f);
                    EndAttack();
                    yield break;
                }
            }
            else if(DU_inRange)
            {
                battleLog += (receivingUnit.name + " tried to attack, but missed.\n");
                yield return new WaitForSeconds(1f);
            }
        }

        Debug.Log("reached end of attack");
        yield return new WaitForSeconds(1f);

        EndAttack();

        yield return null;
    }

    public IEnumerator HealProcess()
    {
        int healAmount = GetHeal(activeUnitStats); 
        if(healAmount + receivingUnitStats.hp > receivingUnitStats.maxHP)
        {
            battleLog += ("Healed " + receivingUnit.name + " for " + (receivingUnitStats.maxHP - receivingUnitStats.hp) + " HP.");
            receivingUnitStats.hp = receivingUnitStats.maxHP;
        }
        else
        {
            battleLog += ("Healed " + receivingUnit.name + " for " + healAmount + " HP.");
            receivingUnitStats.hp += healAmount;
        }

        yield return new WaitForSeconds(1.5f);
        EndHeal();
        yield return null;
    }

    public void UpdateStats()
    {
        activeUnitStats = activeUnit.GetComponent<Stats>();
        receivingUnitStats = receivingUnit.GetComponent<Stats>();

        AU_attackTwice = false;
        DU_attackTwice = false;

        AU_dmg = GetDmg(activeUnitStats, receivingUnitStats);
        AU_accuracy = GetAccuracy(activeUnitStats, receivingUnitStats);
        AU_crit = GetCrit(activeUnitStats);


        DU_dmg = GetDmg(receivingUnitStats, activeUnitStats);
        DU_accuracy = GetAccuracy(receivingUnitStats, activeUnitStats);
        DU_crit = GetCrit(receivingUnitStats);

        if (activeUnitStats.spd >= receivingUnitStats.spd + 5)
            AU_attackTwice = true;
        else if (receivingUnitStats.spd >= activeUnitStats.spd + 5)
            DU_attackTwice = true;

        DU_inRange = CheckWeaponRange(activeUnitStats,receivingUnitStats);
    }

    //returns damage unit does to target
    private int GetDmg(Stats unit, Stats target)
    {
        Tile unitTile = unit.GetComponentInParent<Tile>();
        Tile targetTile = target.GetComponentInParent<Tile>();
        int dmg = 0;
        //if unit is attacking with physical weapon
        if (unit.UsingPhysicalWeapon())
        {
            dmg += unit.str + unitTile.terrainEffect.strBoost + unit.equippedWeapon.dmg - target.def - targetTile.terrainEffect.defBoost;
            //if target is also attacking with physical weapon 
            if(target.UsingPhysicalWeapon())
            {
                switch (unit.equippedWeapon.weaponType)
                {
                    case WeaponType.SWORD:
                    {
                        if (target.equippedWeapon.weaponType == WeaponType.AXE)
                            dmg = (int)(dmg * 1.35);
                        else if (target.equippedWeapon.weaponType == WeaponType.LANCE)
                            dmg = (int)(dmg * 0.65);
                        break;
                    }
                    case WeaponType.AXE:
                    {
                        if (target.equippedWeapon.weaponType == WeaponType.LANCE)
                            dmg = (int)(dmg * 1.35);
                        else if (target.equippedWeapon.weaponType == WeaponType.SWORD)
                            dmg = (int)(dmg * 0.65);
                        break;
                    }
                    case WeaponType.LANCE:
                    {
                        if (target.equippedWeapon.weaponType == WeaponType.SWORD)
                            dmg = (int)(dmg * 1.35);
                        else if (target.equippedWeapon.weaponType == WeaponType.AXE)
                            dmg = (int)(dmg * 0.65);
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
        else if (unit.UsingOffensiveMagic())
        {
            dmg = unit.mag + ((OffensiveMagic)unit.equippedBlackMagic).dmg + unitTile.terrainEffect.magBoost - target.res - targetTile.terrainEffect.resBoost;
        }

        return dmg;
    }

    //returns accuracy of unit when attacking target
    private int GetAccuracy(Stats unit, Stats target)
    {
        int accuracy = 0;
        Tile unitTile = unit.GetComponentInParent<Tile>();
        Tile targetTile = target.GetComponentInParent<Tile>();

        if (unit.UsingPhysicalWeapon())
            accuracy = unit.equippedWeapon.hitRate + (unit.skl * 2) - target.spd * 2;

        else if (unit.UsingOffensiveMagic())
            accuracy = unit.equippedBlackMagic.hitRate + (unit.skl * 2) - target.spd * 2;

        accuracy += unitTile.terrainEffect.hitBoost - targetTile.terrainEffect.avoidBoost;

        if (accuracy > 100)
            accuracy = 100;
        return accuracy;
    }

    private int GetCrit(Stats unit)
    {
        if (unit.UsingPhysicalWeapon())
            return (unit.skl / 2) + unit.equippedWeapon.critRate;
        else if (unit.UsingOffensiveMagic())
            return 0;

        return 0;
    }

    public int GetHeal(Stats unit)
    {
        return ((HealingMagic)unit.equippedWhiteMagic).heal + (unit.mag / 2);
    }

    //Hit or miss, guess they never miss huh
    private bool GetHitChance(int accuracy)
    {
        int rnd = Random.Range(0, 100);
        return rnd < accuracy;
    }
    
    private bool GetRandomCrit(int critRate)
    {
        int rnd = Random.Range(0, 100);
        return rnd < critRate;
    }

    //returns true if unit1 is within unit2's weapon range. In otherwords, returns true if unit2 can attack
    private bool CheckWeaponRange(Stats unit1, Stats unit2)
    {
        int distance = GetDistanceBetweenUnits(unit1.gameObject, unit2.gameObject);
        //float distance = Vector2.Distance(unit1.transform.position, unit2.transform.position); //Distance between unit 1 and 2
        //distance = Mathf.Ceil(distance);

        if(unit2.UsingPhysicalWeapon())
            return (distance >= unit2.equippedWeapon.minRange && distance <= unit2.equippedWeapon.maxRange);
        else if(unit2.UsingOffensiveMagic())
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

        if (activeUnit.CompareTag("PlayerUnit") && !activeUnitStats.isDead)
            SetExp(activeUnitStats, receivingUnitStats);
        else if (receivingUnit.CompareTag("PlayerUnit") && !receivingUnitStats.isDead)
            SetExp(receivingUnitStats, activeUnitStats);

        if (activeUnit.CompareTag("PlayerUnit") && !activeUnitStats.isDead && activeUnitStats.UsingPhysicalWeapon())
        {
            switch (activeUnitStats.equippedWeapon.weaponType)
            {
                case WeaponType.SWORD:
                    AddWeaponExperience(activeUnitStats, WeaponType.SWORD); break;
                case WeaponType.AXE:
                    AddWeaponExperience(activeUnitStats, WeaponType.AXE); break;
                case WeaponType.LANCE:
                    AddWeaponExperience(activeUnitStats, WeaponType.LANCE); break;
                case WeaponType.BOW:
                    AddWeaponExperience(activeUnitStats, WeaponType.BOW); break;
                default:
                    break;
            }
        }

        else if(activeUnit.CompareTag("PlayerUnit") && !activeUnitStats.isDead && activeUnitStats.UsingOffensiveMagic())
        {
            if (activeUnitStats.equippedBlackMagic.magicType == MagicType.BLACK)
                AddMagicExperience(activeUnitStats, MagicType.BLACK);
            else if (activeUnitStats.equippedBlackMagic.magicType == MagicType.WHITE)
                AddMagicExperience(activeUnitStats, MagicType.WHITE);
        }
            
        if (receivingUnitStats.isDead)
            Destroy(receivingUnit);
        if (activeUnitStats.isDead)
            Destroy(activeUnit);
    }

    private void SetExp(Stats playerUnitStats, Stats enemyUnitStats)
    {
        int levelDifference = playerUnitStats.level - enemyUnitStats.level;
        int[] previousStats = null;
        int expGain = 0;

        switch (levelDifference)
        {
            case 0:
                expGain = 30; break;
            case 1:
                expGain = 25; break;
            case 2:
                expGain = 19; break;
            case 3:
                expGain = 13; break;
            case 4:
                expGain = 7; break;
            case 5:
                expGain = 3; break;
            default:
                {
                    if (levelDifference >= 6)
                        expGain = 1;
                    else
                        expGain = 35;
                }
                break;
        }
        if (!enemyUnitStats.isDead)
        {
            expGain /= 2;
        }

        playerUnitStats.gameObject.GetComponent<AllyStats>().AddExperience(expGain);
    }

    private void EndHeal()
    {
        battleLog = "";
        if(activeUnit.CompareTag("PlayerUnit"))
        {
            activeUnit.GetComponent<AllyStats>().AddExperience(20);
        }

        AddMagicExperience(activeUnitStats, MagicType.WHITE);
        
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

    private void AddMagicExperience(Stats unit, MagicType magicType)
    {
        int magicTypeIndex = (int)magicType; //index of the weapon type the unit used during this attack

        //if the unit hasn't already reached the max skill level possible for their respective class, they gain 10 exp in that skill level
        if (unit.skillLevels.magicLevels[magicTypeIndex] < unit.classType.skillLevels.magicLevels[magicTypeIndex])
            unit.skillLevels.magicLevelsExperience[magicTypeIndex] += 10;

        //If the unit's skill level experience is equal to or more than 100, set experience to 0 and raise the skill level by one
        if (unit.skillLevels.magicLevelsExperience[magicTypeIndex] >= 100)
        {
            unit.skillLevels.magicLevelsExperience[magicTypeIndex] = 0;
            unit.skillLevels.magicLevels[magicTypeIndex]++;
        }
    }

    private int GetDistanceBetweenUnits(GameObject unit1, GameObject unit2)
    {
        int xDistance = (int)Mathf.Abs(unit1.transform.parent.transform.position.x - unit2.transform.parent.transform.position.x);
        int yDistance = (int)Mathf.Abs(unit1.transform.parent.transform.position.y - unit2.transform.parent.transform.position.y);
        return (xDistance + yDistance);
    }
}
