using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameFlow : MonoBehaviour {


    [SerializeField]
    private StageControl stage;
    [SerializeField]
    private CardControl myCard;

    private Dictionary<PlayerIndex, int> playerVotes = new Dictionary<PlayerIndex, int>();

    private CardDeckControl cardDeck = new CardDeckControl();


	void Awake () {
        GameFlowControl.SetGameFlow(this);
	}
	
	
    private IEnumerator StartGameRoutine()
    {
        GameFlowControl.State = GameStates.StartGame;
        stage.SetConnInfoVis(false);
        StartCoroutine(FirstDealRoutine());
        yield return null;
    }


    private IEnumerator FirstDealRoutine()
    {
        GameFlowControl.State = GameStates.DealingCards;

        stage.SetInputVis(false);
        stage.SetAllCardVis(false);
        stage.SetStartButtonVis(false);
        stage.SetInfoText(false, "");
        stage.SetTitleText(false, "");
        stage.SetInfoText(true, "Waiting For Story.");

        if (GlobalVariables.PlayerIndex == PlayerIndex.Player1)
        {
            cardDeck.Shuffle();
            for(int i = 1; i <= GlobalVariables.PlayerNum; i++)
            {
                for(int j = 0; j < GlobalVariables.MyCardNum; j++)
                {
                    CardType dealCard = cardDeck.DealCardToPlayer((PlayerIndex)i);
                    GameFlowControl.SendDealCardToPlayer((PlayerIndex)i, dealCard);
                }
            }
        }

        StartCoroutine(AskTitleRoutine());

        yield return null;
    }


    private IEnumerator RoundDealRoutine()
    {
        GameFlowControl.State = GameStates.DealingCards;

        stage.SetInputVis(false);
        stage.SetAllCardVis(false);
        stage.SetStartButtonVis(false);
        stage.SetInfoText(false, "");
        stage.SetTitleText(false, "");
        stage.SetInfoText(true, "Waiting For Story.");

        if (GlobalVariables.PlayerIndex == PlayerIndex.Player1)
        {
            cardDeck.Shuffle();
            for (int i = 1; i <= GlobalVariables.PlayerNum; i++)
            {
                while(cardDeck.GetPlayerDeckCount((PlayerIndex)i) < GlobalVariables.MyCardNum)
                {
                    cardDeck.DealCardToPlayer((PlayerIndex)i);
                }
                CardType[] playerDeck = cardDeck.GetPlayerDeck((PlayerIndex)i);
                for (int j = 0; j < GlobalVariables.MyCardNum; j++)
                {
                    GameFlowControl.SendDealCardToPlayer((PlayerIndex)i, playerDeck[j]);
                }
            }
        }

        StartCoroutine(AskTitleRoutine());

        yield return null;
    }


    private IEnumerator AskTitleRoutine()
    {
        if (GlobalVariables.PlayerIndex == PlayerIndex.Player1)
        {
            GameFlowControl.SendAskTitle((PlayerIndex)GameFlowControl.TitlePlayer);
        }

        yield return null;
    }


    private IEnumerator ShowResultRoutine()
    {
        stage.SetInfoText(true, "RESULTS");

        yield return new WaitForSeconds(5.0f);

        if (GlobalVariables.PlayerIndex == PlayerIndex.Player1)
            GameFlowControl.SendNextRound();

        yield return null;
    }


    private IEnumerator NextRoundRoutine()
    {
        GameFlowControl.TitlePlayer++;
        if (GameFlowControl.TitlePlayer > GlobalVariables.PlayerNum)
            GameFlowControl.TitlePlayer = 1;

        if (GlobalVariables.PlayerIndex == PlayerIndex.Player1)
            cardDeck.RecycleShowCards();

        myCard.Reset();
        playerVotes.Clear();

        StartCoroutine(RoundDealRoutine());

        yield return null;
    }


    public void SetPlayerNum()
    {
        if (GlobalVariables.PlayerNum >= 3 && GlobalVariables.PlayerIndex == PlayerIndex.Player1)
        {
            stage.SetStartButtonVis(true);
            stage.SetConnInfoVis(false);
        }
    }


    public void StartGame()
    {
        StartCoroutine(StartGameRoutine());
    }


    public void StartNextRound()
    {
        StopAllCoroutines();
        StartCoroutine(NextRoundRoutine());
    }


    public void ShowResult()
    {
        StartCoroutine(ShowResultRoutine());
    }


    public void ShowReceivedCard(CardType card)
    {
        myCard.GetCard(card);
        int currentCardIndex = myCard.CardNum - 1;
        stage.SetCardVis(currentCardIndex, true);
        stage.SetCardImage(currentCardIndex, card);
    }


    public void AskedTitle()
    {
        GameFlowControl.State = GameStates.WaitingTitle;
        stage.SetInfoText(true, "Please Tell A Story.");
    }


    public void AskedGiveOutCard(string title, int titleCard)
    {
        GameFlowControl.State = GameStates.WaitingAllCards;
        if((int)GlobalVariables.PlayerIndex != GameFlowControl.TitlePlayer)
            stage.SetInfoText(true, "Please Give Card(s)...");
        stage.SetTitleText(true, "The Story is : \n" + title);

        if(GlobalVariables.PlayerIndex == PlayerIndex.Player1)
        {
            cardDeck.PlayerGiveOutCard((PlayerIndex)GameFlowControl.TitlePlayer, (CardType)titleCard);
        }
    }


    public void AskedGiveVote(int[] values)
    {
        stage.SetAllCardVis(false);
        for (int i = 0; i < values.Length; i++)
        {
            stage.SetCardVis(i, true);
            stage.SetCardImage(i, (CardType)values[i]);
        }

        if((int)GlobalVariables.PlayerIndex != GameFlowControl.TitlePlayer)
        {
            GameFlowControl.State = GameStates.WaitingAllVotes;
            stage.SetInfoText(true, "Please Vote.");
        }
        else
        {
            GameFlowControl.State = GameStates.VoteGiven;
            stage.SetInfoText(true, "Waiting For Votes...");
        }
    }


    public void GotGivenCard(int from, int card)
    {
        if(GlobalVariables.PlayerIndex == PlayerIndex.Player1)
        {
            cardDeck.PlayerGiveOutCard((PlayerIndex)from, (CardType)card);

            if(cardDeck.ShowDeckCardNum >= GlobalVariables.PublicCardNum)
            {
                cardDeck.ShuffleShowDeck();
                GameFlowControl.SendAskVote(cardDeck.CurrentShowDeck);
            }
        }
    }


    public void GotGivenVote(int from, int card)
    {
        if (GlobalVariables.PlayerIndex == PlayerIndex.Player1)
        {
            playerVotes.Add((PlayerIndex)from, card);

            if (playerVotes.Count >= GlobalVariables.PlayerNum - 1)
                GameFlowControl.SendShowResult();
        }
    }


    public void SelectTitle(int index)
    {
        stage.PreviewImageOpen(index);
        stage.SetInputVis(true);
    }


    public void SelectGiveOutCard(int index)
    {
        stage.PreviewImageOpen(index);
        stage.SetYesNoVis(true);
    }


    public void SelectVoteCard(int index)
    {
        stage.PreviewImageOpen(index);
        stage.SetYesNoVis(true);
    }


    public void CloseSelect()
    {
        stage.PreviewImageClose();
        stage.SetInputVis(false);
    }


    public void SendTitle(int index)
    {
        if (stage.GetInputText() == "")
            return;

        GameFlowControl.SendGiveTitle(stage.GetInputText(), (int)myCard.ViewCard(index));
        stage.SetInfoText(true, "Waiting For Cards...");

        stage.SetAllCardVis(false);
        stage.SetCardVis(0, true);
        stage.SetCardImage(0, myCard.ViewCard(index));
        stage.PreviewImageFlyOut();
        stage.SetInputVis(false);
        myCard.RemoveCard(index);
    }


    public void SendGiveCard(int index)
    {
        GameFlowControl.SendGiveCard((int)myCard.ViewCard(index));

        myCard.RemoveCard(index);
        stage.PreviewImageFlyOut();
        stage.SetInputVis(false);
        stage.SetAllCardVis(false);
        for (int i = 0; i < myCard.CardNum; i++)
        {
            stage.SetCardVis(i, true);
            stage.SetCardImage(i, myCard.ViewCard(i));
        }

        if (myCard.CardsGiven >= GlobalVariables.SendCardNum)
        {
            GameFlowControl.State = GameStates.CardsGiven;
            stage.SetInfoText(true, "Waiting For Others...");
        }
    }


    public void SendVoteCard(int index)
    {
        GameFlowControl.SendVoteCard((int)myCard.ViewCard(index));
        stage.PreviewImageFlyOut();
        stage.SetInputVis(false);

        GameFlowControl.State = GameStates.VoteGiven;
        stage.SetInfoText(true, "Waiting For Others...");
    }

}
