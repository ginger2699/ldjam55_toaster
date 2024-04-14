using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class DemonProfile
{
    public string d_name;
    public string age;
    public int[] sins = new int[7];
    public Image profile_img;
    public bool isGreg = false;

    private string[] names = new string[]{
        "Azazel",
        "Belial",
        "Abaddon",
        "Mephist",
        "Lucifer",
        "Beelzeb",
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
        "Makosias",
        "Barbatos",
        "Gremory",
        "Berith",
        "Malphas",
    };
    private string[] nicknames = new string[]{
        "Uth'",
        "Mhir'",
        "Kha ",
        "Rath ",
        "Moth ",
        "Din ",
        "Hutz ",
        "Khaal ",
        "Daz'",
        "Khos ",
        "Gruul ",
        "Daas ",
        "Muk ",
        "Seth ",
        "Tal'",
        "Lohk "
    };
    private string[] surnames = new string[]{
        "Marbas",
        "Shax",
        "Orobas",
        "Agares",
        "Buer",
        "Sitri",
        "Vassago",
        "Zepar",
        "Dantal",
        "Furfur",
        "Vine",
        "Gaap",
        "Haures",
        "Pamon",
        "Sallos",
        "Vepar",
        "Ronove",
        "Glasya",
        "Labolas",
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

    public void Init()
    {
        d_name = generateName();
        age = Random.Range(20, 7000).ToString();
        if (Random.Range(0, 200) == 0)
        {
            isGreg = true;
            d_name = "Greg";
            age = "???";
        }
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

    private string generateName()
    {
        StringBuilder sb = new StringBuilder();

        int id_name = Random.Range(0, names.Length);
        int id_nickname = Random.Range(0, nicknames.Length);
        int id_surname = Random.Range(0, surnames.Length);

        sb.Append(
            names[id_name].Substring(0, (names[id_name].Length)-2) +
            " " + nicknames[id_nickname] +
            surnames[id_surname].Substring(0, (surnames[id_surname].Length)-1)
            );

        return sb.ToString();
    }

}
