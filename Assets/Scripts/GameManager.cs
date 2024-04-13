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
    public List<Couple> couples;
    public enum Sin { sloth, pride, wrath, greed, lust, envy, gluttony};
    // Start is called before the first frame update
    void Start()
    {
        demons_profile = new List<DemonProfile>();
        StartRound();
    }
    public void StartRound()
    {
        List<int> randomIndices = GenerateRandomIndices(0, demonNames.Count, 8);
        for(int i = 0; i < 8; i++)
        {
            DemonProfile newDemon = new DemonProfile();
            newDemon.d_name = demonNames[randomIndices[i]];
            newDemon.age = UnityEngine.Random.Range(20,8000);

            demons_profile.Add(newDemon); 

            Debug.Log("Name: " + newDemon.d_name);
            Debug.Log("Age: " + newDemon.age); 
 
            newDemon.GenerateSinsLevels();
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

    // Function to generate random indices within a range
    static List<int> GenerateRandomIndices(int min, int max, int count)
    {
        List<int> indices = new List<int>();
        System.Random rand = new System.Random();
        while (indices.Count < count)
        {
            int index = rand.Next(min, max);
            if (!indices.Contains(index))
            {
                indices.Add(index);
            }
        }
        return indices;
    }
    List<string> demonNames = new List<string>
    {
        "Azazel",
        "Belial",
        "Abaddon",
        "Mephistopheles",
        "Lucifer",
        "Beelzebub",
        "Leviathan",
        "Lilith",
        "Asmodeus",
        "Belphegor",
        "Mammon",
        "Moloch",
        "Nybbas",
        "Astaroth",
        "Behemoth",
        "Beherit",
        "Raum",
        "Baal",
        "Marchosias",
        "Barbatos",
        "Gremory",
        "Berith",
        "Malphas",
        "Marbas",
        "Shax",
        "Orobas",
        "Agares",
        "Buer",
        "Sitri",
        "Vassago",
        "Zepar",
        "Dantalion",
        "Furfur",
        "Vine",
        "Gaap",
        "Haures",
        "Paimon",
        "Sallos",
        "Vepar",
        "Ronove",
        "Glasya-Labolas",
        "Crocell",
        "Furcas",
        "Ipos",
        "Sabnock",
        "Andras",
        "Forneus",
        "Shax",
        "Vapula",
        "Zagan"
    };
}
