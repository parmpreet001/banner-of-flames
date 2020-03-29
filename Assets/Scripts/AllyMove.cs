using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyMove :TileMove
{
    public bool firstClick = true;
    public bool attacked = false;
    [SerializeField]
    public GameObject selectedTile;
    private Tile startingTile; //The tile the unit started on
    private Tile movedTile; //The tile the unit moved to
    
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
            if(moving)
            {
                cursor.GetComponent<Cursor>().followTarget = transform;
            }
            else
            {
                cursor.GetComponent<Cursor>().followTarget = null;
            }
            CheckCursor();
            if (finished)
                UnselectUnit();
            else
                CheckInput();
        }
        if (GetComponent<Stats>().isDead)
            cursor.GetComponent<Cursor>().canMove = true;
    }

    private void CheckInput()
    {
        if (Input.GetKeyUp(KeyCode.Z) && !firstClick)
        {
            if (actionMenu)
            {
                Debug.Log("Action menu");
            }
            else if (attacking)
            {
                cursor.GetComponent<Cursor>().canMove = true;
            }
            else if (findingTarget)
            {
                cursor.GetComponent<Cursor>().GetTile();
                selectedTile = cursor.GetComponent<Cursor>().currentTile.gameObject;
                if (selectedTile.GetComponent<Tile>().attackable)
                {
                    //GetComponent<Stats>().Attack(selectedTile.transform.GetChild(0).gameObject);
                    attacked = true;
                }
            }
            else if(moved)
            {
                
            }
            //If unit is not moving
            else if (!moving)
            {
                selectedTile = cursor.GetComponent<Cursor>().currentTile.gameObject;

                if (selectedTile)
                {
                    //If the selectedTile is selectable and is not the same one the unit is standing on
                    if (selectedTile.GetComponent<Tile>().selectable && !selectedTile.GetComponent<Tile>().current)
                    {
                        MovetToTile(selectedTile.GetComponent<Tile>());
                        movedTile = selectedTile.GetComponent<Tile>();
                    }
                      
                    else if(selectedTile.GetComponent<Tile>().current)
                    //else if ((!selectedTile.GetComponent<Tile>().selectable || selectedTile.GetComponent<Tile>().current) && selectedTile.GetComponent<Tile>().transform.childCount == 0)
                    {
                        actionMenu = true;
                        RemoveSelectableTiles();
                        FindAttackableTiles(GetComponent<AllyStats>().equippedWeapon.minRange,GetComponent<AllyStats>().equippedWeapon.maxRange);
                        //UnselectUnit();
                    }
                        
                }
            }
        }
        else if(Input.GetKeyUp(KeyCode.X))
        {
            if(findingTarget && !actionMenu)
            {
                actionMenu = true;
            }
            else if(moved && !attacking)
            {
                transform.SetParent(startingTile.transform);
                transform.position = startingTile.transform.position;
                RemoveSelectableTiles();
                moved = false;
                actionMenu = false;
                findingTarget = false;
                FindSelectableTiles(GetComponent<AllyStats>().mov);
            }
            else if(!attacking)
            {
                transform.SetParent(startingTile.transform);
                transform.position = startingTile.transform.position;
                UnselectUnit();
            }
            if(!attacking)
                cursor.transform.position = transform.parent.transform.position;
        }
        else if(firstClick)
        {
            firstClick = false;
            FindSelectableTiles(GetComponent<AllyStats>().mov);
            startingTile = currentTile;
            Debug.Log("Starting on " + startingTile.transform.name + "," + startingTile.transform.parent.name);
        }     
    }

    private void CheckCursor()
    {
        if (attacking || actionMenu)
            cursor.GetComponent<Cursor>().canMove = false;
        else
            cursor.GetComponent<Cursor>().canMove = true;
    }

    public void UnselectUnit()
    {
        selected = false;
        moving = false;
        findingTarget = false;
        attacking = false;
        firstClick = true;
        moved = false;
        actionMenu = false;
        cursor.GetComponent<Cursor>().canMove = true;
        cursor.GetComponent<Cursor>().followTarget = null;
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
