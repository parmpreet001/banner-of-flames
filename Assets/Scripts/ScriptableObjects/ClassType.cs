using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ClassType : ScriptableObject
{
    [System.Serializable]
    public class WeaponLevel
    {
        public WeaponType weaponType;   //The weapon this class can use
        public TerrainType[] walkableTerrain; //The terrain this class can walk therough
        public int maxLevel;              //The max weapon level a unit with this class can reach
        /* Level    Letter Rank
         * 1        E
         * 2        D
         * 3        C
         * 4        B
         * 5        A
         * 6        S
         */
    }

    public new string name;
    [TextArea]
    public string description;
    public int mov;
    public WeaponLevel[] weaponLevels;
    public int hpGrowth, strGrowth, magGrowth, defGrowth, resGrowth, sklGrowth, spdGrowth;
    public int maxHp, maxStr, maxMag, maxDef, maxRes, maxSkl, maxSpd;
    



}
