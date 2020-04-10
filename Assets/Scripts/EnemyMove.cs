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
        FindSelectableTiles(GetComponent<EnemyStats>().classType.mov, GetComponent<EnemyStats>().classType.walkableTerrain);
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

        targetDistance = GetDistanceBetweenTiles(transform.parent.gameObject,closestTarget.transform.parent.gameObject);
        targetOutsideRange = (targetDistance > (GetComponent<EnemyStats>().classType.mov + GetComponent<EnemyStats>().equippedWeapon.maxRange));

        if (!targetOutsideRange)
        {
            int targetTileDistance = 0; //Distance between the target and the tile during iteration

            for (int i = selectableTiles.Count - 1; i >= 0; i--)
            {
                targetTileDistance = GetDistanceBetweenTiles(closestTarget.transform.parent.gameObject, selectableTiles[i].gameObject);
                if (targetTileDistance < GetComponent<Stats>().equippedWeapon.minRange || targetTileDistance > GetComponent<Stats>().equippedWeapon.maxRange)
                    selectableTiles.RemoveAt(i);
            }
            closestTileToTarget = selectableTiles[0];
        }

        else
        {
            closestTileToTarget = selectableTiles[selectableTiles.Count-1];
            int targetTileDistance = GetDistanceBetweenTiles(closestTarget.transform.parent.gameObject, closestTileToTarget.gameObject);

            for (int i = selectableTiles.Count - 1; i >= 0; i--)
            {
                if(GetDistanceBetweenTiles(closestTarget.transform.parent.gameObject, selectableTiles[i].gameObject) < targetTileDistance)
                {
                    closestTileToTarget = selectableTiles[i];
                    targetTileDistance = GetDistanceBetweenTiles(closestTarget.transform.parent.gameObject, closestTileToTarget.gameObject);
                }
            }
        }
        Debug.Log("Cloest tile to target is " + closestTileToTarget.transform.name + " with a distance of " + targetDistance);
        MovetToTile(closestTileToTarget);
    }
}
