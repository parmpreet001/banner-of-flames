using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : Stats
{
    public EnemyBehavior enemyBehavior;
    void Start()
    {
        Init();
    }
}
