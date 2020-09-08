using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class HealingItem : Item
{
    public int healAmount;
    public int currentUses;
    public int maxUses;
}
