using UnityEngine;
using System.Collections.Generic;

public class CardControl : MonoBehaviour {


    private List<CardType> myCards;

    private int cardsGiven = 0;
    public int CardsGiven
    {
        get { return cardsGiven; }
    }

    public int CardNum
    {
        get { return myCards.Count; }
    }
	

    private void Awake()
    {
        myCards = new List<CardType>();
    }


    public void Reset()
    {
        myCards.Clear();
        cardsGiven = 0;
    }


    public void GetCard(CardType card)
    {
        myCards.Add(card);
        if (cardsGiven > 0)
            cardsGiven--;
    }


    public CardType ViewCard(int index)
    {
        return myCards[index];
    }


    public void RemoveCard(int index)
    {
        myCards.RemoveAt(index);
        cardsGiven++;
    }


}
