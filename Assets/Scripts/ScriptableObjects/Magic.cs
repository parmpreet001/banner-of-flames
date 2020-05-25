using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Magic : ScriptableObject
{
    public new string name;
    public int minRequirement; //Mininum skill level needed to use this magic. Ranges from E to S, represented as an integer from 1 to 6
    public int[] maxUses = new int[6]; //Number of times this magic can be used, depending on the skill level of the user
    public int currentUses; //Number of uses left per battle
    public TargetType targetType; //Whether the magic targets allies or enemies
    public MagicType magicType;

    public int minRange;
    public int maxRange;
    public int rangeScale; //How much the max range scales with the magic stat. 0 means there is no scaling. If set to 2, max range increases by one for every 2 points in magic
    public int hitRate;

    public bool equipped;
}
