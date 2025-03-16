using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    private MeshRenderer mr;
    public Material defaultMaterial;
    public Material pressedMaterial;

    public KeyCode keyToPress;

    // Start is called before the first frame update
    void Start()
    {
        mr = GetComponent<MeshRenderer>();       
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(keyToPress))
        {
            mr.material = pressedMaterial;
        }

        if(Input.GetKeyUp(keyToPress))
        {
            mr.material = defaultMaterial;
        }
    }
}
