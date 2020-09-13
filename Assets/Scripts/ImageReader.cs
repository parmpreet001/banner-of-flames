using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageReader : MonoBehaviour
{
    public Texture2D image;

    public GameObject grassObject;
    private int mapLength;
    private int mapHeight;

    private Color grass;
    private Color enemyUnit;

    int grassCount;

    private void Start()
    {
        mapLength = image.width;
        mapHeight = image.height;

        Debug.Log(mapLength + "," + mapHeight);

        grass = new Color32(144, 212, 97,255);
        enemyUnit = new Color32(221, 41, 41,255);

        Color[] pixels = image.GetPixels();
        foreach(Color pixel in pixels)
        {
            if(pixel.Equals(grass))
            {
                grassCount++;
            }
        }

        int pixelIndex = 0;
        for(int i = mapHeight; i > 0; i--)
        {
            if(pixels[pixelIndex].Equals(grass))
            {
                Instantiate(grassObject, new Vector3(pixelIndex,mapHeight,0),Quaternion.identity);
            }
            pixelIndex++;
        }

        Debug.Log(grassCount);
    }

}
