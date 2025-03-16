using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject controls;
    public GameObject rules_Maze;
    public GameObject rules_Scroll;

    public void ControlsToScrollRules()
    {
        rules_Scroll.SetActive(true);
        controls.SetActive(false);
    }
    public void ControlsToMazeRules()
    {
        rules_Maze.SetActive(true);
        controls.SetActive(false);
    }

    public void ControlsToMainMenu()
    {
        mainMenu.SetActive(true);
        controls.SetActive(false);
    }

}
