using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DescriptionManager : MonoBehaviour
{
    private static DescriptionManager current;

    private void Awake()
    {
        current = this;
        current.gameObject.SetActive(false);

    }

    public static void Show()
    {
        current.gameObject.SetActive(true);
    }

    public static void Hide()
    {
        current.gameObject.SetActive(false);
    }
}
