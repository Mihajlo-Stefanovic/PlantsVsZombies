using UnityEngine;
using UnityEngine.EventSystems;

public class EndTurn : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData pointerEventData)
    {
        GameManager.Instance.EndTechTurn();
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