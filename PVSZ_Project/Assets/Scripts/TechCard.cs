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
        if (this.type == CardType.Shooter) GameManager.Instance.TechCardClicked(this);
        if (this.type == CardType.Collector) GameManager.Instance.ResourceCardClicked(this);

    }

    public void Enable()
    {
        gameObject.SetActive(true);
    }

    public void Dissable()
    {
        gameObject.SetActive(false);
    }
}