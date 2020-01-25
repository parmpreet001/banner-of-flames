using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//This script contains basic info that will be used by other elements of 
public class MapUIInfo : MonoBehaviour
{
    public GameObject selectedAllyUnit;
    // Update is called once per frame
    private void Update()
    {
        if (selectedAllyUnit && !selectedAllyUnit.GetComponent<AllyMove>().selected)
            selectedAllyUnit = null;
    }
}
