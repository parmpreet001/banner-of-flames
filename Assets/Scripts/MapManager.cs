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
    [SerializeField]
    private GameObject selectedUnit; //The currently selected unit. Can be either a ally unit or an enemy unit. 

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
    }
    void Update()
    {
        if(playerPhase)
        {
            CheckMouse();
            CheckUnmovedUnits();
        }
        else
        {
            EnemyPhase();
        }
    }

    //Checks where the player clicked
    private void CheckMouse()
    {
        //If left mouse button is pressed
        if (Input.GetMouseButtonUp(0))
        {
            Collider2D selectedTile = Physics2D.OverlapBox((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition), new Vector2(0.01f, 0.01f), 0); //tile the player clicked on

            if (selectedUnit && selectedUnit.GetComponent<AllyMove>().attacking)
            {
                if(selectedTile.GetComponent<Tile>().attackable)
                {
                    Debug.Log("Attacked");
                    selectedUnit.GetComponent<AllyMove>().attacking = false;
                    selectedUnit.GetComponent<AllyMove>().RemoveSelectableTiles();
                }
            }
            //if a unit is already selected and the player clicks on a tile that is not selectable, unselects the unit
            else if (selectedUnit && (!selectedTile.GetComponent<Tile>().selectable || selectedTile.GetComponent<Tile>().current))
            {
                Debug.Log("Option 1");
                selectedUnit.GetComponent<AllyMove>().selected = false;
                selectedUnit.GetComponent<AllyMove>().RemoveSelectableTiles();
                selectedUnit = null;
            }

            //If the tile has a player unit that hasn't moved yet, selects that unit
            else if(selectedTile.transform.childCount == 1 && !selectedTile.transform.GetChild(0).GetComponent<AllyMove>().moved)
            {
                Debug.Log("Option 2");
                //If there is already a selected unit, then that unit becomes unselected
                if (selectedUnit)
                    selectedUnit.GetComponent<AllyMove>().selected = false;

                selectedUnit = selectedTile.transform.GetChild(0).gameObject; 
                selectedUnit.GetComponent<AllyMove>().selected = true; 
                Debug.Log("Selected " + selectedUnit.transform.name);
            }
        }
    }

    //Checks and updates the value of unmovedAllyUnits
    private void CheckUnmovedUnits()
    {
        //If the selected unit has moved and is not currently moving(meaning that it has reaching the end of its walk animation coroutine)
        if (selectedUnit && selectedUnit.GetComponent<AllyMove>().moved && !selectedUnit.GetComponent<AllyMove>().moving && !selectedUnit.GetComponent<AllyMove>().attacking)
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
        activeEnemyUnits = 0;
        playerPhase = false;
        foreach(GameObject enemyUnit in enemyUnits)
        {
            activeEnemyUnits++;
            enemyUnit.GetComponent<EnemyMove>().moved = false;
        }
        selectedUnit = enemyUnits[activeEnemyUnits - 1];
    }

    //Starts the player phase
    private void StartPlayerPhase()
    {
        selectedUnit = null;
        unmovedAllyUnits = 0;
        playerPhase = true;
        turn++;
        foreach(GameObject allyUnit in allyUnits)
        {
            unmovedAllyUnits += 1;
            allyUnit.GetComponent<AllyMove>().moved = false;
            allyUnit.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    //Enemy Phase
    private void EnemyPhase()
    {
        //If the enemy has not moved and is not currently moving, they move (wow this sounds fucking stupid)
        if(!selectedUnit.GetComponent<EnemyMove>().moved && !selectedUnit.GetComponent<EnemyMove>().moving)
            selectedUnit.GetComponent<EnemyMove>().Move();
        //Enemy attacking phase. Will attack a target if there is one avaliable
        else if(selectedUnit.GetComponent<EnemyMove>().attacking)
        {
            if(selectedUnit.GetComponent<EnemyMove>().closestTarget.transform.parent.GetComponent<Tile>().attackable)
            {
                Debug.Log("Enemy attacked");
                enemyUnits[activeEnemyUnits - 1].GetComponent<EnemyMove>().RemoveSelectableTiles();
                enemyUnits[activeEnemyUnits - 1].GetComponent<EnemyMove>().attacking = false;
            }
        }
        //If the enemy has moved
        else if(selectedUnit.GetComponent<EnemyMove>().moved)
        {
            enemyUnits[activeEnemyUnits - 1].GetComponent<EnemyMove>().RemoveSelectableTiles();
            Debug.Log("finished moving");
            activeEnemyUnits -= 1;
            if(activeEnemyUnits != 0)
                selectedUnit = enemyUnits[activeEnemyUnits - 1];
        }   
        //If all enemies have moved
        if(activeEnemyUnits == 0)
            StartPlayerPhase();
    }
}
