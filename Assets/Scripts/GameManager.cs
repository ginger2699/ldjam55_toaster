using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public int pointsTotal;
    public int pointsRound;

    public List<T> demons_profile;
    public List<T> couples;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void CalculateMatch(T demon1, T demon2){

        for(i = 0; i < 10; i++){
            pow((demon1.sin[i]-demon2.sin[i]),2);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
