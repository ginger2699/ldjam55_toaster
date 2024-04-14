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
            sins[i] = UnityEngine.Random.Range(1, 11);
            i++;
            
        }
        Debug.Log("Sins: " + sins[0].ToString() + ", " + sins[1].ToString() + ", " + sins[2].ToString() + ", " + sins[3].ToString() + ", " + sins[4].ToString() + ", " + sins[5].ToString() + ", " + sins[6].ToString());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
