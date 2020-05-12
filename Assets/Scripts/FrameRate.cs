using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameRate : MonoBehaviour
{
    public int avgFrameRate;

    // Start is called before the first frame update
    void Start()
    {


        //Application.targetFrameRate = 120;
        
    }

    private void Update()
    {
        float current = 0;
        current = (int)(1f / Time.unscaledDeltaTime);
        avgFrameRate = (int)current;
    }
}
