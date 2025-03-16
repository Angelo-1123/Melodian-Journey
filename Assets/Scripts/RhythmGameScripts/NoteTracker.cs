using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteTracker : MonoBehaviour
{
    public bool canBePressed;

    public KeyCode keyToPress;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(keyToPress))
        {
            if(canBePressed)
            {
                gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Activator")
        {
            canBePressed = true;
        }
    }

        private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Activator")
        {
            canBePressed = false;
        }
    }
}
