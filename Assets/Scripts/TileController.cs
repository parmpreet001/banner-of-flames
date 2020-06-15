using System.Collections;
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
                selectableTiles[i].UpdateColors();
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

    public void MoveToTile(GameObject unit, Tile tile)
    {
        path.Clear();
        Tile next = tile;
        while (next != null)
        {
            path.Push(next);
            next = next.parent;
        }
        RemoveSelectableTiles();
        StartCoroutine(WalkToTileAnimation(unit, tile));
    }

    IEnumerator WalkToTileAnimation(GameObject unit, Tile tile)
    {
        Debug.Log(transform.name + "has Stared coroutine");
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
}
