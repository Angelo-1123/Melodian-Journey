using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RulesSplitMenu : MonoBehaviour
{
    public GameObject controls;
    public GameObject rules_Maze;
    public GameObject rules_Scroll;
    
    public void ScrollToControls()
    {
        rules_Scroll.SetActive(false);
        controls.SetActive(true);
    }
    public void ScrollToMazeRules()
    {
        rules_Maze.SetActive(true);
        rules_Scroll.SetActive(false);
    }
}
