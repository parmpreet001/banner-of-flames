using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class TerrainEffect : ScriptableObject
{
    [TextArea]
    public string description;
    public int strBoost, magBoost, defBoost, resBoost, hitBoost, avoidBoost;
}
