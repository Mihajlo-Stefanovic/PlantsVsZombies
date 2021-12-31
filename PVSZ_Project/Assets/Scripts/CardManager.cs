using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public struct CardInfo
{
    public CardType    Type;
    public int         Cost;
    
    public TechUnit    UnitPrefab; // NOTE(sftl): null for PowerCards
    
    public void Deconstruct(out CardType type, out int cost, out TechUnit unitPrefab)
    {
        type = Type; cost = Cost; unitPrefab = UnitPrefab;
    }
}

public class CardManager : MonoBehaviour
{
    public static CardManager Instance;
    
    [SerializeField] private CardInfo[]     _cardsInfoArray; // NOTE(sftl): unity inspector hack
    public Dictionary<CardType, CardInfo>   CardsInfo;
    
    [SerializeField] private List<Card> _cards; // NOTE(sftl): cards on the scene
    
    public Material DisabledMaterial; // NOTE(sftl): used on disabled cards
    
    void Awake()
    {
        //-singleton
        if (Instance != null) Destroy(gameObject);
        else Instance = this;
        
        //-init cardsInfo
        CardsInfo = new();
        foreach (var cardInfo in _cardsInfoArray) CardsInfo.Add(cardInfo.Type, cardInfo); 
    }
    
    void Start()
    {
        foreach (var card in _cards) card.Cost = CardsInfo[card.Type].Cost;
    }
    
    public void UpdateCardsForResources(int res)
    {
        foreach (var card in _cards)
        {
            if (res < CardsInfo[card.Type].Cost) card.Enabled = false;
            else card.Enabled = true;
        }
    }
}