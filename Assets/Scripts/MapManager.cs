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
    public GameObject selectedUnit; //The currently selected unit. Can be either a ally unit or an enemy unit.
    private AllyStats selectedUnit_AllyStats; //AllyStats component of the selected unit
    private BattleManager battleManager; //BattleManager script
    private Tile selectedTile;
    private Cursor cursor;
    [SerializeField]
    public UnitStates unitState;
    private TileController tileController;
    [SerializeField]
    private Tile startingTile = null;

    //Enemy ai
    GameObject closestTarget;
    bool targetInRange;

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
        CheckCursor();
    }

    private void PlayerPhase()
    {
        
        switch(unitState)
        {
            //If a unit has not been selected
            case UnitStates.UNSELECTED:
            {
                //If Z was pressed
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    //If the tile has an ally unit that has not yet moved
                    if (cursor.CurrentTileHasAllyUnit() && !cursor.GetCurrentUnit().GetComponent<Stats>().finishedTurn)
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
                if (cursor.CurrentTileHasEnemyUnit())
                    UpdateBattleManagerStats(selectedUnit, cursor.GetCurrentUnit());
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
                if (selectedUnit.GetComponent<Stats>().finishedTurn)
                {
                    EndUnitTurn();
                }
                if(Input.GetKeyDown(KeyCode.X))
                {
                    tileController.SetUnitToTile(selectedUnit, startingTile);
                    unitState = UnitStates.UNSELECTED;
                    selectedUnit = null;
                    selectedUnit_AllyStats = null;
                    startingTile = null;
                    tileController.RemoveSelectableTiles();
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
            case UnitStates.FINDING_TARGET:
            {
                if(cursor.CurrentTileHasEnemyUnit())
                {
                    tileController.RemoveSelectableTiles();
                    tileController.SetCurrentTile(selectedUnit);
                    tileController.ShowWeaponRange(selectedUnit_AllyStats.GetMinRange(), selectedUnit_AllyStats.GetMaxRange());
                    UpdateBattleManagerStats(selectedUnit, cursor.GetCurrentUnit());
                }
                else
                {
                    tileController.RemoveSelectableTiles();
                }
                if(Input.GetKeyDown(KeyCode.X))
                {
                    tileController.RemoveSelectableTiles();
                    unitState = UnitStates.MOVED;
                }    
                if(Input.GetKeyDown(KeyCode.Z))
                {
                    if(cursor.currentTile.attackable)
                    {
                        StartCoroutine(Attack());
                    }
                }
                break;
            }
            case UnitStates.FINDING_ALLY:
            {
                if(cursor.CurrentTileHasAllyUnit())
                {
                    UpdateBattleManagerStats(selectedUnit, cursor.GetCurrentUnit());
                }
                if(Input.GetKeyDown(KeyCode.X))
                {
                    tileController.RemoveSelectableTiles();
                    unitState = UnitStates.MOVED;
                }
                
                if(Input.GetKeyDown(KeyCode.Z))
                {
                    if(cursor.currentTile.healable)
                    {
                        StartCoroutine(Heal());
                    }
                }
               
                break;
            }
            case UnitStates.HEALING:
            {
                cursor.canMove = false;
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

    IEnumerator Attack()
    {
        bool usingThunderMagic = false;
        if (battleManager.activeUnit.GetComponent<Stats>().attackMethod == AttackMethod.OFFENSIVE_MAGIC &&
            battleManager.activeUnit.GetComponent<Stats>().equippedBlackMagic.name == "Thunder" &&
            battleManager.receivingUnit.GetComponentInParent<Tile>().terrainType == TerrainType.WATER)
        {
            usingThunderMagic = true;
        }

            unitState = UnitStates.ATTACKING;

        if(usingThunderMagic)
        {
            tileController.RemoveSelectableTiles();
            tileController.GetAdjacenTileTerrainType(battleManager.receivingUnit.GetComponentInParent<Tile>());
            foreach(Tile tile in tileController.selectableTiles)
            {
                if(tile.HasUnit() && tile.GetUnit() != battleManager.receivingUnit)
                {
                    Debug.Log("Added " + tile.GetUnit().name);
                    battleManager.additionalUnits.Add(tile.GetUnit());
                }
            }
        }
        yield return battleManager.AttackProcess();

        if(usingThunderMagic)
        {
            yield return battleManager.LightningDamage(cursor);
        }

        tileController.RemoveSelectableTiles();
        selectedUnit.GetComponent<Stats>().finishedTurn = true;
        EndUnitTurn();
        yield return null;
    }

    IEnumerator Heal()
    {
        unitState = UnitStates.HEALING;
        yield return battleManager.HealProcess();
        tileController.RemoveSelectableTiles();
        selectedUnit.GetComponent<Stats>().finishedTurn = true;
        EndUnitTurn();
        yield return null;
    }

    private void UpdateBattleManagerStats(GameObject attackingUnit, GameObject defendingUnit)
    {
        battleManager.activeUnit = attackingUnit;
        battleManager.receivingUnit = defendingUnit;
        battleManager.UpdateStats();
    }


    //Checks unitState and sets behavior of the cursor
    private void CheckCursor()
    {
        switch(unitState)
        {
            case UnitStates.SELECTED:
            {
                if (selectedUnit.tag == "EnemyUnit")
                    cursor.followTarget = selectedUnit.transform;
                break;
            }
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
            case UnitStates.ATTACKING:
            {
                cursor.canMove = false;
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
            enemyUnit.GetComponent<Stats>().finishedTurn = false;
        }
        unitState = UnitStates.SELECTED;
        activeEnemyUnits = enemyUnits.Count;
        selectedUnit = enemyUnits[activeEnemyUnits - 1];
        //cursor.canMove = false;
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
            allyUnit.GetComponent<Stats>().finishedTurn = false;
            allyUnit.GetComponent<SpriteRenderer>().color = Color.white;
            }
        }

        foreach(GameObject enemyUnit in enemyUnits)
        {
            if(enemyUnit)
            {
                enemyUnit.GetComponent<SpriteRenderer>().color = new Color32(200, 110, 120, 255);
            }
        }

        //cursor.canMove = true;
        //cursor.followTarget = null;
    }

    //Enemy Phase
    private void EnemyPhase()
    {
        switch (unitState)
        {
            case UnitStates.UNSELECTED:
            {
                if(activeEnemyUnits > 0)
                {
                    selectedUnit = enemyUnits[activeEnemyUnits - 1];
                    unitState = UnitStates.SELECTED;
                }
                else
                {
                    StartPlayerPhase();
                }
                        
                break;
            }
            case UnitStates.SELECTED:
            {
                closestTarget = tileController.GetClosestTarget(selectedUnit);
                Tile closestTile = tileController.GetClosestTileToTarget(selectedUnit, closestTarget);
                StartCoroutine(MoveToTile(selectedUnit, closestTile));
                unitState = UnitStates.MOVING;
                break;
            }
            case UnitStates.MOVED:
            {
                tileController.SetCurrentTile(selectedUnit);
                targetInRange = tileController.AllyInRange(selectedUnit.GetComponent<EnemyStats>().GetMinRange(),
                    selectedUnit.GetComponent<EnemyStats>().GetMaxRange());

                if(!targetInRange)
                {
                    tileController.RemoveSelectableTiles();
                    EndUnitTurn();
                }
                else
                {
                    unitState = UnitStates.FINDING_TARGET;
                }
                break;
            }
            case UnitStates.FINDING_TARGET:
            {
                if(targetInRange)
                {
                        battleManager.activeUnit = selectedUnit;
                        battleManager.receivingUnit = closestTarget;
                        StartCoroutine(Attack());
                }
                break;
            }
            default:
            {
                break;
            }
        }
    }

    private void EndUnitTurn()
    {
        if (selectedUnit.tag == "PlayerUnit")
            unmovedAllyUnits--;
        else
            activeEnemyUnits--;
        selectedUnit.GetComponent<SpriteRenderer>().color = Color.gray;
        selectedUnit = null;
        tileController.RemoveSelectableTiles();
        unitState = UnitStates.UNSELECTED;
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
