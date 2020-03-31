using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : TileMove
{
    public GameObject closestTarget;
    private bool targetOutsideRange;
    void Start()
    {
        Init();   
    }

    void Update()
    {
        if (moving)
        {
            cursor.GetComponent<Cursor>().followTarget = transform;
        }
    }
    public void Move()
    {
        FindSelectableTiles(GetComponent<EnemyStats>().mov);
        GetClosestTarget();
    }

    public void GetClosestTarget()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("PlayerUnit"); //All possible targets
        closestTarget = targets[0]; //Closest target. 
        double targetDistance = 0; //Distance to the closestTarget
        Tile closestTileToTarget = selectableTiles[0]; //Tile closest to the target. Default value is the tile the enemy is currently standing on

        //Finds closest target
        foreach (GameObject target in targets)
        {
            if (Vector2.Distance(transform.position, target.transform.position) < Vector2.Distance(transform.position, closestTarget.transform.position))
                closestTarget = target;
            
        }
        //Debug.Log("Closest target to " + transform.name + " is " + closestTarget.transform.name);
        targetDistance = Vector2.Distance(transform.position, closestTarget.transform.position);
        //Debug.Log("Distance is " + targetDistance);

        targetOutsideRange = (targetDistance > GetComponent<EnemyStats>().mov);

        foreach (Tile tile in selectableTiles)
        {
            //Debug.Log("Distance between " + tile.transform.name + " and target is " + Vector2.Distance(tile.transform.position, closestTarget.transform.position));
            if (Vector2.Distance(tile.transform.position, closestTarget.transform.position) < targetDistance)
            {
                //Debug.Log("here");
                if((Vector2.Distance(transform.position,closestTarget.transform.position) > GetComponent<Stats>().mov + GetComponent<Stats>().equippedWeapon.maxRange ||
                    (Vector2.Distance(tile.transform.position, closestTarget.transform.position) >= GetComponent<Stats>().equippedWeapon.minRange &&
                    Vector2.Distance(tile.transform.position, closestTarget.transform.position) <= GetComponent<Stats>().equippedWeapon.maxRange)) || targetOutsideRange)
                {
                    closestTileToTarget = tile;
                    targetDistance = Vector2.Distance(closestTileToTarget.transform.position, closestTarget.transform.position);
                }
            }
        }
        Debug.Log("Cloest tile to target is " + closestTileToTarget.transform.name + " with a distance of " + targetDistance);
        MovetToTile(closestTileToTarget);
    }
}
