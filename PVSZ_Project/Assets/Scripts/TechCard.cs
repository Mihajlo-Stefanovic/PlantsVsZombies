using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum CardType
{
    Shooter,
    Collector
}

public class TechCard : MonoBehaviour, IPointerDownHandler
{
    public CardType type;
    
    public void OnPointerDown(PointerEventData pointerEventData)
    {
        GameManager.Instance.TechCardClicked(this);
    }
}