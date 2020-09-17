using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageReader : MonoBehaviour
{
    public Texture2D image;

    public GameObject grassObject;
    public GameObject mountainObject;
    public GameObject forestObject;
    private int mapLength;
    private int mapHeight;

    private Color grassColor;
    private Color enemyUnitColor;
    private Color playerUnitColor;
    private Color mountainColor;
    private Color forestColor;
    private Color waterColor;

    private void Start()
    {


        mapLength = image.width;
        mapHeight = image.height;

        grassColor = new Color32(144, 212, 97,255);
        enemyUnitColor = new Color32(221, 41, 41,255);
        playerUnitColor = new Color32(90, 40, 217, 255);
        mountainColor = new Color32(174, 125, 46, 255);
        forestColor = new Color32(58, 167, 32, 255);

        for(int row = 0; row < mapHeight; row++)
        {
            GameObject temp = new GameObject();
            temp.name = ("row" + (row + 1).ToString());
            temp.transform.position = new Vector2(0, -row);
        }

        
        for(int row = 0; row < mapHeight; row++)
        {
            for(int col = 0; col < mapLength; col++)
            {
                GameObject temp = null;
                if(image.GetPixel(col,row).Equals(grassColor))
                {
                    temp = Instantiate(grassObject, new Vector3(col-1-mapLength, row-1-mapHeight, 0), Quaternion.identity);          
                }
                else if(image.GetPixel(col, row).Equals(mountainColor))
                {
                    temp = Instantiate(mountainObject, new Vector3(col - 1 - mapLength, row - 1 - mapHeight, 0), Quaternion.identity);
                }
                else if(image.GetPixel(col, row).Equals(forestColor))
                {
                    temp = Instantiate(forestObject, new Vector3(col - 1 - mapLength, row - 1 - mapHeight, 0), Quaternion.identity);
                }
                temp.transform.parent = GameObject.Find("row" + (row + 1).ToString()).transform;
            }
        }
        

        /*
        for(int i = mapHeight; i > 0; i--)
        {
            for(int j = 0; j < mapLength; j++)
            {
                if (pixels[j+((mapHeight-i)*mapHeight)].Equals(mountainColor))
                {
                    Debug.Log("here");
                    Instantiate(mountainObject, new Vector3(j, i, 0), Quaternion.identity);
                }
                else if (pixels[j + ((mapHeight - i) * mapHeight)].Equals(grassColor))
                {
                    Instantiate(grassObject, new Vector3(j,i,0),Quaternion.identity);
                }
            }
        }
        */
    }

}
