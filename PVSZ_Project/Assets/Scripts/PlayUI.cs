using UnityEngine;

public class PlayUI : MonoBehaviour
{
    public EndTurn      endTurn;
    public GameObject   techCards;
    public RemoveCard   removeCard;
    public GameObject   powerCards;
    
    public void OnAlienTurn()
    {
        endTurn.Dissable();
        removeCard.Dissable();
        
        powerCards.SetActive(true);
        techCards.SetActive(false);
    }
    
    public void OnTechTurn()
    {
        endTurn.Enable();
        removeCard.Enable();
        
        powerCards.SetActive(false);
        techCards.SetActive(true);
    }
}