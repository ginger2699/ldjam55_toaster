using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DescriptionTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        DescriptionManager.Show();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        DescriptionManager.Hide();
    }
}
