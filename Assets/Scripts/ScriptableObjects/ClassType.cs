using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ClassType : ScriptableObject
{

    public new string name;
    [TextArea]
    public string description;
    public TerrainType[] walkableTerrain; //The terrain this class can walk therough
    public SkillLevels skillLevels;
    public int hpGrowth, strGrowth, magGrowth, defGrowth, resGrowth, sklGrowth, spdGrowth, lckGrowth;
    public int maxHp, maxStr, maxMag, maxDef, maxRes, maxSkl, maxSpd, maxLck;
    public int mov;

    public bool usesPhysicalAttacks, usesBlackMagic, usesWhiteMagic;
}
