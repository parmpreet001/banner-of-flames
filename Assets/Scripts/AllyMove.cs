using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyMove :TileMove
{
    public bool firstClick = true;
    private GameObject selectedTile;
    private Tile startingTile;
    
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
                UnselectUnit();
            else
                CheckMouse();
        }
    }

    private void CheckMouse()
    {
        if (Input.GetMouseButtonUp(0) && !firstClick)
        {
            if (actionMenu)
            {

            }
            else if (attacking)
            {
                //Nothing here yet
            }
            else if (findingTarget)
            {
                selectedTile = GetTile();
                if (selectedTile.GetComponent<Tile>().attackable)
                    GetComponent<Stats>().Attack(selectedTile.transform.GetChild(0).gameObject);
            }
            //If unit is not moving
            else if (!moving)
            {
                selectedTile = GetTile();

                if (selectedTile)
                {
                    //If the selectedTile is selectable and is not the same one the unit is standing on
                    if (selectedTile.GetComponent<Tile>().selectable && !selectedTile.GetComponent<Tile>().current)
                        MovetToTile(selectedTile.GetComponent<Tile>());
                    else if (!selectedTile.GetComponent<Tile>().selectable || selectedTile.GetComponent<Tile>().current)
                    {
                        actionMenu = true;
                        RemoveSelectableTiles();
                        //UnselectUnit();
                    }
                        
                }
            }
        }
        else if(Input.GetMouseButtonUp(1))
        {
            transform.SetParent(startingTile.transform);
            transform.position = startingTile.transform.position;
            UnselectUnit();
        }
        else if(firstClick)
        {
            firstClick = false;
            FindSelectableTiles(GetComponent<AllyStats>().mov);
            startingTile = currentTile;
            Debug.Log("Starting on " + startingTile.transform.name + "," + startingTile.transform.parent.name);
        }
            
    }

    private void UnselectUnit()
    {
        selected = false;
        moving = false;
        findingTarget = false;
        attacking = false;
        firstClick = true;
        RemoveSelectableTiles();

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

    private void ShowMenu()
    {
        actionMenu = true;
    }
}
