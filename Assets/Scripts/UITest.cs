using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITest : MonoBehaviour
{
    public GameObject target;
    private GameObject actionMenu;
    private bool buttonsCreated = false;

    // Start is called before the first frame update
    void Start()
    {
        actionMenu = transform.Find("ActionMenu").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(target)
        {
            if(target.GetComponent<AllyMove>().actionMenu)
            {
                if(!buttonsCreated)
                {
                    actionMenu.SetActive(true);
                    actionMenu.transform.GetChild(0).gameObject.GetComponent<Button>().onClick.AddListener(OnClick);
                    buttonsCreated = true;
                }
                else
                {
                    transform.position = new Vector2(target.transform.position.x + 1f,target.transform.position.y);
                }
            }
        }
    }

    private void OnClick()
    {
        Debug.Log("d");
    }
}
