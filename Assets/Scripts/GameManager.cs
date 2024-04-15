using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System.IO;
using Unity.VisualScripting;

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
    public GameObject pointsPanel;
    //for sin levels
    public GameObject CubePrefab;
    public List<GameObject> demons_sins;
    //ui cards
    public List<GameObject> demons_cards;
    //profile of demons
    public List<DemonProfile> demons_profile;
    //public List<Couple> couples;
    public Sprite fullheart;
    public Sprite emptyheart;

    //next round button
    public GameObject nextRoundButton;
    //summon button
    public GameObject summonButton;

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
        //List<int> randomIndices = GenerateRandomIndices(0, demonNames.Count, 8);
        for(int i = 0; i < 8; i++)
        {
            DemonProfile newDemon = new DemonProfile();
            newDemon.Init();
            //newDemon.d_name = demonNames[randomIndices[i]];
            //newDemon.age = UnityEngine.Random.Range(20,7000);

            demons_profile.Add(newDemon); 

            Debug.Log("Name: " + newDemon.d_name);
            Debug.Log("Age: " + newDemon.age); 
 
            newDemon.GenerateSinsLevels();
            demons_cards[i].GetComponent<DemonCard>().d_profile = newDemon;
            demons_cards[i].transform.GetChild(2).gameObject.GetComponent<TMP_Text>().text = (newDemon.d_name); 
            demons_cards[i].transform.GetChild(3).gameObject.GetComponent<TMP_Text>().text = ("Age: " + newDemon.age);
            if (newDemon.isGreg)
                demons_cards[i].transform.GetChild(11).gameObject.GetComponent<GeneratePropicParts>().setGreg(true);
            else
                demons_cards[i].transform.GetChild(11).gameObject.GetComponent<GeneratePropicParts>().setGreg(false);
            for (int propicPart = 4; propicPart < 12; propicPart++)
                demons_cards[i].transform.GetChild(propicPart).gameObject.GetComponent<GeneratePropicParts>().GeneratePropics();
            
            //show graphic sins levels
            for (int j = 0; j < 7; j++)
            {
                int current_level = newDemon.sins[j];
                GameObject current_sin = demons_sins[i].transform.GetChild(j).gameObject;
                Color current_color = colors[j];
                for (int k = 1; k < current_level; k++)
                {
                    GameObject cube = Instantiate(CubePrefab, current_sin.transform);
                    cube.GetComponent<Image>().color = current_color;
                }
                //demons_sins[i].transform.GetChild(j).gameObject.GetComponent<TMP_Text>().text = newDemon.sins[j].ToString();
            }
        }

    }

    private void ClearSins()
    {
        for (int i = 0; i < 8; i++)
        {
            DemonProfile currentDemon = demons_profile[i];
            for (int j = 0; j < 7; j++)
            {
                int current_level = currentDemon.sins[j];
                GameObject current_sin = demons_sins[i].transform.GetChild(j).gameObject;
                Color current_color = colors[j];
                for (int k = 0; k < current_level - 1; k++)
                    Destroy(current_sin.transform.GetChild(k).gameObject);
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

    public IEnumerator CalculateMatchCoroutine()
    {
        pointsPanel.SetActive(true);
        pointsRound = 0;
        int numCouple = 0;
        List<DemonProfile> demons_selected = new List<DemonProfile>();
        summonButton.SetActive(false);
        nextRoundButton.SetActive(true);
        nextRoundButton.GetComponent<Button>().interactable = false;
        for(int i = 0;i< demons_cards.Count; i++)
        {
            DemonCard current_card = demons_cards[i].GetComponent<DemonCard>();
            if (demons_selected.Find(x => x == current_card.d_profile) == null)
            {
                numCouple++;
                demons_selected.Add(current_card.d_profile);
                demons_selected.Add(current_card.matchedDemon.GetComponent<DemonCard>().d_profile);
                int matchability = matchMaking(current_card.d_profile.sins.ToList(), demons_selected[demons_selected.Count-1].sins.ToList());
                Debug.Log("Matchability: " + matchability);
                GameObject resultCouplePanel = pointsPanel.transform.GetChild(numCouple - 1).gameObject;
                
                RecreatePropics(demons_cards[i], current_card.matchedDemon, resultCouplePanel.transform.GetChild(1).gameObject);
                resultCouplePanel.SetActive(true);
                float numOfHearts = matchability / 20;
                Debug.Log("Hearts: " + numOfHearts.ToString());
                for (int j = 0; j < numOfHearts; j++)
                {
                    resultCouplePanel.transform.GetChild(2).GetChild(j).gameObject.GetComponent<Image>().sprite = fullheart;
                    yield return new WaitForSeconds(0.5f); // Wait for 1.5 seconds

                }
                if (numOfHearts == 5)
                {
                    resultCouplePanel.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = "Perfect Match!";
                }
                else if (numOfHearts >= 3)
                {
                    resultCouplePanel.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = "Good Match";
                }
                else if (numOfHearts >= 2)
                {
                    resultCouplePanel.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = "Mmh...not quite";
                }
                else
                {
                    resultCouplePanel.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = "That's bad!";
                }
                //pointsPanel.transform.GetChild(numCouple-1).gameObject.GetComponent<TMP_Text>().text = (i+1).ToString()+ "° Couple: " + matchability.ToString() + "% match";
                pointsRound += matchability;
                yield return new WaitForSeconds(1f); // Wait for 1.5 seconds
            }
        }
        nextRoundButton.GetComponent<Button>().interactable = true;
        //pointsPanel.transform.GetChild(5).gameObject.SetActive(true);
        //pointsPanel.transform.GetChild(5).gameObject.GetComponent<TMP_Text>().text = "Final Score: " + pointsRound.ToString();
    }

    public void RecreatePropics(GameObject first_demon, GameObject second_demon, GameObject couplePics) 
    {
        for (int i=0; i< 8; i++)
        {
            couplePics.transform.GetChild(0).GetChild(i).gameObject.GetComponent<Image>().sprite = first_demon.transform.GetChild(i+4).GetComponent<Image>().sprite;
            couplePics.transform.GetChild(1).GetChild(i).gameObject.GetComponent<Image>().sprite = second_demon.transform.GetChild(i+4).GetComponent<Image>().sprite;
        }

    }
    public void StartSummonDate()
    {
        if (CanStartSummon())
        {
            Debug.Log("Starting summon date");
            StartCoroutine(CalculateMatchCoroutine());

        }
        else {
            Debug.Log("Not all demons are paired");
        }
    }
    public bool CanStartSummon()
    {
        return demons_cards.FindAll(x => x.GetComponent<DemonCard>().isPaired == false).Count == 0;
    }

    public int returnPointsHighAndHigh(int value1Demon, int value2Demon)
    {
       if (Math.Abs(value1Demon - value2Demon) < 3)
        {
            return 20;
        }
        else if (Math.Abs(value1Demon - value2Demon) > 8)
        {
            return -5;
        }
        else
        {
            return 10 - Math.Abs(value1Demon - value2Demon);
        }
    }
    public int returnPointsHighandLow(int value1Demon, int value2Demon)
    {
        if (Math.Abs(value1Demon - value2Demon) > 8)
        {
           return 20;
        }
        else if (Math.Abs(value1Demon - value2Demon) < 3)
        {
            return -5;
        }
        else
        {
            return Math.Abs(value1Demon - value2Demon) + 1;
        }
    }

    int matchMaking(List<int> demon1, List<int> demon2)
    {
        int match = 0;
        Debug.Log("Inizio match:");

        //public enum Sin { wrath, gluttony, greed, pride, lust, envy, sloth};
        //First Rule
        // Rule matching envy: high envy goes with high envy (same with low)
        match += returnPointsHighAndHigh(demon1[5], demon2[5]);
        Debug.Log(match);

        //Second Rule
        // Rule for wrath e pride: high wrath goes with high pride and high pride goes with high wrath
        int sec_rule_1half = returnPointsHighAndHigh(demon1[0], demon2[3]) / 2;
        int sec_rule_2half = returnPointsHighAndHigh(demon2[0], demon1[3]) / 2;
        match += sec_rule_1half + sec_rule_2half;
        Debug.Log(match);

        //third Rule
        // Rule for gluttony e greed: high gluttony goes with low greed and high greed goes with low gluttony
        int third_rule_1half = returnPointsHighandLow(demon1[1], demon2[2]) / 2;
        int third_rule_2half = returnPointsHighandLow(demon2[1], demon1[2]) / 2;
        match += third_rule_1half + third_rule_2half;
        Debug.Log(match);

        //Fourth Rule
        // Rule for sloth e lust: high sloth goes with low lust and high lust goes with low sloth
        int fourth_rule_1half = returnPointsHighandLow(demon1[6], demon2[4]) / 2;
        int fourth_rule_2half = returnPointsHighandLow(demon2[6], demon1[4]) / 2;
        match += fourth_rule_1half + fourth_rule_2half;
        Debug.Log(match);


        // Normalize the match value to 100%
        double normalizedMatch = (match * 100) / 40;
        Debug.Log(normalizedMatch);

        if (normalizedMatch > 100)
        {
            return 100;
        }

        return (int)normalizedMatch;

    }
    int matchMaking1(List<int> demon1, List<int> demon2)
    {
        int match = 0;
        Debug.Log("Inizio match:");
        

        //First Rule
        // Rule matching gluttony: high gluttony goes with high gluttony (same with low)
        if (Math.Abs(demon1[1] - demon2[1])<3)
        {
            match += 20;
        }
        else if (Math.Abs(demon1[1] - demon2[1]) > 8)
        {
            match -= 5;
        }
        else
        {
            match += 10 - Math.Abs(demon1[1] - demon2[1]);
        }
        Debug.Log(match);

        //Second Rule
        // Rule for Superbia: high pride goes with low pride and viceversa 
        if (Math.Abs(demon1[1] - demon2[1]) > 8 )
        {
            match += 20;
        }
        else if (Math.Abs(demon1[1] - demon2[1]) < 3)
        {
            match -= 5;
        }
        else
        {
            match += Math.Abs(demon1[1] - demon2[1]);
        }
        Debug.Log(match);

        //public enum Sin { wrath, gluttony, greed, pride, lust, envy, sloth};

        //Third Rule
        // Rule for Wrath: high wrath goes with low wrath and viceversa
        //match += Math.Abs(demon1[0] - demon2[0]) + 1;
        //Debug.Log(match);

        //Fourth Rule
        // Rule for Lussuria: high lust goes with high lust (same with low)
        if (Math.Abs(demon1[4] - demon2[4]) < 3)
        {
            match += 20;
        }
        else if (Math.Abs(demon1[4] - demon2[4]) > 8)
        {
            match -= 5;
        }
        else
        {
            match += 10 -Math.Abs(demon1[4] - demon2[4]);
        }
        Debug.Log(match);

        //Fifth Rule
        // Rule for Invidia: high envy goes with high envy (same with low)
        //match += 10 - Math.Abs(demon1[5] - demon2[5]);
        //Debug.Log(match);

        //sixth Rule
        // Rule for Invidia e Avidit�: high greed goes with low envy and high envy goes with low greed
        //int six_rule_1half = (Math.Abs(demon1[5] - demon2[2]) + 1) / 2;
        //int six_rule_2half = (Math.Abs(demon2[5] - demon1[2]) + 1) / 2;
        //match += six_rule_1half + six_rule_2half;
        //Debug.Log(match);

        //seventh Rule
        // Rule for Lussuria e Pigrizia: high lust goes with low sloth and high sloth goes with low lust
        int sev_rule_1half = (Math.Abs(demon1[6] - demon2[4]) + 1) / 2;
        int sev_rule_2half = (Math.Abs(demon2[6] - demon1[4]) + 1) / 2;
        match += sev_rule_1half + sev_rule_2half;
        Debug.Log(match);


        // Normalize the match value to 100%
        double normalizedMatch = (match * 100)/40;
        Debug.Log(normalizedMatch);

        return (int)normalizedMatch;

    }


    // Update is called once per frame
    public void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            ClearSins();
            demons_profile.Clear();
            StartRound();
        }*/
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Quit the application
            Application.Quit();
        }
    }

    public void NextRound()
    {
        ClearSins();
        ClearResultScreen();
        ClearCards();
        demons_profile.Clear();
        nextRoundButton.SetActive(false);
        summonButton.SetActive(true);
        StartRound();
    }

    public void ClearResultScreen()
    {
        pointsPanel.SetActive(false);
        for (int i = 0; i < 4; i++)
        {
            pointsPanel.transform.GetChild(i).gameObject.SetActive(false);
            pointsPanel.transform.GetChild(i).GetChild(0).gameObject.GetComponent<TMP_Text>().text = "Matchy Match";
            for (int j = 0; j < 5; j++)
            {
                pointsPanel.transform.GetChild(i).GetChild(2).GetChild(j).gameObject.GetComponent<Image>().sprite = emptyheart;
            }
        }
    }
    public void ClearCards()
    {
        for (int i = 0; i < 8; i++)
        {
            DemonCard currentCard = demons_cards[i].GetComponent<DemonCard>();
            currentCard.isPaired = false;
            currentCard.isSelected = false;
            currentCard.matchedDemon = null;
            currentCard.transform.GetChild(0).gameObject.GetComponent<Image>().color = Color.white;
            //currentCard.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = currentCard.cardBack;
        }
        demons_cards[0].GetComponent<DemonCard>().ResetNextRound();
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
    //List<string> demonNames = new List<string>
    //{
    //    "Azazel",
    //    "Belial",
    //    "Abaddon",
    //    "Mephistopheles",
    //    "Lucifer",
    //    "Beelzebub",
    //    "Leviathan",
    //    "Lilith",
    //    "Asmodeus",
    //    "Belphegor",
    //    "Mammon",
    //    "Moloch",
    //    "Nybbas",
    //    "Astaroth",
    //    "Behemoth",
    //    "Beherit",
    //    "Raum",
    //    "Baal",
    //    "Marchosias",
    //    "Barbatos",
    //    "Gremory",
    //    "Berith",
    //    "Malphas",
    //    "Marbas",
    //    "Shax",
    //    "Orobas",
    //    "Agares",
    //    "Buer",
    //    "Sitri",
    //    "Vassago",
    //    "Zepar",
    //    "Dantalion",
    //    "Furfur",
    //    "Vine",
    //    "Gaap",
    //    "Haures",
    //    "Paimon",
    //    "Sallos",
    //    "Vepar",
    //    "Ronove",
    //    "Glasya-Labolas",
    //    "Crocell",
    //    "Furcas",
    //    "Ipos",
    //    "Sabnock",
    //    "Andras",
    //    "Forneus",
    //    "Shax",
    //    "Vapula",
    //    "Zagan"
    //};
}
