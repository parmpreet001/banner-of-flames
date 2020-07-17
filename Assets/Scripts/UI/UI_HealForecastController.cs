using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_HealForecastController : MonoBehaviour
{
    private MapUIInfo mapUIInfo;
    private UI_HealForecastDisplay healForecastDisplay;
    void Start()
    {
        mapUIInfo = GetComponentInParent<MapUIInfo>();
        healForecastDisplay = transform.GetChild(0).GetComponent<UI_HealForecastDisplay>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
