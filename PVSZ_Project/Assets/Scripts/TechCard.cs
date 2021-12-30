using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum CardType
{
    Guardian,
    Collector,
    MachineGun,
    Wall
}

public class TechCard : MonoBehaviour, IPointerDownHandler
{
    public Image    image;
    public Material disabledMaterial;
    
    public ParticleSystem ps;
    
    public CardType type;
    
    [HideInInspector] 
        public bool isAble = true;
    
    public void OnPointerDown(PointerEventData pointerData)
    {
        if ((pointerData.button == PointerEventData.InputButton.Left) && isAble)
        {
            GameManager.Instance.TechCardClicked(this);
            ps.Play();
        }
    }
    
    public void Enable()
    {
        image.material = null; // NOTE(sftl): default material for UI
        isAble = true;
    }
    
    public void Disable()
    {
        image.material = disabledMaterial; 
        isAble = false;
    }
}