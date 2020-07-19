using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public bool walkable = true; //Tile is passable by walking
    public bool current = false; //Tile is current if ally or enemy is standing on it
    public bool target = false; //Tile that the ally or enemy will be moving too
    public bool selectable = false; //Tiles that the ally or enemy can go to/click on
    public bool attackable = false;
    public bool healable = false;

    public TerrainType terrainType;
    [SerializeField]
    public TerrainEffect terrainEffect;

    public List<Tile> adjacentTiles = new List<Tile>(); //Adjacent tiles

    public int fireTurnCount = -1;

    //BFS
    public bool visited = false; //TIle has been visited by BFS
    public Tile parent = null; //Starting tile
    public int distance = 0; //How far a tile is from the starting tile. Movement range

    void Start()
    {
        FindNeighbors(true);
    }

    public void Reset()
    {
        current = false;
        target = false; 
        selectable = false;
        attackable = false;
        healable = false;

        adjacentTiles.Clear();

        visited = false;
        parent = null;
        distance = 0;
    }

    //Find adjacent tiles
    public void FindNeighbors(bool ignoreOccupied)
    {
        Reset();
        CheckTile(Vector2.up, ignoreOccupied);
        CheckTile(Vector2.right, ignoreOccupied);
        CheckTile(Vector2.down, ignoreOccupied);
        CheckTile(Vector2.left, ignoreOccupied);
    }

    //Checks to see if there is a tile in the specified direction
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
                if(!tile.HasUnit())
                    adjacentTiles.Add(tile);
                else if(tile.HasUnit() && ignoreOccupied == false)
                    adjacentTiles.Add(tile);
            }
        }
    }

    //Returns true if the child has a unit, otherwise returns false
    public bool HasUnit()
    {
        return (transform.childCount == 1);
    }
    public bool HasAllyUnit()
    {
        return (HasUnit() && transform.GetChild(0).tag == "PlayerUnit");
    }

    public bool HasEnemyUnit()
    {
        return (HasUnit() && transform.GetChild(0).tag == "EnemyUnit");
    }

    public GameObject GetUnit()
    {
        return transform.GetChild(0).gameObject;
    }

    public void UpdateColors()
    {
        if (current)
            GetComponent<SpriteRenderer>().color = Color.magenta;
        else if (target)
            GetComponent<SpriteRenderer>().color = Color.green;
        else if (selectable)
            GetComponent<SpriteRenderer>().color = Color.blue;
        else if (attackable)
            GetComponent<SpriteRenderer>().color = Color.red;
        else if (healable)
            GetComponent<SpriteRenderer>().color = Color.green;
        else
            GetComponent<SpriteRenderer>().color = Color.white;
    }

    //Sets the terrain on fire
    public void SetOnFire()
    {
        fireTurnCount = 3;
        GetComponent<SpriteRenderer>().sprite = (Sprite)AssetDatabase.LoadAssetAtPath("Assets/Images/TileEnflamedGrass.png",typeof(Sprite));
        terrainEffect = (TerrainEffect)AssetDatabase.LoadAssetAtPath("Assets/ScriptableObjects/TerrainEffects/Enflamed Grass.asset", typeof(TerrainEffect));
    }
}
