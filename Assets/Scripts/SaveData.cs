using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData : MonoBehaviour
{
    [System.Serializable]
    public struct PlayerUnitData
    {
        public int level;
        public string equippedWeapon;
        public PlayerUnitData(AllyStats stats)
        {
            level = stats.level;
            if (stats.equippedWeapon)
                equippedWeapon = stats.equippedWeapon.name;
            else
                equippedWeapon = null;
        }
    }

    public int stage;
    public List<PlayerUnitData> playerUnits = new List<PlayerUnitData>(); 

    public SaveData(int stage, List<GameObject> playerUnits)
    {
        this.stage = stage;
        foreach(GameObject playerUnit in playerUnits)
        {
            this.playerUnits.Add(new PlayerUnitData(playerUnit.GetComponent<AllyStats>()));
        }
        Debug.Log("PlayerUnitData size: " + playerUnits.Count);
    }

    public SaveData()
    {

    }

}
