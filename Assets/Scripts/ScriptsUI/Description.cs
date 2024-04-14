using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Description : MonoBehaviour
{


    private void Awake()
    {
        
    }

    private void Update()
    {
        //to make the window appear next to the pointer
        Vector2 position = Input.mousePosition;
        transform.position = position;
    }
}
