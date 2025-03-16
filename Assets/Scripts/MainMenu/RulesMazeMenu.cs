using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RulesMazeMenu : MonoBehaviour
{
    public GameObject controls;
    public GameObject rules_Maze;
    public GameObject rules_Scroll;
    
    public void MazeToScrollRules()
    {
        rules_Maze.SetActive(false);
        rules_Scroll.SetActive(true);
    }
    public void MazeToControls()
    {
        rules_Maze.SetActive(false);
        controls.SetActive(true);
    }
}
