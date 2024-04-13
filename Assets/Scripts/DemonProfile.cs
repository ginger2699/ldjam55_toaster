using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonProfile : MonoBehaviour
{
    public string d_name;
    public int age;
    public int[] sins = new int[7];

    // Start is called before the first frame update
    void Start()
    {
        foreach (int sin in sins)
        {
            sins[sin] = Random.Range(1, 11);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
