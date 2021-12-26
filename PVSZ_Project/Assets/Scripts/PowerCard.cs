using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum PowerCardType
{
    Scan,
    Block,
    Slow,
    Shield
}

public class PowerCard : MonoBehaviour, IPointerDownHandler
{
    public Image    image;
    public Material disabledMaterial;
    
    public PowerCardType Type;

    [HideInInspector]
    public bool isAble = true;
    
    public void OnPointerDown(PointerEventData pointerData)
    {
        if ((pointerData.button == PointerEventData.InputButton.Left) && isAble)
        {
            GameManager.Instance.PowerCardClicked(this);
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