using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITest : MonoBehaviour
{
    public GameObject selectedAllyUnit;
    private GameObject actionMenu;
    private bool buttonsCreated = false;
    List<string> buttons = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        actionMenu = transform.Find("ActionMenu").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(selectedAllyUnit)
        {
            if(selectedAllyUnit.GetComponent<AllyMove>().actionMenu)
            {
                if(!buttonsCreated)
                {
                    CreateButtons();
                    
                    //actionMenu.transform.GetChild(0).gameObject.GetComponent<Button>().onClick.AddListener(OnClick);
                    
                }
                else
                {
                    //transform.position = new Vector2(target.transform.position.x + 1f,target.transform.position.y);
                }
            }
        }
    }

    private void CreateButtons()
    {
        actionMenu.SetActive(true);
        buttonsCreated = true;


        AllyMove am = selectedAllyUnit.GetComponent<AllyMove>();

        if (am.attacking)
            buttons.Add("Attack");
        buttons.Add("Wait");

        for(int i = 0; i < buttons.Count; i++)
        {
            actionMenu.transform.GetChild(i).GetChild(0).GetComponent<Text>().text = buttons[i];
            actionMenu.transform.GetChild(i).GetChild(0).gameObject.SetActive(true);
            actionMenu.transform.GetChild(i).gameObject.SetActive(true);
            actionMenu.transform.GetChild(i).GetComponent<Button>().Select();
            if(buttons[i] == "Wait")
            {
                actionMenu.transform.GetChild(i).gameObject.GetComponent<Button>().onClick.AddListener(Wait);
            }
        }
        for(int i = 0; i < buttons.Count; i++)
        {

        }
    }
    private void Wait()
    {
        selectedAllyUnit.GetComponent<AllyMove>().UnselectUnit();
        selectedAllyUnit.GetComponent<AllyMove>().moved = true;
    }
}
