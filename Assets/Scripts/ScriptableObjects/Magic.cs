using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Magic : ScriptableObject
{
    public new string name;
    public MagicAttribute magicAttribute;
    public int minRequirement; //Mininum skill level needed to use this magic. Ranges from E to S, represented as an integer from 1 to 6
    public int[] numberOfUses = new int[6]; //Number of times this magic can be used, depending on the skill level of the user

    public int dmg;
    public int accuracy;
    public int minRange;
    public int maxRange;
}
