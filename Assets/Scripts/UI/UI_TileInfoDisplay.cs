using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_TileInfoDisplay : MonoBehaviour
{
    private TextMeshProUGUI tileName;
    private TextMeshProUGUI tileInfo;

    private void Start()
    {
        tileName = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        tileInfo = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
    }

    public void SetTileName(string tileName)
    {
        this.tileName.text = tileName;
    }

    public void SetTileInfo(string tileInfo)
    {
        this.tileInfo.text = tileInfo;
    }
}
