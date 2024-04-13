using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Singleton instance
    public static GameManager Instance { get; private set; }
    // Game state
    public enum GameState
    {
        Paused,
        Playing,
        GameOver
    }
    public GameState CurrentState { get; private set; }

    private void Awake()
    {
        // Ensure only one instance of GameManager exists
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public int pointsTotal;
    public int pointsRound;

    public List<GameObject> demons_cards;
    public List<DemonProfile> demons_profile;
    public List<DemonProfile> couples;
    public enum Sin { sloth, pride, wrath, greed, lust, envy, gluttony};
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void StartRound()
    {
        for(int i = 0; i < 8; i++)
        {

        }
    }
    public int CalculateMatch(DemonProfile demon1, DemonProfile demon2){

        double matchLevel = 0;

        for(int i = 0; i < 7; i++){
            matchLevel+=Math.Pow(Math.Pow((demon1.sins[i]-demon2.sins[i]),2),0.5f);
        }
        matchLevel = Math.Floor(100-matchLevel);
        return (int)matchLevel;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
