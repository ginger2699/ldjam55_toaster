using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpAndRules : MonoBehaviour
{
    public GameObject helpPanel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Help()
    {
        // Show help
        helpPanel.SetActive(true);
    }
}
