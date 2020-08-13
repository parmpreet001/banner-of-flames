using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    [System.Serializable]
    public struct PlayerUnitData
    {
        public int level;
        public int experience;
        public int baseHP, baseSTR, baseMAG, baseDEF, baseRES, baseSKL, baseSPD;
        public int hp, str, mag, def, res, skl, spd;
        public int hpGrowth, strGrowth, magGrowth, defGrowth, resGrowth, sklGrowth, spdGrowth;
        public int maxHP;
        public SkillLevels skillLevels;

        public string equippedWeapon, classType, equippedBlackMagic, equippedWhiteMagic;
        public List<string> magicList, blackMagic, whiteMagic;

        public AttackMethod attackMethod;       

        public PlayerUnitData(AllyStats stats)
        {
            magicList = new List<string>();
            blackMagic = new List<string>();
            whiteMagic = new List<string>();

            level = stats.level;
            experience = stats.experience;

            baseHP = stats.baseHP;
            baseSTR = stats.baseSTR;
            baseMAG = stats.baseMAG;
            baseDEF = stats.baseDEF;
            baseRES = stats.baseRES;
            baseSKL = stats.baseSKL;
            baseSPD = stats.baseSPD;

            hp = stats.hp;
            str = stats.str;
            mag = stats.mag;
            def = stats.def;
            res = stats.res;
            skl = stats.skl;
            spd = stats.spd;

            hpGrowth = stats.hpGrowth;
            strGrowth = stats.strGrowth;
            magGrowth = stats.magGrowth;
            defGrowth = stats.defGrowth;
            resGrowth = stats.resGrowth;
            sklGrowth = stats.sklGrowth;
            spdGrowth = stats.spdGrowth;

            maxHP = stats.maxHP;

            skillLevels = stats.skillLevels;

            classType = stats.classType.name;
            equippedWeapon = equippedWhiteMagic = equippedBlackMagic = null;
            if (stats.equippedWeapon)
                equippedWeapon = stats.equippedWeapon.name;
            if (stats.equippedWhiteMagic)
                equippedWhiteMagic = stats.equippedWhiteMagic.name;
            if (stats.equippedBlackMagic)
            {
                
                equippedBlackMagic = stats.equippedBlackMagic.name;
            }
                

            foreach(Magic spell in stats.magicList)
            {
                magicList.Add(spell.name);
            }
            foreach(Magic spell in stats.blackMagic)
            {
                blackMagic.Add(spell.name);
            }
            foreach(Magic spell in stats.whiteMagic)
            {
                whiteMagic.Add(spell.name);
            }

            attackMethod = stats.attackMethod;
        }
    }

    public int stage;
    public List<PlayerUnitData> playerUnits = new List<PlayerUnitData>(); 

    public SaveData(int stage, List<GameObject> playerUnits)
    {
        this.stage = stage;
        foreach(GameObject playerUnit in playerUnits)
        {
            this.playerUnits.Add(new PlayerUnitData(playerUnit.GetComponent<AllyStats>()));
        }
        Debug.Log("PlayerUnitData size: " + playerUnits.Count);
    }

    public SaveData()
    {

    }

}
