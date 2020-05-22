using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static int turn = 1; //What turn it is. Currently static but might change later
    public bool playerPhase = true; //True if player phase, false if enemy phase
    private int unmovedAllyUnits; //During the player's turn, the number of ally units that have not moved yet
    private int activeEnemyUnits; //Total number of active(aka not dead enemy units) on the map
    private List<GameObject> allyUnits = new List<GameObject>(); //List of all alive ally units on the map
    private List<GameObject> enemyUnits = new List<GameObject>(); //List of all alive enemy units on the map
    public GameObject selectedUnit; //The currently selected unit. Can be either a ally unit or an enemy unit.
    private BattleManager battleManager; //BattleManager script
    GameObject selectedTile;
    public GameObject cursor;


    void Start()
    {
        //Adds each AllyUnit to the allyUnits list
        foreach(GameObject allyUnit in GameObject.FindGameObjectsWithTag("PlayerUnit"))
        {
            unmovedAllyUnits += 1;
            allyUnits.Add(allyUnit);
        }
        //Adds each enemyUnit to  the enemy unit list
        foreach (GameObject enemyUnit in GameObject.FindGameObjectsWithTag("EnemyUnit"))
        {
            activeEnemyUnits += 1;
            enemyUnits.Add(enemyUnit);
        }

        battleManager = GetComponent<BattleManager>();
    }
    void Update()
    {
        if(playerPhase)
        {
            if (unmovedAllyUnits == 0)
                StartEnemyPhase();
            else
                PlayerPhase();
        }
        else
        {
            EnemyPhase();
        }
    }

    //Checks where the player clicked
    private void PlayerPhase()
    {
        //If a unit is selected
        if (selectedUnit)
        {
            //if the unit has finished moving
            if (selectedUnit.GetComponent<AllyMove>().finished)
            {
                selectedUnit.GetComponent<SpriteRenderer>().color = Color.gray;
                if (selectedUnit.GetComponent<Stats>().isDead)
                    Destroy(selectedUnit);
                selectedUnit = null;
                unmovedAllyUnits -= 1;
                Debug.Log("Number of unmoved ally units is " + unmovedAllyUnits);
            }
            //If the unit just attacked an enemy
            else if(selectedUnit.GetComponent<AllyMove>().attacked)
            {
                Debug.Log("Attacking");
                battleManager.attackingUnit = selectedUnit;
                battleManager.defendingUnit = cursor.GetComponent<Cursor>().GetCurrentUnit();
                battleManager.Attack();
                selectedUnit.GetComponent<AllyMove>().attacked = false;
                
            }
            //if the player is selecting a target and is hovering over a tile with an enemy on it
            else if(selectedUnit.GetComponent<AllyMove>().moved && cursor.GetComponent<Cursor>().CurrentTileHasEnemyUnit())
            {
                battleManager.attackingUnit = selectedUnit;
                battleManager.defendingUnit = cursor.GetComponent<Cursor>().GetCurrentUnit();
                battleManager.UpdateStats();
            }
            else if (!selectedUnit.GetComponent<AllyMove>().selected)
                selectedUnit = null;
        }
        if (Input.GetKeyUp(KeyCode.Z))
        {
            //If a unit is not selected
            if (!selectedUnit)
            {
                selectedTile = cursor.GetComponent<Cursor>().currentTile.gameObject; 
                //If selectedTile is not null, meaning that the player didnt click outside the map
                if(selectedTile)
                {
                    //If a player unit is standing on the tile and has not moved
                    if(selectedTile.transform.childCount == 1 && selectedTile.transform.GetChild(0).tag == "PlayerUnit" &&
                        !selectedTile.transform.GetChild(0).GetComponent<AllyMove>().finished)
                    {
                        selectedUnit = selectedTile.transform.GetChild(0).gameObject;
                        selectedUnit.GetComponent<AllyMove>().selected = true;
                    }
                }
            }
        }
    }

    //Checks and updates the value of unmovedAllyUnits
    private void CheckUnmovedUnits()
    {
        //If the selected unit has moved and is not currently moving(meaning that it has reaching the end of its walk animation coroutine)
        if (selectedUnit && selectedUnit.GetComponent<AllyMove>().finished && !selectedUnit.GetComponent<AllyMove>().moving && !selectedUnit.GetComponent<AllyMove>().findingTarget)
        {
            unmovedAllyUnits -= 1; //Decrements the amount of allies that have not moved yet
            selectedUnit = null; //Unselects the current unit
            //If all ally units have moved, begin the enemy phase
            if (unmovedAllyUnits == 0)
                StartEnemyPhase();
        }
    }

    //Starts the enemy phase
    private void StartEnemyPhase()
    {
        playerPhase = false;
        List<GameObject> enemyUnitsTemp = new List<GameObject>();
        
        //Removes dead enemies from enemyUnits list
        foreach (GameObject enemyUnit in enemyUnits)
        {
            if (enemyUnit)
                enemyUnitsTemp.Add(enemyUnit);
        }
        enemyUnits = enemyUnitsTemp;

        //Sets all enemyUnits moved value to false
        foreach(GameObject enemyUnit in enemyUnits)
        {
            enemyUnit.GetComponent<EnemyMove>().finished = false;
        }
        activeEnemyUnits = enemyUnits.Count;
        selectedUnit = enemyUnits[activeEnemyUnits - 1];
        cursor.GetComponent<Cursor>().canMove = false;
    }

    //Starts the player phase
    private void StartPlayerPhase()
    {
        selectedUnit = null;
        unmovedAllyUnits = 0;
        playerPhase = true;
        turn++;
        List<GameObject> allyUnitsTemp = new List<GameObject>();

        foreach(GameObject allyUnit in allyUnits)
        {
            if(allyUnit)
            {
            unmovedAllyUnits += 1;
            allyUnit.GetComponent<AllyMove>().finished = false;
            allyUnit.GetComponent<SpriteRenderer>().color = Color.white;
            }
        }

        cursor.GetComponent<Cursor>().canMove = true;
        cursor.GetComponent<Cursor>().followTarget = null;
    }

    //Enemy Phase
    private void EnemyPhase()
    {
        //If the enemy has not moved and is not currently moving, they move (wow this sounds fucking stupid)

        if(selectedUnit.GetComponent<EnemyMove>().attacking)
        {

        }
        //Enemy attacking phase. Will attack a target if there is one avaliable
        else if(selectedUnit.GetComponent<EnemyMove>().findingTarget)
        {
            if(selectedUnit.GetComponent<EnemyMove>().closestTarget.transform.parent.GetComponent<Tile>().attackable)
            {
                Debug.Log("Enemy attacked");
                battleManager.attackingUnit = selectedUnit;
                battleManager.defendingUnit = selectedUnit.GetComponent<EnemyMove>().closestTarget;
                battleManager.Attack();
            }
        }
        else if (!selectedUnit.GetComponent<EnemyMove>().finished && !selectedUnit.GetComponent<EnemyMove>().moving)
            selectedUnit.GetComponent<EnemyMove>().Move();
        //If the enemy has moved
        else if(selectedUnit.GetComponent<EnemyMove>().finished)
        {
            enemyUnits[activeEnemyUnits - 1].GetComponent<EnemyMove>().RemoveSelectableTiles();
            Debug.Log(selectedUnit.transform.name + " has finished moving");
            activeEnemyUnits -= 1;
            
            //If the enemy unit dies during a counter attack, delete
            if (selectedUnit.GetComponent<Stats>().isDead)
                Destroy(selectedUnit);

            if(activeEnemyUnits != 0)
                selectedUnit = enemyUnits[activeEnemyUnits - 1];    
        }  

        //If all enemies have moved
        if(activeEnemyUnits == 0)
            StartPlayerPhase();
    }
}
