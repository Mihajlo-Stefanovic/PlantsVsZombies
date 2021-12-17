using UnityEngine;
using UnityEngine.EventSystems;

public class PowerCard : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData pointerEventData)
    {
        GameManager.Instance.PowerCardClicked(this);
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