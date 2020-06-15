using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static int turn = 1; //What turn it is. Currently static but might change later
    private bool playerPhase = true; //True if player phase, false if enemy phase
    private int unmovedAllyUnits; //During the player's turn, the number of ally units that have not moved yet
    private int activeEnemyUnits; //Total number of active(aka not dead enemy units) on the map
    private List<GameObject> allyUnits = new List<GameObject>(); //List of all alive ally units on the map
    private List<GameObject> enemyUnits = new List<GameObject>(); //List of all alive enemy units on the map
    public GameObject selectedUnit { get; private set; } //The currently selected unit. Can be either a ally unit or an enemy unit.
    private AllyStats selectedUnit_AllyStats; //AllyStats component of the selected unit
    private BattleManager battleManager; //BattleManager script
    private Tile selectedTile;
    private Cursor cursor;
    [SerializeField]
    private UnitStates unitState;
    private TileController tileController;
    [SerializeField]
    private Tile startingTile = null;


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

        cursor = GameObject.Find("Cursor").GetComponent<Cursor>();
        battleManager = GetComponent<BattleManager>();
        tileController = GetComponent<TileController>();
        tileController.Init();
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

    private void PlayerPhase()
    {
        CheckCursor();
        switch(unitState)
        {
            //If a unit has not been selected
            case UnitStates.UNSELECTED:
            {
                //If Z was pressed
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    //If the tile has an ally unit that has not yet moved
                    if (cursor.CurrentTileHasAllyUnit() && !cursor.GetCurrentUnit().GetComponent<AllyMove>().finished)
                    {
                        startingTile = cursor.currentTile;
                        selectedUnit = cursor.GetCurrentUnit();
                        selectedUnit_AllyStats = selectedUnit.GetComponent<AllyStats>();
                        unitState = UnitStates.SELECTED;
                        tileController.SetCurrentTile(selectedUnit);
                        tileController.FindSelectableTiles(selectedUnit_AllyStats.classType.mov, selectedUnit_AllyStats.classType.walkableTerrain, true);
                    }
                }
                break;
            }
            //If a unit is currently selected
            case UnitStates.SELECTED:
            {
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    if (cursor.currentTile.selectable || cursor.currentTile == startingTile)
                        StartCoroutine(MoveToTile(selectedUnit, cursor.currentTile));
                }
                else if (Input.GetKeyDown(KeyCode.X))
                {
                    unitState = UnitStates.UNSELECTED;
                    selectedUnit = null;
                    selectedUnit_AllyStats = null;
                    startingTile = null;
                    tileController.RemoveSelectableTiles();
                }
                break;
            }
            //If a unit has moved to a tile
            case UnitStates.MOVED:
            {
                if(Input.GetKeyDown(KeyCode.X))
                {
                    tileController.SetUnitToTile(selectedUnit, startingTile);
                    unitState = UnitStates.UNSELECTED;
                    selectedUnit = null;
                    selectedUnit_AllyStats = null;
                    startingTile = null;
                    tileController.RemoveSelectableTiles();
                }
                if(selectedUnit.GetComponent<AllyMove>().finished)
                {
                    selectedUnit.GetComponent<SpriteRenderer>().color = Color.gray;
                    unitState = UnitStates.UNSELECTED;
                }
                break;
            }
            //If the player is navigating through one of the action menus(attack, items, magic, etc)
            case UnitStates.ACTION_MENU:
            {
                if(Input.GetKeyDown(KeyCode.X))
                {
                    unitState = UnitStates.MOVED;
                }
                break;
            }
            default:
                break;
        }
    }
    IEnumerator MoveToTile(GameObject unit, Tile tile)
    {
        unitState = UnitStates.MOVING;
        yield return tileController.MoveToTile(unit, tile);
        unitState = UnitStates.MOVED;
        yield return null;
    }

    //Sets the unit state to ActionMenu
    public void SetUnitStateActionMenu()
    {
        unitState = UnitStates.ACTION_MENU;
    }

    //Checks unitState and sets behavior of the cursor
    private void CheckCursor()
    {
        switch(unitState)
        {
            case UnitStates.MOVING:
            {
                cursor.followTarget = selectedUnit.transform;
                break;
            }
            case UnitStates.MOVED:
            {
                cursor.followTarget = selectedUnit.transform;
                break;
            }
            case UnitStates.ACTION_MENU:
            {
                cursor.followTarget = selectedUnit.transform;
                break;
            }
            default:
            {
                cursor.followTarget = null;
                cursor.canMove = true;
                break;
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
        cursor.canMove = false;
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
            //allyUnit.GetComponent<AllyMove>().finished = false;
            allyUnit.GetComponent<SpriteRenderer>().color = Color.white;
            }
        }

        cursor.canMove = true;
        cursor.followTarget = null;
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

    //Returns true if unitState is equal to any of the values in states
    public bool CheckUnitStates(params UnitStates[] states)
    {
        foreach (UnitStates state in states)
        {
            if(unitState == state)
                return true;
        }
         return false;
    }
}
