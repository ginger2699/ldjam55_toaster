using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using UnityEngine.UI;

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
    //for sin levels
    public GameObject CubePrefab;
    public List<GameObject> demons_sins;
    //ui cards
    public List<GameObject> demons_cards;
    //profile of demons
    public List<DemonProfile> demons_profile;
    //public List<Couple> couples;

    public List<Color> colors = new List<Color>();
    public enum Sin { wrath, gluttony, greed, pride, lust, envy, sloth};
    // Start is called before the first frame update
    void Start()
    {
        //insert color
        // Add the colors to the list
        colors.Add(new Color(189f / 255f, 15f / 255f, 15f / 255f)); // WRATH
        colors.Add(new Color(220f / 255f, 94f / 255f, 24f / 255f)); // GLUT
        colors.Add(new Color(219f / 255f, 188f / 255f, 24f / 255f)); // GREED
        colors.Add(new Color(145f / 255f, 63f / 255f, 196f / 255f)); // PRIDE
        colors.Add(new Color(219f / 255f, 43f / 255f, 127f / 255f)); // LUST
        colors.Add(new Color(20f / 255f, 197f / 255f, 80f / 255f)); // ENVY
        colors.Add(new Color(16f / 255f, 131f / 255f, 180f / 255f)); // SLOTH

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
            newDemon.age = UnityEngine.Random.Range(20,7000);

            demons_profile.Add(newDemon); 

            Debug.Log("Name: " + newDemon.d_name);
            Debug.Log("Age: " + newDemon.age); 
 
            newDemon.GenerateSinsLevels();
            demons_cards[i].GetComponent<DemonCard>().d_profile = newDemon;
            demons_cards[i].transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = (newDemon.d_name); 
            demons_cards[i].transform.GetChild(2).gameObject.GetComponent<TMP_Text>().text = ("Age: " + newDemon.age.ToString());
            
            //show graphic sins levels
            for (int j = 0; j < 7; j++)
            {
                int current_level = newDemon.sins[j];
                GameObject current_sin = demons_sins[i].transform.GetChild(j).gameObject;
                Color current_color = colors[j];
                for (int k = 1; k < current_level; k++)
                {
                    GameObject cube = Instantiate(CubePrefab, new Vector3(0, 0, 0), Quaternion.identity, current_sin.transform);
                    cube.GetComponent<Image>().color = current_color;
                }
                //demons_sins[i].transform.GetChild(j).gameObject.GetComponent<TMP_Text>().text = newDemon.sins[j].ToString();
            }
        }

    }
    public int CalculateMatchOld(DemonProfile demon1, DemonProfile demon2){

        double matchLevel = 0;

        for(int i = 0; i < 7; i++){
            matchLevel+=Math.Pow(Math.Pow((demon1.sins[i]-demon2.sins[i]),2),0.5f);
        }
        matchLevel = Math.Floor(100-matchLevel);
        return (int)matchLevel;
    }

    public float CalculateMatch()
    {
        pointsRound = 0;
        List<DemonProfile> demons_selected = new List<DemonProfile>();
        for(int i = 0;i< demons_cards.Count; i++)
        {
            DemonCard current_card = demons_cards[i].GetComponent<DemonCard>();
            if (demons_selected.Find(x => x == current_card.d_profile) == null)
            {
                demons_selected.Add(current_card.d_profile);
                demons_selected.Add(current_card.matchedDemon.GetComponent<DemonCard>().d_profile);
                int matchability = matchMaking(current_card.d_profile.sins.ToList(), demons_selected[demons_selected.Count-1].sins.ToList());
                Debug.Log("Matchability: " + matchability);
                pointsRound += matchability;
            }
        }

        return 2f;
    }
    public void StartSummonDate()
    {
        if (CanStartSummon())
        {
            Debug.Log("Starting summon date");
            CalculateMatch();
        }
        else {
            Debug.Log("Not all demons are paired");
        }
    }
    public bool CanStartSummon()
    {
        return demons_cards.FindAll(x => x.GetComponent<DemonCard>().isPaired == false).Count == 0;
    }
    int matchMaking(List<int> demon1, List<int> demon2)
    {
        int match = 0;
        Debug.Log("Inizio match:");
        //public enum Sin { Pigrizia, Superbia, Ira, Avidit�, Lussuria, Invidia, Gola };

        //First Rule
        // Rule matching gluttony: high gluttony goes with high gluttony (same with low)
        match += 10 - Math.Abs(demon1[6] - demon2[6]);
        Debug.Log(match);

        //Second Rule
        // Rule for Superbia: high pride goes with low pride and viceversa 
        match += Math.Abs(demon1[1] - demon2[1]) + 1;
        Debug.Log(match);

        //Third Rule
        // Rule for Wrath: high wrath goes with low wrath and viceversa
        match += Math.Abs(demon1[2] - demon2[2]) + 1;
        Debug.Log(match);

        //Fourth Rule
        // Rule for Lussuria: high lust goes with high lust (same with low)
        match += 10 - Math.Abs(demon1[4] - demon2[4]);
        Debug.Log(match);

        //Fifth Rule
        // Rule for Invidia: high envy goes with high envy (same with low)
        match += 10 - Math.Abs(demon1[5] - demon2[5]);
        Debug.Log(match);

        //sixth Rule
        // Rule for Invidia e Avidit�: high greed goes with low envy and high envy goes with low greed
        int six_rule_1half = (Math.Abs(demon1[5] - demon2[3]) + 1) / 2;
        int six_rule_2half = (Math.Abs(demon2[5] - demon1[3]) + 1) / 2;
        match += six_rule_1half + six_rule_2half;
        Debug.Log(match);

        //seventh Rule
        // Rule for Lussuria e Pigrizia: high lust goes with low sloth and high sloth goes with low lust
        int sev_rule_1half = (Math.Abs(demon1[0] - demon2[4]) + 1) / 2;
        int sev_rule_2half = (Math.Abs(demon2[0] - demon1[4]) + 1) / 2;
        match += sev_rule_1half + sev_rule_2half;
        Debug.Log(match);


        // Normalize the match value to 100%
        double normalizedMatch = (match * 100)/70;
        Debug.Log(normalizedMatch);

        return (int)normalizedMatch;

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
