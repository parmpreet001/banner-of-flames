using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData : MonoBehaviour
{
    public int level;
    public struct PlayerUnitData
    {
        public int level, experience;
        ClassType classType;
        public SkillLevels skillLevels;
        public Weapon equippedWeapon;
        public Magic equippedBlackMagic;
        public Magic equippedWhiteMagic;

        public Item[] inventory;// = new Item[5];
        public List<Magic> magicList;// = new List<Magic>(); //List of magic skills this unit can potentionally learn

        public List<Magic> blackMagic;// = new List<Magic>(); //List of black magic skills the unit currently knows
        public List<Magic> whiteMagic;// = new List<Magic>(); //List of black magic skills the unit currently knows

        public int baseHP, baseSTR, baseMAG, baseDEF, baseRES, baseSKL, baseSPD; //The base stats of the unit. ie their stats at level 1
        public int hp, str, mag, def, res, skl, spd; //The units current stats
        public int hpGrowth, strGrowth, magGrowth, defGrowth, resGrowth, sklGrowth, spdGrowth; //Growth rates of the unit.
        public int maxHP;

        public PlayerUnitData(PlayerUnitData playerUnitData)
        {
            level = playerUnitData.level;
            experience = playerUnitData.experience;
            classType = playerUnitData.classType;
            skillLevels = playerUnitData.skillLevels;
            equippedWeapon = playerUnitData.equippedWeapon;
            equippedBlackMagic = playerUnitData.equippedBlackMagic;
            equippedWhiteMagic = playerUnitData.equippedWhiteMagic;
            inventory = playerUnitData.inventory;
            magicList = playerUnitData.magicList;
            blackMagic = playerUnitData.blackMagic;
            whiteMagic = playerUnitData.whiteMagic;

            baseHP = playerUnitData.baseHP;
            baseSTR = playerUnitData.baseSTR;
            baseMAG = playerUnitData.baseMAG;
            baseDEF = playerUnitData.baseDEF;
            baseRES = playerUnitData.baseRES;
            baseSKL = playerUnitData.baseSKL;
            baseSPD = playerUnitData.baseSPD;

            hp = playerUnitData.hp;
            str = playerUnitData.str;
            mag = playerUnitData.mag;
            def = playerUnitData.def;
            res = playerUnitData.res;
            skl = playerUnitData.skl;
            spd = playerUnitData.spd;

            hpGrowth = playerUnitData.hpGrowth;
            strGrowth = playerUnitData.strGrowth;
            magGrowth = playerUnitData.magGrowth;
            defGrowth = playerUnitData.defGrowth;
            resGrowth = playerUnitData.resGrowth;
            sklGrowth = playerUnitData.sklGrowth;
            spdGrowth = playerUnitData.spdGrowth;

            maxHP = playerUnitData.maxHP;
        }
    }

    public List<PlayerUnitData> playerUnits = new List<PlayerUnitData>();

    public SaveData (SaveData saveData)
    {
        for(int i = 0; i < saveData.playerUnits.Count; i++)
        {
            playerUnits.Add(new PlayerUnitData(saveData.playerUnits[i]));
        }
    }

    public SaveData()
    {

    }
}
