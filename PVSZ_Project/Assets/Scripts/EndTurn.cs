using UnityEngine;
using UnityEngine.EventSystems;

public class EndTurn : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    
    public void OnPointerDown(PointerEventData eventData)
    {
        // NOTE(sftl): nothing
    }
    
    public void OnPointerUp(PointerEventData pointerEventData)
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