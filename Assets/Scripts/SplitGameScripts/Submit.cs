using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Submit : MonoBehaviour
{
    // Start is called before the first frame update
    public void submitAnswer()
    {
        GameManager.instance.EndGame();
    }
}
