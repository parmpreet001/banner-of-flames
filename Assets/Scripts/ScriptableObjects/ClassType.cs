using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ClassType : ScriptableObject
{
    [System.Serializable]
    public class WeaponLevel
    {
        public WeaponType weaponType;
        public char level;
    }

    public new string name;
    public int mov;
    public WeaponLevel[] weaponLevels;
    public int hpGrowth, strGrowth, magGrowth, defGrowth, resGrowth, sklGrowth, spdGrowth;
    public int maxHp, maxStr, maxMag, maxDef, maxRes, maxSkl, maxSpd;


}
