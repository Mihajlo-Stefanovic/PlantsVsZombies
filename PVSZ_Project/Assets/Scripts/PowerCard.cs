using UnityEngine;
using UnityEngine.EventSystems;

public enum PowerCardType
{
    Scan,
    Block,
    Slow,
    Shield
}

public class PowerCard : MonoBehaviour, IPointerDownHandler
{
    public PowerCardType Type;
    
    public void OnPointerDown(PointerEventData pointerData)
    {
        if (pointerData.button == PointerEventData.InputButton.Left)
        {
            GameManager.Instance.PowerCardClicked(this);
        }
    }
    
    public void Enable()
    {
        gameObject.SetActive(true);
    }
    
    public void Disable()
    {
        gameObject.SetActive(false);
    }
}