using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour
{
    public bool canMove = true;
    public Tile currentTile; //The tile the cursor is currently on
    public Transform followTarget;
    [SerializeField]
    private int buffer = -1;
    void Update()
    {
        if (followTarget)
        {
            transform.position = followTarget.position;
        }
        
        else if(canMove && buffer == -1)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
                transform.position = (Vector2)transform.position + Vector2.up;
            else if (Input.GetKeyDown(KeyCode.RightArrow))
                transform.position = (Vector2)transform.position + Vector2.right;
            else if (Input.GetKeyDown(KeyCode.DownArrow))
                transform.position = (Vector2)transform.position + Vector2.down;
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
                transform.position = (Vector2)transform.position + Vector2.left;
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.LeftArrow))
                buffer = 60;
                
        }
        else if (canMove && buffer >= 0)
        {
            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow))
            {
                buffer--;
                if(buffer == 0)
                {
                    if (Input.GetKey(KeyCode.UpArrow))
                    {
                        transform.position = (Vector2)transform.position + Vector2.up;
                    }
                    else if(Input.GetKey(KeyCode.RightArrow))
                    {
                        transform.position = (Vector2)transform.position + Vector2.right;
                    }
                    else if(Input.GetKey(KeyCode.DownArrow))
                    {
                        transform.position = (Vector2)transform.position + Vector2.down;
                    }
                    else if (Input.GetKey(KeyCode.LeftArrow))
                    {
                        transform.position = (Vector2)transform.position + Vector2.left;
                    }
                }
                else if (buffer == -1)
                    buffer = 20;
            }
            else
            {
                buffer = -1;
            }
        }
        GetTile();
    }

    public void GetTile()
    {
        Tile tile = null;
        Collider2D[] colliders = Physics2D.OverlapBoxAll((Vector2)transform.position, new Vector2(0.5f, 0.5f), 0);
        foreach (Collider2D item in colliders)
        {
            if (item.transform.tag == "Tile")
            {
                tile = item.GetComponent<Tile>();
            }
        }
        currentTile = tile;
    }

    public bool CurrentTileHasAllyUnit()
    {
        return (currentTile && currentTile.HasAllyUnit());
    }

    public bool CurrentTileHasEnemyUnit()
    {
        return (currentTile && currentTile.HasEnemyUnit());
    }

    public GameObject GetCurrentUnit()
    {
            
        if (CurrentTileHasAllyUnit() || CurrentTileHasEnemyUnit())
            return currentTile.transform.GetChild(0).gameObject;
        else
            return null;
    }

    public void MoveToTile(Tile tile)
    {
        transform.position = new Vector2(tile.transform.position.x, tile.transform.position.y);
    }
}
