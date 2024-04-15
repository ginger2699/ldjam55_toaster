using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class GeneratePropicParts : MonoBehaviour
{
    private enum PartType
    {
        BODY,
        HEAD,
        MOUTH,
        NOSE,
        EARS,
        EYES,
        HORNS,
        GREG
    }

    [SerializeField] PartType type;
    private Sprite sprite;
    private Image image;
    private StringBuilder sb = new StringBuilder();
    private string partName = "";
    private int numParts = 0;
    private int curPartNumber = 0;
    private Color color;
    private bool isGreg = false;
    private bool drawGreg = false;


    public void Start()
    {
        image = gameObject.GetComponent<Image>();

        sb.Append("PropicParts/");

        switch (type)
        {
            case PartType.BODY:
                partName = "body";
                numParts = 3;
                break;
            case PartType.HEAD:
                partName = "head";
                numParts = 8;
                break;
            case PartType.MOUTH:
                partName = "mouth";
                numParts = 10;
                break;
            case PartType.NOSE:
                partName = "nose";
                numParts = 8;
                break;
            case PartType.EARS:
                partName = "ears";
                numParts = 8;
                break;
            case PartType.EYES:
                partName = "eyes";
                numParts = 14;
                break;
            case PartType.HORNS:
                partName = "horns";
                numParts = 8;
                break;
            case PartType.GREG:
                isGreg = true;
                break;
        }
    }

    public void setColor(Color value)
    {
        print(value);
        color = value;
    }

    public void setGreg(bool value)
    {
        drawGreg = value;
    }

    public void GeneratePropics()
    {
        if (!isGreg)
        {
            if (partName != "eyes" && partName != "mouth")
                image.color = color;

            curPartNumber = Random.Range(1, numParts + 1);

            if (curPartNumber < 10)
                sb.Append(partName + "0");
            else
                sb.Append(partName);

            sb.Append(curPartNumber);
        }
        else
        {
            if (drawGreg)
                sb.Append("greg");
            else
                sb.Append("default");
        }

        sprite = Resources.Load<Sprite>(sb.ToString());

        image.sprite = sprite;

        ResetPath();
    }

    private void ResetPath()
    {
        sb.Clear();
        sb.Append("PropicParts/");
    }

}
