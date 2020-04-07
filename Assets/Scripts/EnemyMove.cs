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

        targetDistance = GetDistanceBetweenTiles(transform.parent.gameObject,closestTarget.transform.parent.gameObject);

        targetOutsideRange = (targetDistance > (GetComponent<EnemyStats>().mov + GetComponent<EnemyStats>().equippedWeapon.maxRange));
        if (!targetOutsideRange)
        {
            int targetTileDistance = 0; //Distance between the target and the tile during iteration

            for (int i = selectableTiles.Count - 1; i >= 0; i--)
            {
                targetTileDistance = GetDistanceBetweenTiles(closestTarget.transform.parent.gameObject, selectableTiles[i].gameObject);
                if (targetTileDistance < GetComponent<Stats>().equippedWeapon.minRange || targetTileDistance > GetComponent<Stats>().equippedWeapon.maxRange)
                    selectableTiles.RemoveAt(i);
            }
            foreach(Tile tile in selectableTiles)
            {
                Debug.Log(tile.transform.name + "," + tile.transform.parent.name + ". Distance: "
                    + Vector2.Distance(closestTarget.transform.position,tile.transform.position));
            }
            closestTileToTarget = selectableTiles[0];
        }
        else
        {
            Debug.Log("Target is outside of range. Distance is " + targetDistance + ". Max range is" + GetComponent<EnemyStats>().mov +
    GetComponent<EnemyStats>().equippedWeapon.maxRange);
            foreach (Tile tile in selectableTiles)
            {
                //TODO The way an enemy with a bow moves when standing next to an ally unit is weird. Fix later
                //Debug.Log("Distance between " + tile.transform.name + " and target is " + Vector2.Distance(tile.transform.position, closestTarget.transform.position));
                if ((Vector2.Distance(tile.transform.position, closestTarget.transform.position) < targetDistance) ||
                    Mathf.Ceil(Vector2.Distance(tile.transform.position, closestTarget.transform.position)) >= GetComponent<Stats>().equippedWeapon.minRange)
                {
                    Debug.Log("Passed first check");
                    if ((Vector2.Distance(transform.position, closestTarget.transform.position) > GetComponent<Stats>().mov + GetComponent<Stats>().equippedWeapon.maxRange ||
                        (Vector2.Distance(tile.transform.position, closestTarget.transform.position) >= GetComponent<Stats>().equippedWeapon.minRange &&
                        Vector2.Distance(tile.transform.position, closestTarget.transform.position) <= GetComponent<Stats>().equippedWeapon.maxRange)) || targetOutsideRange)
                    {
                        closestTileToTarget = tile;
                        targetDistance = Vector2.Distance(closestTileToTarget.transform.position, closestTarget.transform.position);
                    }
                }
            }
        }
        Debug.Log("Cloest tile to target is " + closestTileToTarget.transform.name + " with a distance of " + targetDistance);
        MovetToTile(closestTileToTarget);
    }
}
