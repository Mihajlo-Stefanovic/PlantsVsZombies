using UnityEngine;

public class PlayUI : MonoBehaviour
{
    public EndTurn      endTurn;
    public GameObject   techCards;
    public RemoveCard   removeCard;
    
    public GameObject   powerCards;
    public FastForward  fastForward;
    
    public void OnAlienTurn()
    {
        endTurn.Dissable();
        removeCard.Dissable();
        techCards.SetActive(false);
        
        powerCards.SetActive(true);
        fastForward.Enable();
    }
    
    public void OnTechTurn()
    {
        endTurn.Enable();
        removeCard.Enable();
        techCards.SetActive(true);
        
        powerCards.SetActive(false);
        fastForward.Dissable();
    }
    
    public void OnFastForward()
    {
        powerCards.SetActive(false);
        fastForward.Dissable();
    }
}