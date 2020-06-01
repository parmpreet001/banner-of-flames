using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyMove :TileMove
{
    public bool firstClick = true;
    public bool attacked = false;
    [SerializeField]
    public Tile selectedTile;
    private Tile startingTile; //The tile the unit started on
    private Tile movedTile; //The tile the unit moved to

    private AllyStats _AllyStats; //AllyStats component of the unit this script is attached to
    
    // Start is called before the first frame update
    void Start()
    {
        _AllyStats = GetComponent<AllyStats>();
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        if (selected)
        {
            if(moving)
            {
                cursor.followTarget = transform;
            }
            else
            {
                cursor.followTarget = null;
            }
            CheckCursor();
            if (finished)
                UnselectUnit();
            else
                CheckInput();
        }
        if (_AllyStats.isDead)
            cursor.canMove = true;
    }

    private void CheckInput()
    {
        //If the Z key is pressed
        if (Input.GetKeyUp(KeyCode.Z) && !firstClick)
        {
            if (actionMenu)
            {
                
            }
            else if (attacking)
            {
                cursor.canMove = true;
            }
            else if (findingTarget)
            {
                cursor.GetTile();
                selectedTile = cursor.currentTile;
                if (selectedTile.attackable)
                {
                    attacked = true;
                }
            }
            else if(moved)
            {
                
            }
            //If unit is not moving
            else if (!moving)
            {
                selectedTile = cursor.currentTile;

                if (selectedTile)
                {
                    if (selectedTile.selectable && !selectedTile.current)
                    {
                        MovetToTile(selectedTile);
                        movedTile = selectedTile;
                    }
                      
                    else if(selectedTile.current)
                    {
                        actionMenu = true;
                        RemoveSelectableTiles();
                        if(_AllyStats.equippedWeapon)
                            FindAttackableTiles(_AllyStats.equippedWeapon.minRange, _AllyStats.equippedWeapon.maxRange);
                    }      
                }
            }
        }
        else if(Input.GetKeyUp(KeyCode.X))
        {
            if(moving)
            {

            }
            else if(findingTarget)
            {
                if(!actionMenu)
                    actionMenu = true;
                else if(moved && !attacking)
                {
                    transform.SetParent(startingTile.transform);
                    transform.position = startingTile.transform.position;
                    RemoveSelectableTiles();
                    moved = false;
                    actionMenu = false;
                    findingTarget = false;
                    FindSelectableTiles(_AllyStats.classType.mov, _AllyStats.classType.walkableTerrain, true);
                }

            }
            else if(moved)
            {
                if(!attacking)
                {
                    transform.SetParent(startingTile.transform);
                    transform.position = startingTile.transform.position;
                    RemoveSelectableTiles();
                    moved = false;
                    actionMenu = false;
                    findingTarget = false;
                    FindSelectableTiles(_AllyStats.classType.mov, _AllyStats.classType.walkableTerrain, true);
                }
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
            FindSelectableTiles(_AllyStats.classType.mov, _AllyStats.classType.walkableTerrain, true);
            startingTile = currentTile;
            Debug.Log("Starting on " + startingTile.transform.name + "," + startingTile.transform.parent.name);
        }     
    }

    private void CheckCursor()
    {
        if (attacking || actionMenu)
            cursor.canMove = false;
        else
            cursor.canMove = true;
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
        cursor.canMove = true;
        cursor.followTarget = null;
        RemoveSelectableTiles();
    }
}
