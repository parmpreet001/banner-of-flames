using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    private GameObject attackingUnit;
    private GameObject defendingUnit;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator AttackProcess()
    {
        bool AU_attackTwice = false;
        bool DU_attackTwice = false;

        //int AU_dmg = attackingUnit.GetComponent<Stats>().GetDmg()

        yield return null;
    }
}
