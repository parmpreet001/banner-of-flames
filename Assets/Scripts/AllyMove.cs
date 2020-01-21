using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyMove :TileMove
{
    private GameObject selectedTile;
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        if (selected)
        {
            if (moved)
                selected = false;
            else
                CheckMouse();
        }
    }

    private void CheckMouse()
    {
        if(Input.GetMouseButtonUp(0))
        {
            if (attacking)
            {
                //Nothing here yet
            }
            else if(findingTarget)
            {
                selectedTile = GetTile();
                if (selectedTile.GetComponent<Tile>().attackable)
                    GetComponent<Stats>().Attack(selectedTile.transform.GetChild(0).gameObject);
            }
            //If unit is not moving
            else if(!moving)
            {
                FindSelectableTiles(GetComponent<AllyStats>().mov);
                selectedTile = GetTile();
                //If the selectedTile is selectable and is not the same one the unit is standing on
                if(selectedTile && selectedTile.GetComponent<Tile>().selectable && !selectedTile.GetComponent<Tile>().current)
                    MovetToTile(selectedTile.GetComponent<Tile>());
            }
        }
    }

    private GameObject GetTile()
    {
        GameObject tile = null;
        Collider2D[] colliders = Physics2D.OverlapBoxAll((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition), new Vector2(0.01f, 0.01f), 0);
        foreach(Collider2D item in colliders)
        {
            if (item.tag == "Tile")
            {
                tile = item.gameObject;
            }
        }
        return tile;
    }
}
