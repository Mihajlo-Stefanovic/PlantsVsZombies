using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum CardType
{
    Shooter,
    Collector,
    MachineGun
}

public class TechCard : MonoBehaviour, IPointerDownHandler
{
    public CardType type;
    public ParticleSystem ps;
    
    public void OnPointerDown(PointerEventData pointerEventData)
    {
        GameManager.Instance.TechCardClicked(this);
        ps.Play();
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