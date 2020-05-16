using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TileMove : MonoBehaviour
{
    public GameObject cursor;
    public List<Tile> selectableTiles = new List<Tile>(); //List of tiles that can be selected
    GameObject[] tiles;

    Stack<Tile> path = new Stack<Tile>();
    public Tile currentTile = new Tile(); //Tile the unit starts on

    public bool selected = false;
    public bool moving = false;
    public bool moved = false;
    public bool findingTarget = false;
    public bool attacking = false;
    public bool actionMenu = false;
    public bool finished = false;



    protected void Init()
    {
        cursor = GameObject.Find("Cursor");
        tiles = GameObject.FindGameObjectsWithTag("Tile");
    }

    //Gets tile under the unit
    public void GetCurrentTile()
    {
        currentTile = GetTargetTile(gameObject);
        currentTile.current = true;
    }

    public Tile GetTargetTile(GameObject target)
    {
        Tile tile = null;
        Collider2D[] colliders = Physics2D.OverlapBoxAll((Vector2)transform.position, new Vector2(0.5f, 0.5f), 0);
        foreach(Collider2D item in colliders)
        {
            if(item.transform.tag == "Tile")
            {
                tile = item.GetComponent<Tile>();
            }
        }
        return tile;
    }

    public void ComputeAdjacentLists(bool ignoreOccupied)
    {
        foreach (GameObject tile in tiles)
        {
            Tile t = tile.GetComponent<Tile>();
            t.FindNeighbors(ignoreOccupied);
        }
    }

    public void FindSelectableTiles(int moveRange, TerrainType[] terrain)
    {
        ComputeAdjacentLists(true);
        GetCurrentTile();

        Queue<Tile> process = new Queue<Tile>();

        process.Enqueue(currentTile); //Adds the tile the unit is standing on to the process Queue
        currentTile.visited = true;
        
        //Loop runs while process has tiles in it. Adds all tiles within unit's move range to the process queue. 
        while(process.Count > 0)
        {
            Tile t = process.Dequeue(); //Removes the tile from process and assigns it to Tile t

            selectableTiles.Add(t); //The tile is added to the list of selectable tiles
            t.selectable = true; //The tile is marked as selectable

            if (t.distance < moveRange) //True if the tile is within the unit's move range
            {
                foreach(Tile tile in t.adjacentTiles) //For each tile adjacent to the current tile
                {
                    if (!tile.visited) //True if the tile has not already been visited by the search
                    {
                        bool validTerrain = false;
                        for(int i = 0; i < terrain.Length; i++)
                        {
                            if (terrain[i] == tile.terrainType)
                                validTerrain = true;
                        }
                        if(validTerrain)
                        {
                            tile.parent = t; //The parent of the adjacent tile is the current tile
                            tile.visited = true; //The tile is marked as visited
                            tile.distance = 1 + t.distance; //The distance of the is equal to the distance of the parent tile plus one
                            process.Enqueue(tile); //The tile gets added to the proces queue
                        }

                    }
                }
            }
        }
    }

    public void MovetToTile(Tile tile)
    {
        moving = true;
        path.Clear();
        tile.target = true;

        Tile next = tile;
        while(next != null)
        {
            path.Push(next);
            next = next.parent;
        }
        RemoveSelectableTiles();
        StartCoroutine(WalkToTileAnimation(tile));
    }

    public void RemoveSelectableTiles()
    {
        if(currentTile != null)
        {
            currentTile.current = false;
            currentTile = null;
        }

        foreach(Tile tile in selectableTiles)
        {
            tile.Reset();
        }

        selectableTiles.Clear();
    }

    IEnumerator WalkToTileAnimation(Tile tile)
    {
        Debug.Log(transform.name + "has Stared coroutine");
        Tile t = path.Pop();
        while(path.Count > 0)
        {
            transform.SetParent(t.transform);
            transform.position = t.transform.position;
            t = path.Pop();
            yield return new WaitForSeconds(0.25f);
        }
        transform.SetParent(tile.transform);
        transform.position = tile.transform.position;
        moving = false;
        moved = true;
        Debug.Log(transform.name + "has reached end of coroutine");
        if(GetComponent<Stats>().equippedWeapon)
            FindAttackableTiles(GetComponent<Stats>().equippedWeapon.minRange,GetComponent<Stats>().equippedWeapon.maxRange);
        else
            actionMenu = true;
        yield return null;
    } 
    
    //Highlights all targets the unit can attack
    public void FindAttackableTiles(int minRange, int maxRange)
    {
        ComputeAdjacentLists(false);
        GetCurrentTile();

        Queue<Tile> process = new Queue<Tile>();

        process.Enqueue(currentTile); //Adds the tile the unit is standing on to the process Queue
        currentTile.visited = true;

        //Loop runs while process has tiles in it. Adds all tiles within unit's move range to the process queue. 
        while (process.Count > 0)
        {
            Tile t = process.Dequeue(); //Removes the tile from process and assigns it to Tile t

            //If the tile is within the unit's attack range, and it has an attackable unit on it, then that tile gets added to the list of selectableTiles
            if(t.distance >= minRange && t.distance <= maxRange && t.transform.childCount == 1)
            {
                if(tag != t.transform.GetChild(0).tag)
                {
                    selectableTiles.Add(t); //The tile is added to the list of selectable tiles
                    t.attackable = true; //The tile is marked as selectable
                }
            }

            if (t.distance < maxRange) //True if the tile is within the unit's move range
            {
                foreach (Tile tile in t.adjacentTiles) //For each tile adjacent to the current tile
                {
                    if (!tile.visited) //True if the tile has not already been visited by the search
                    {
                        tile.parent = t; //The parent of the adjacent tile is the current tile
                        tile.visited = true; //The tile is marked as visited
                        tile.distance = 1 + t.distance; //The distance of the is equal to the distance of the parent tile plus one
                        process.Enqueue(tile); //The tile gets added to the proces queue
                    }
                }
            }
        }
        if(selectableTiles.Count != 0)
        {
            findingTarget = true;
        }
        if (transform.tag == "EnemyUnit" && findingTarget == false)
        {
            selected = false;
            finished = true;
            RemoveSelectableTiles();
        }
        else
            actionMenu = true;
    }

    public int GetDistanceBetweenTiles(GameObject tile1, GameObject tile2)
    {
        int xDistance = (int)Mathf.Abs(tile1.transform.position.x - tile2.transform.position.x);
        int yDistance = (int)Mathf.Abs(tile1.transform.position.y - tile2.transform.position.y);
        return (xDistance + yDistance);
    }
}
