using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_BattleLog : MonoBehaviour
{
    public GameObject BattleManager;
    public GameObject BattleLogText;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        BattleLogText.GetComponent<TMP_Text>().text = BattleManager.GetComponent<BattleManager>().battleLog;
    }
}
