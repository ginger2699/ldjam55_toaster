using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ruler : MonoBehaviour
{
    public enum Sin { sloth, pride, wrath, greed, lust, envy, gluttony };

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
        double normalizedMatch = (double)match / 70 * 100;
        Debug.Log(normalizedMatch);

        return (int)normalizedMatch;


    }
}
