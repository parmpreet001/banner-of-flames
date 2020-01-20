using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyMove :TileMove
{
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        if (!moving && selected)
        {
            FindSelectableTiles(GetComponent<AllyStats>().mov);
            CheckMouse();
        }
    }

    private void CheckMouse()
    {
        if(Input.GetMouseButtonUp(0))
        {
            Collider2D[] colliders = Physics2D.OverlapBoxAll((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition), new Vector2(0.01f, 0.01f), 0);
            foreach(Collider2D item in colliders)
            {
                if(item.tag == "Tile" && item.GetComponent<Tile>().selectable && item.transform.position != transform.position)
                {
                    MovetToTile(item.GetComponent<Tile>());
                }
            }
        }
    }
}
