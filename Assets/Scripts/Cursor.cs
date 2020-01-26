using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour
{
    public bool canMove = true;
    public Tile currentTile; //The tile the cursor is currently on
    void Update()
    {
        if(canMove)
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
                GetTile();
        }
    }

    public void GetTile()
    {
        Tile tile = null;
        Collider2D[] colliders = Physics2D.OverlapBoxAll((Vector2)transform.position, new Vector2(0.5f, 0.5f), 0);
        foreach (Collider2D item in colliders)
        {
            if (item.transform.tag == "Tile")
            {
                Debug.Log("TIle found");
                tile = item.GetComponent<Tile>();
            }
        }
        currentTile = tile;
    }

    public bool CurrentTileHasAllyUnit()
    {
        return (currentTile && currentTile.transform.childCount == 1 && currentTile.transform.GetChild(0).tag == "PlayerUnit");
    }

    public bool CurrentTileHasEnemyUnit()
    {
        return (currentTile && currentTile.transform.childCount == 1 && currentTile.transform.GetChild(0).tag == "EnemyUnit");
    }
}
