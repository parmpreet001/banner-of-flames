using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyMove :TileMove
{
    public bool firstClick = true;
    public bool attacked = false;
    [SerializeField]
    public Tile selectedTile;
    private Tile startingTile; //The tile the unit started on
    private Tile movedTile; //The tile the unit moved to

    private AllyStats _AllyStats; //AllyStats component of the unit this script is attached to
    
    // Start is called before the first frame update
    void Start()
    {
        _AllyStats = GetComponent<AllyStats>();
        Init();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void CheckInput()
    {
        
    }

    private void CheckCursor()
    {

    }

    public void UnselectUnit()
    {

    }
}
