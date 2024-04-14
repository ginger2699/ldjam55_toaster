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
        Vector2 position = Input.mousePosition;


        transform.position = position;
    }
}
