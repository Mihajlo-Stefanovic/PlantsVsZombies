using UnityEngine;

public class PlayUI : MonoBehaviour
{
    public EndTurn      endTurn;
    public TechCard     card;           // NOTE(sftl): handle multiple
    public RemoveCard   removeCard;
    public GameObject   powerCards;
    
    public void OnAlienTurn()
    {
        endTurn.Dissable();
        card.Dissable();
        removeCard.Dissable();
        
        powerCards.SetActive(true);
    }
    
    public void OnTechTurn()
    {
        endTurn.Enable();
        card.Enable();
        removeCard.Enable();
        
        powerCards.SetActive(false);
    }
}