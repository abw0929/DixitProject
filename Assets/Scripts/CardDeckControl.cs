using System.Collections.Generic;
using UnityEngine;
using System;

public class CardDeckControl {

    private List<CardType> deck = new List<CardType>();
    private List<CardType>[] playerDeck = new List<CardType>[7];
    private List<CardType> showDeck = new List<CardType>();
    private List<PlayerIndex> showDeckGiver = new List<PlayerIndex>();

    public int ShowDeckCardNum
    {
        get { return showDeck.Count; }
    }

    public int[] CurrentShowDeck
    {
        get
        {
            int[] result = new int[showDeck.Count];
            for (int i = 0; i < result.Length; i++)
                result[i] = (int)showDeck[i];

            return result;
        }
    }


    public int[] CurrentGiver
    {
        get
        {
            int[] result = new int[showDeckGiver.Count];
            for (int i = 0; i < result.Length; i++)
                result[i] = (int)showDeckGiver[i];

            return result;
        }
    }


    public CardDeckControl()
    {
        for(int i = 0; i < GlobalVariables.TOTAL_CARD_NUM; i++)
            deck.Add((CardType)i);

        for(int i = 0; i < 7; i++)
            playerDeck[i] = new List<CardType>();
    }


    public CardType[] GetPlayerDeck(PlayerIndex player)
    {
        return playerDeck[(int)player - 1].ToArray();
    }


    public int GetPlayerDeckCount(PlayerIndex player)
    {
        return playerDeck[(int)player - 1].Count;
    }


    public void Shuffle()
    {
        for(int i = 0; i < deck.Count - 1; i++)
        {
            int randIndex = UnityEngine.Random.Range(i, deck.Count);
            CardType temp = deck[randIndex];
            deck[randIndex] = deck[i];
            deck[i] = temp;
        }
    }


    public void ShuffleShowDeck()
    {
        for (int i = 0; i < showDeck.Count - 1; i++)
        {
            int randIndex = UnityEngine.Random.Range(i, showDeck.Count);
            CardType temp = showDeck[randIndex];
            showDeck[randIndex] = showDeck[i];
            showDeck[i] = temp;
            PlayerIndex temp2 = showDeckGiver[randIndex];
            showDeckGiver[randIndex] = showDeckGiver[i];
            showDeckGiver[i] = temp2;
        }
    }


    public CardType DealCardToPlayer(PlayerIndex player)
    {
        if (playerDeck[(int)player - 1].Count >= GlobalVariables.MyCardNum)
            throw new Exception("Player has too much cards!");

        CardType dealCard = deck[0];
        playerDeck[(int)player - 1].Add(dealCard);
        deck.RemoveAt(0);

        return dealCard;
    }


    public void PlayerGiveOutCard(PlayerIndex player, CardType card)
    {
        if (!playerDeck[(int)player - 1].Contains(card))
            throw new Exception("Player does not have that card!");

        if(playerDeck[(int)player - 1].Count == 0)
            throw new Exception("Player has no cards!");

        if (showDeck.Count >= GlobalVariables.PublicCardNum)
            throw new Exception("Showdeck has too much cards!");

        playerDeck[(int)player - 1].Remove(card);
        showDeck.Add(card);
        showDeckGiver.Add(player);
    }


    public void RecycleShowCards()
    {
        deck.AddRange(showDeck);
        showDeck.Clear();
        showDeckGiver.Clear();
    }

}
