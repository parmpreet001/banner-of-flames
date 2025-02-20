﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour
{
    public Cursor cursor; 
    public List<Tile> selectableTiles = new List<Tile>(); //List of tiles that can be selected
    GameObject[] tiles; //Array of all tiles 
    private Tile currentTile;

    Stack<Tile> path = new Stack<Tile>();

    public void Init()
    {
        cursor = GameObject.Find("Cursor").GetComponent<Cursor>();
        tiles = GameObject.FindGameObjectsWithTag("Tile");
    }

    //Sets the current tile to the one the unit is standing on
    public void SetCurrentTile(GameObject unit)
    {
        currentTile = GetTargetTile(unit);
        currentTile.current = true;
    }

    //Gets the tile the unit is currently standing
    public Tile GetTargetTile(GameObject target)
    {
       return target.GetComponentInParent<Tile>();
    }

    public void SetUnitToTile(GameObject unit, Tile tile)
    {
        unit.transform.SetParent(tile.transform);
        unit.transform.position = tile.transform.position;
    }

    //Sets adjacency list for all tiles
    public void ComputeAdjacentLists(bool ignoreOccupied)
    {
        foreach (GameObject tile in tiles)
        {
            Tile t = tile.GetComponent<Tile>();
            t.FindNeighbors(ignoreOccupied);
        }
    }

    //Finds all tiles that the unit can move to
    public void FindSelectableTiles(int moveRange, TerrainType[] terrain, bool updateTTileColors)
    {
        ComputeAdjacentLists(true);
        FindTilesWithinDistance(0, moveRange, terrain);
        for (int i = selectableTiles.Count - 1; i >= 0; i--)
        {
            if (selectableTiles[i].HasUnit())
                selectableTiles.RemoveAt(i);
            else
            {
                selectableTiles[i].selectable = true;
                if(updateTTileColors)
                    selectableTiles[i].UpdateColors();
            }
        }
    }

    public void ShowAttackRange(int moveRange, TerrainType[] terrain, int minAttackRange, int maxAttackRange)
    {
        ComputeAdjacentLists(true);
        FindTilesWithinDistance(0, moveRange, terrain);
        foreach(Tile tile in selectableTiles)
        {
            tile.selectable = true;
            tile.UpdateColors();
        }
        foreach(Tile tile in selectableTiles)
        {
            foreach(Tile adjacentTile in tile.adjacentTiles)
            {
                if(!adjacentTile.selectable)
                {
                    adjacentTile.attackable = true;
                    adjacentTile.UpdateColors();
                }
            }
        }
        ComputeAdjacentLists(false);
        foreach(Tile tile in selectableTiles)
        {
            foreach(Tile adjacentTile in tile.adjacentTiles)
            {
                if(adjacentTile.HasAllyUnit())
                {
                    adjacentTile.attackable = true;
                    adjacentTile.UpdateColors();
                }
            }
        }
    }

    public void RemoveSelectableTiles()
    {
        if (currentTile != null)
        {
            currentTile.current = false;
            currentTile = null;
        }

        foreach (Tile tile in selectableTiles)
        {
            tile.Reset();
            tile.UpdateColors();
        }

        selectableTiles.Clear();
    }

    public void RemoveAllTiles()
    {
        for(int i = 0; i < tiles.Length; i++)
        {
            tiles[i].GetComponent<Tile>().Reset();
            tiles[i].GetComponent<Tile>().UpdateColors();
        }
    }

    public void ShowWeaponRange(int minRange, int maxRange)
    {
        ComputeAdjacentLists(false);
        FindTilesWithinDistance(minRange, maxRange, null);

        foreach (Tile tile in selectableTiles)
        {
            tile.attackable = true;
            tile.UpdateColors();
        }
    }

    //Gets all tiles within a set distance and of type terrain, and then adds them to selectableTiles
    private void FindTilesWithinDistance(int min, int max, TerrainType[] terrain)
    {
        Queue<Tile> process = new Queue<Tile>();

        process.Enqueue(currentTile); //Adds the tile the unit is standing on to the process Queue
        currentTile.visited = true;

        //Loop runs while process has tiles in it. Adds all tiles within unit's move range to the process queue. 
        while (process.Count > 0)
        {
            Tile t = process.Dequeue(); //Removes the tile from process and assigns it to Tile t

            //If the tile is within the unit's attack range
            if (t.distance >= min && t.distance <= max)
            {
                if (CheckTileTerrain(t, terrain))
                    selectableTiles.Add(t); //The tile is added to the list of selectable tiles
            }

            if (t.distance < max) //True if the tile is within the unit's move range
            {
                foreach (Tile tile in t.adjacentTiles) //For each tile adjacent to the current tile
                {
                    if (!tile.visited && CheckTileTerrain(tile, terrain)) //True if the tile has not already been visited by the search
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

    //Returns true if the tile is of type terrain, false otherwise
    private bool CheckTileTerrain(Tile tile, TerrainType[] terrain)
    {
        if (terrain == null)
            return true;
        bool validTerrain = false;
        foreach (TerrainType terrainType in terrain)
        {
            if (tile.terrainType == terrainType)
                validTerrain = true;
        }
        return validTerrain;
    }

    //Returns a list of adjacent tiles that have the same terrain type as tile
    public void GetAdjacenTileTerrainType(Tile startingTile)
    {
        ComputeAdjacentLists(false);
        Queue<Tile> process = new Queue<Tile>();
        process.Enqueue(startingTile);
        startingTile.visited = true;

        while(process.Count > 0)
        {
            Tile t = process.Dequeue();

            if(t.terrainType == startingTile.terrainType)
            {
                selectableTiles.Add(t);

                foreach(Tile adjacentTile in t.adjacentTiles)
                {
                    if (!adjacentTile.visited) //True if the tile has not already been visited by the search
                    {
                    adjacentTile.parent = t; //The parent of the adjacent tile is the current tile
                    adjacentTile.visited = true; //The tile is marked as visited
                    adjacentTile.distance = 1 + t.distance; //The distance of the is equal to the distance of the parent tile plus one
                    process.Enqueue(adjacentTile); //The tile gets added to the proces queue
                    }
                }
            }
        }
    }

    public IEnumerator MoveToTile(GameObject unit, Tile tile)
    {
        path.Clear();
        Tile next = tile;
        while (next != null)
        {
            path.Push(next);
            next = next.parent;
        }
        RemoveSelectableTiles();
        yield return WalkToTileAnimation(unit, tile);
    }

    IEnumerator WalkToTileAnimation(GameObject unit, Tile tile)
    {
        Tile t = path.Pop();
        while (path.Count > 0)
        {
            unit.transform.SetParent(t.transform);
            unit.transform.position = t.transform.position;
            t = path.Pop();
            yield return new WaitForSeconds(0.25f);
        }
        unit.transform.SetParent(tile.transform);
        unit.transform.position = tile.transform.position;
        yield return null;
    }

    public bool EnemyInRange(int minRange, int maxRange)
    {
        ComputeAdjacentLists(false);
        FindTilesWithinDistance(minRange, maxRange, null);
        foreach (Tile tile in selectableTiles)
        {
            if (tile.HasEnemyUnit())
                return true;
        }
        return false;
    }

    public bool AllyInRange(int minRange, int maxRange)
    {
        ComputeAdjacentLists(false);
        FindTilesWithinDistance(minRange, maxRange, null);
        foreach (Tile tile in selectableTiles)
        {
            if (tile.HasAllyUnit())
                return true;
        }
        return false;
    }

    public void FindHealableTiles(int minRange, int maxRange)
    {
        ComputeAdjacentLists(false);
        FindTilesWithinDistance(minRange, maxRange, null);

        for (int i = selectableTiles.Count - 1; i >= 0; i--)
        {
            if (selectableTiles[i].HasAllyUnit() && selectableTiles[i].GetUnit().GetComponent<Stats>().hp 
                != selectableTiles[i].GetUnit().GetComponent<Stats>().maxHP)
            {
                selectableTiles[i].healable = true;
                selectableTiles[i].UpdateColors();
            }
            else
            {
                selectableTiles.RemoveAt(i);
            }
        }
    }

    public int GetDistanceBetweenTiles(GameObject tile1, GameObject tile2)
    {
        int xDistance = (int)Mathf.Abs(tile1.transform.position.x - tile2.transform.position.x);
        int yDistance = (int)Mathf.Abs(tile1.transform.position.y - tile2.transform.position.y);
        return (xDistance + yDistance);
    }

    //Returns the closest ally target that the enemy unit can attack
    public GameObject GetClosestTarget(GameObject unit)
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("PlayerUnit"); //All possible targets
        GameObject closestTarget = targets[0]; //Closest target. 

        foreach (GameObject target in targets)
        { 
            if(GetDistanceBetweenTiles(target.transform.parent.gameObject,unit.transform.parent.gameObject) 
                < GetDistanceBetweenTiles(closestTarget.transform.parent.gameObject, unit.transform.parent.gameObject))
            {
                closestTarget = target;
            }
            //if (Vector2.Distance(unit.transform.position, target.transform.position) < Vector2.Distance(unit.transform.position, closestTarget.transform.position))
            //    closestTarget = target;
        }

        return closestTarget;
    }

    public Tile GetClosestTileToTarget(GameObject unit, GameObject target)
    {
        RemoveSelectableTiles();
        SetCurrentTile(unit);
        FindSelectableTiles(unit.GetComponent<Stats>().classType.mov, unit.GetComponent<Stats>().classType.walkableTerrain, false);

        int minRange = unit.GetComponent<EnemyStats>().GetMinRange();
        int maxRange = unit.GetComponent<EnemyStats>().GetMaxRange();
        Tile closestTileToTarget = unit.GetComponentInParent<Tile>();
        bool targetOutsideRange = true;
        Debug.Log("Target outside range: " + targetOutsideRange);

        foreach(Tile tile in selectableTiles)
        {
            if(GetDistanceBetweenTiles(tile.gameObject,target.transform.parent.gameObject) <= unit.GetComponent<Stats>().GetMaxRange())
            {
                targetOutsideRange = false;
            }
        }

        if (!targetOutsideRange)
        {
            if(GetDistanceBetweenTiles(unit.transform.parent.gameObject, target.transform.parent.gameObject) <= unit.GetComponent<Stats>().GetMaxRange())
            {
                closestTileToTarget = unit.GetComponentInParent<Tile>();
            }
            else
            {
                int targetTileDistance = 0; //Distance between the target and the tile during iteration

                //Find closest tile that this unit can attack from
                for (int i = selectableTiles.Count - 1; i >= 0; i--)
                {
                    targetTileDistance = GetDistanceBetweenTiles(target.transform.parent.gameObject, selectableTiles[i].gameObject);
                    if (targetTileDistance < minRange || targetTileDistance > maxRange)
                        selectableTiles.RemoveAt(i);
                }
                closestTileToTarget = selectableTiles[0];
            }
        }

        else if(targetOutsideRange && selectableTiles.Count > 0)
        {
            closestTileToTarget = selectableTiles[selectableTiles.Count - 1];
            int targetTileDistance = GetDistanceBetweenTiles(target.transform.parent.gameObject, closestTileToTarget.gameObject);

            //Find tile closest to target
            for (int i = selectableTiles.Count - 1; i >= 0; i--)
            {
                if (GetDistanceBetweenTiles(target.transform.parent.gameObject, selectableTiles[i].gameObject) < targetTileDistance)
                {
                    closestTileToTarget = selectableTiles[i];
                    targetTileDistance = GetDistanceBetweenTiles(target.transform.parent.gameObject, closestTileToTarget.gameObject);
                }
            }
        }
        return closestTileToTarget;
    }
}
