using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DemonProfile
{
    public string d_name;
    public int age;
    public int[] sins = new int[7];
    public Image profile_img;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void GenerateSinsLevels(){
        int i = 0;
        foreach (int sin in sins)
        {
            i++;
            sins[sin] = UnityEngine.Random.Range(1, 11);
            Debug.Log("Sins number " + i + " value: " + sins[sin]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
