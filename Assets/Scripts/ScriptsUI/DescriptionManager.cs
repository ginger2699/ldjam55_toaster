using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using JetBrains.Annotations;

public class DescriptionManager : MonoBehaviour
{
    private static DescriptionManager current;
    public static int visibleChild = 0;

    private void Awake()
    {
        current = this;
        current.gameObject.SetActive(false);

    }

    public static void Show(int childToShow)
    {
        current.gameObject.SetActive(true);
        current.transform.GetChild(childToShow).gameObject.SetActive(true);
        visibleChild = childToShow;
    }

    public static void Hide()
    {
        current.gameObject.SetActive(false);
        current.transform.GetChild(visibleChild).gameObject.SetActive(false);
    }
}
