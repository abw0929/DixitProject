using UnityEngine;
using System.Collections;
using System;


public enum GameStates
{
    None = -1,
    Waiting = 0,
    StartGame = 1,
    DealingCards = 2,
    WaitingTitle = 3,
    TitleGiven = 4,
    WaitingAllCards = 5,
    CardsGiven = 6,
    WaitingAllVotes = 7,
    VoteGiven = 8,
    ShowResult = 9,
    GameEnd = 10,

}


public static class GameFlowControl {


    private static GameFlow gameFlow;

    public static GameStates State = GameStates.None;
    public static int TitlePlayer = 1;
    public static int TitleCard = 0;

    public static int[] CurrentShowDeck;
    public static int[] CurrentShowGiver;

    public static int[] PlayerPoints;
    public static int[] PlayerGetPoints;

    public static void SetGameFlow(GameFlow flow)
    {
        gameFlow = flow;
    }


    public static void SendMessageToAll(MyMessage msg)
    {
        try
        {
            GlobalObjects.NetworkMessage.SendToAll(msg);
        }
        catch(Exception e)
        {
            Debug.Log(e.Message + "\n" + e.StackTrace);
        }
    }


    public static void SendMessageToPlayer(PlayerIndex playerIndex, MyMessage msg)
    {
        try
        {
            GlobalObjects.NetworkMessage.SendToPlayer(playerIndex, msg);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message + "\n" + e.StackTrace);
        }
    }


    public static void ReceiveMessage(PlayerIndex playerIndex, MyMessage msg)
    {
        try
        {
            if(playerIndex == PlayerIndex.All || playerIndex == GlobalVariables.PlayerIndex)
            {
                Debug.Log(string.Format("Receive: {0} from : {1} to : {2} value : {3} msg : {4}", (MyMessageType)msg.messageType, (PlayerIndex)msg.from, playerIndex, msg.value, msg.msg));
                OnMessage(msg);
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message + "\n" + e.StackTrace);
        }
    }


    public static void SendOnClientConnect()
    {
        SendMessageToPlayer(PlayerIndex.Player1, new MyMessage(MyMessageType.OnClientConnect));
    }


    public static void SendGiveIndex(int index)
    {
        SendMessageToAll(new MyMessage(MyMessageType.SetPlayerIndex, index, ""));
    }


    public static void SendSetPlayerNum()
    {
        SendMessageToAll(new MyMessage(MyMessageType.SetPlayerNum, GlobalObjects.IndexControl.CurrentPlayers, ""));
    }


    public static void SendStartGame()
    {
        SendMessageToAll(new MyMessage(MyMessageType.StartGame));
    }

    
    public static void SendNextRound()
    {
        SendMessageToAll(new MyMessage(MyMessageType.StartNextRound));
    }


    public static void SendDealCardToPlayer(PlayerIndex player, CardType card)
    {
        SendMessageToPlayer(player, new MyMessage(MyMessageType.DealCard, (int)card, ""));
    }


    public static void SendAskTitle(PlayerIndex player)
    {
        SendMessageToPlayer(player, new MyMessage(MyMessageType.AskTitle));
    }


    public static void SendGiveTitle(string title, int card)
    {
        SendMessageToAll(new MyMessage(MyMessageType.GiveTitle, card, title));
    }


    public static void SendGiveCard(int card)
    {
        SendMessageToPlayer(PlayerIndex.Player1, new MyMessage(MyMessageType.GiveCard, card, ""));
    }


    public static void SendAskVote(int[] showDeck, int[] giver)
    {
        SendMessageToAll(new MyMessage(MyMessageType.AskVote, showDeck, giver, ""));
    }


    public static void SendVoteCard(int card)
    {
        SendMessageToPlayer(PlayerIndex.Player1, new MyMessage(MyMessageType.GiveVote, card, ""));
    }


    public static void SendShowResult(int[] playerPoints, int[] playerGetPoints)
    {
        SendMessageToAll(new MyMessage(MyMessageType.ShowResult, playerPoints, playerGetPoints, ""));
    }


    public static void SendResetAllClients()
    {
        SendMessageToAll(new MyMessage(MyMessageType.ResetAllClient));
    }


    private static void ReceiveOnClientConnect()
    {
        GlobalObjects.IndexControl.GiveIndex();
        SendSetPlayerNum();
    }


    private static void ReceiveSetPlayerIndex(int value)
    {
        GlobalObjects.IndexControl.SetIndex((PlayerIndex)value);
    }


    private static void ReceiveSetPlayerNum(int value)
    {
        if (value >= 3)
        {
            GlobalVariables.SetPlayerNum((PlayerNumberType)(value));
        }
        gameFlow.SetPlayerNum(value);
    }


    private static void ReceiveStartGame()
    {
        StartGame();
    }


    private static void ReceiveDealCard(int value)
    {
        gameFlow.ShowReceivedCard((CardType)value);
    }


    private static void ReceiveAskTitle()
    {
        gameFlow.AskedTitle();
    }


    private static void ReceiveAskCard(string title, int value)
    {
        TitleCard = value;
        gameFlow.AskedGiveOutCard(title, value);
    }


    private static void ReceiveGiveCard(int from, int value)
    {
        gameFlow.GotGivenCard(from, value);
    }


    private static void ReceiveAskVote(int[] values, int[] values2)
    {
        CurrentShowDeck = values;
        CurrentShowGiver = values2;
        gameFlow.AskedGiveVote(values);
    }


    private static void ReceiveGiveVote(int from, int value)
    {
        gameFlow.GotGivenVote(from, value);
    }


    private static void ReceiveShowResult(int[] values, int[] values2)
    {
        PlayerPoints = values;
        PlayerGetPoints = values2;
        gameFlow.ShowResult();
    }


    private static void ReceiveStartNextRound()
    {
        gameFlow.StartNextRound();
    }


    private static void ReceiveResetAllClient()
    {
        GlobalObjects.NetworkManager.Reset();
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }


    public static void StartGame()
    {
        gameFlow.StartGame();
    }


    public static void Reset()
    {
        State = GameStates.None;
        TitlePlayer = 1;
        TitleCard = 0;
        CurrentShowDeck = null;
        CurrentShowGiver = null;

        PlayerPoints = null;
        PlayerGetPoints = null;
    }


    private static void OnMessage(MyMessage msg)
    {
        switch ((MyMessageType)msg.messageType)
        {
            case MyMessageType.OnClientConnect:
                {
                    ReceiveOnClientConnect();
                    break;
                }
            case MyMessageType.SetPlayerIndex:
                {
                    ReceiveSetPlayerIndex(msg.value);
                    break;
                }
            case MyMessageType.SetPlayerNum:
                {
                    ReceiveSetPlayerNum(msg.value);
                    break;
                }
            case MyMessageType.StartGame:
                {
                    ReceiveStartGame();
                    break;
                }
            case MyMessageType.DealCard:
                {
                    ReceiveDealCard(msg.value);
                    break;
                }
            case MyMessageType.AskTitle:
                {
                    ReceiveAskTitle();
                    break;
                }
            case MyMessageType.GiveTitle:
                {
                    ReceiveAskCard(msg.msg, msg.value);
                    break;
                }
            case MyMessageType.GiveCard:
                {
                    ReceiveGiveCard(msg.from, msg.value);
                    break;
                }
            case MyMessageType.AskVote:
                {
                    ReceiveAskVote(msg.values, msg.values2);
                    break;
                }
            case MyMessageType.GiveVote:
                {
                    ReceiveGiveVote(msg.from, msg.value);
                    break;
                }
            case MyMessageType.ShowResult:
                {
                    ReceiveShowResult(msg.values, msg.values2);
                    break;
                }
            case MyMessageType.StartNextRound:
                {
                    ReceiveStartNextRound();
                    break;
                }
            case MyMessageType.ResetAllClient:
                {
                    ReceiveResetAllClient();
                    break;
                }
        }
    }


}
