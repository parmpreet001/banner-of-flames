using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;

public class MainMenu : MonoBehaviour
{

    private GameObject cursor;
    private int cursorPosition = 1;

    private void Start()
    {
        cursor = GameObject.Find("Cursor");
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            if(cursorPosition <= 1)
                MoveCursorDown();
        }
        else if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            if(cursorPosition > 1)
                MoveCursorUp();
        }
        if(Input.GetKeyDown(KeyCode.Z))
        {
            if(cursorPosition == 1)
            {
                NewGame();
            }
            else if(cursorPosition == 2)
            {
                LoadGame();
            }
        }
    }

    private void MoveCursorDown()
    {
        cursorPosition++;
        cursor.transform.position = new Vector2(cursor.transform.position.x, cursor.transform.position.y - 70);
    }

    private void MoveCursorUp()
    {
        cursorPosition--;
        cursor.transform.position = new Vector2(cursor.transform.position.x, cursor.transform.position.y + 70);
    }

    private void NewGame()
    {
        if (File.Exists(Application.dataPath + "save.json"))
            File.Delete(Application.dataPath + "save.json");
        SceneManager.LoadScene("TutorialScene");
    }

    void LoadGame()
    {
        SaveData save = SaveSystem.LoadGame();
        if(save.stage == 1)
        {
            SceneManager.LoadScene("Level1");
        }
    }
}
