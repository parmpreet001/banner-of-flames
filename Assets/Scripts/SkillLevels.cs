using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SkillLevels
{
    public int[] weaponLevels = new int[] {1,1,1,1}; //Sword, Axe, Lance, Bow
    public int[] weaponLevelsExperience = new int[4];
    public int[] magicLevels = new int[] { 1, 1 }; //Black, White
    public int[] magicLevelsExperience = new int[4];
}

