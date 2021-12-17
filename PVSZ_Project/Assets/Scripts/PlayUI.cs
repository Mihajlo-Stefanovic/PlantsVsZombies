using UnityEngine;

public class PlayUI : MonoBehaviour
{
    public EndTurn      endTurn;
    public TechCard     card; // NOTE(sftl): handle multiple
    public RemoveCard   removeCard;
    public PowerCard    powerCard; // NOTE(sftl): handle multiple
    
    public void OnAlienTurn()
    {
        endTurn.Dissable();
        card.Dissable();
        removeCard.Dissable();
        
        powerCard.Enable();
    }
    
    public void OnTechTurn()
    {
        endTurn.Enable();
        card.Enable();
        removeCard.Enable();
        
        powerCard.Disable();
    }
}