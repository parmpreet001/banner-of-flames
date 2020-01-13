using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool walkable = true; //Tile is passable by walking
    public bool current = false; //Tile is current if ally or enemy is standing on it
    public bool target = false; //Tile that the ally or enemy will be moving too
    public bool selectable = false; //Tiles that the ally or enemy can go to/click on
    public bool attackable = false;

    public List<Tile> adjacentTiles = new List<Tile>(); //Adjacent tiles

    //BFS
    public bool visited = false; //TIle has been visited by BFS
    public Tile parent = null; //Starting tile
    public int distance = 0; //How far a tile can be from the starting tile. Movement range

    // Start is called before the first frame update
    void Start()
    {
        FindNeighbors(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (current)
            GetComponent<SpriteRenderer>().color = Color.magenta;
        else if (target)
            GetComponent<SpriteRenderer>().color = Color.green;
        else if (selectable)
            GetComponent<SpriteRenderer>().color = Color.blue;
        else if (attackable)
            GetComponent<SpriteRenderer>().color = Color.red;
        else
            GetComponent<SpriteRenderer>().color = Color.white;
    }

    public void Reset()
    {
        current = false;
        target = false; 
        selectable = false;
        attackable = false;

        adjacentTiles.Clear();

        visited = false;
        parent = null;
        distance = 0;
    }

    public void FindNeighbors(bool ignoreOccupied)
    {
        Reset();
        CheckTile(Vector2.up, ignoreOccupied);
        CheckTile(Vector2.right, ignoreOccupied);
        CheckTile(Vector2.down, ignoreOccupied);
        CheckTile(Vector2.left, ignoreOccupied);
    }

    //Checks tiles in direction
    public void CheckTile(Vector2 direction, bool ignoreOccupied)
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll((Vector2)transform.position + direction, new Vector2(0.5f, 0.5f),0);

        //Iterates through all the colliders in the adjacent area.
        foreach(Collider2D item in colliders)
        {
            Tile tile = item.GetComponent<Tile>();
            //If a tile is found and it has no child objects, meaning that nothing is standing on top of the tile, it gets added to the adjacenctTiles list
            if (tile != null && tile.walkable)
            {
                if(tile.transform.childCount == 0)
                {
                    adjacentTiles.Add(tile);
                }
                else if(tile.transform.childCount == 1 && ignoreOccupied == false)
                {
                    adjacentTiles.Add(tile);
                }
            }
        }
    }
}
