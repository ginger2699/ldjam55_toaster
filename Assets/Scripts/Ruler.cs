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

        //public enum Sin { Pigrizia, Superbia, Ira, Avidità, Lussuria, Invidia, Gola };

        //First Rule
        // Rule matching gluttony
        match += 10 - Math.Abs(demon1[6] - demon2[6]);

        //Second Rule
        // Rule for Superbia e Invidia
        int sec_rule_1half = (Math.Abs(demon1[1] - demon2[5]) + 1) / 2;
        int sec_rule_2half = (Math.Abs(demon2[1] - demon1[5]) + 1) / 2;
        match += sec_rule_1half + sec_rule_2half;

        //Third Rule
        // Rule for Ira
        match += Math.Abs(demon1[2] - demon2[2]) + 1;

        //Fourth Rule
        // Rule for Lussuria
        match += 10 - Math.Abs(demon1[4] - demon2[4]);

        //fiveth Rule
        // Rule for Invidia
        match += 10 - Math.Abs(demon1[5] - demon2[5]);

        //sixth Rule
        // Rule for Superbia e Avidità
        int six_rule_1half = (Math.Abs(demon1[1] - demon2[3]) + 1) / 2;
        int six_rule_2half = (Math.Abs(demon2[1] - demon1[3]) + 1) / 2;
        match += six_rule_1half + six_rule_2half;

        //seventh Rule
        // Rule for Lussuria e Pigrizia
        int sev_rule_1half = (Math.Abs(demon1[0] - demon2[4]) + 1) / 2;
        int sev_rule_2half = (Math.Abs(demon2[0] - demon1[4]) + 1) / 2;
        match += sev_rule_1half + sev_rule_2half;


        // Normalize the match value to 100%
        double normalizedMatch = (double)match / 70 * 100;

        return (int)normalizedMatch;


    }
}
