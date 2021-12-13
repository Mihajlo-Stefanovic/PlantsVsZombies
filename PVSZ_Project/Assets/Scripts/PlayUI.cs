using UnityEngine;

public class PlayUI : MonoBehaviour
{
    public EndTurn      endTurn;
    public TechCard     card; // NOTE(sftl): handle multiple
    public RemoveCard   removeCard;
    
    public void Dissable()
    {
        endTurn.Dissable();
        card.Dissable();
        removeCard.Dissable();
    }
    
    public void Enable()
    {
        endTurn.Enable();
        card.Enable();
        removeCard.Enable();
    }
}