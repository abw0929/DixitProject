﻿using UnityEngine;
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
            Debug.Log(e.Message);
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
            Debug.Log(e.Message);
        }
    }


    public static void ReceiveMessage(PlayerIndex playerIndex, MyMessage msg)
    {
        try
        {
            if(playerIndex == PlayerIndex.All || playerIndex == GlobalVariables.PlayerIndex)
            {
                Debug.Log(string.Format("Receive: {0} {1} value : {2} msg : {3}", (MyMessageType)msg.messageType, playerIndex, msg.value, msg.msg));
                OnMessage(msg);
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.StackTrace);
        }
    }


    public static void SendGiveIndex(int index)
    {
        SendMessageToAll(new MyMessage(MyMessageType.SetPlayerIndex, index, ""));
    }


    public static void SendStartGame()
    {
        SendMessageToAll(new MyMessage(MyMessageType.StartGame));
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


    public static void SendAskVote(int[] showDeck)
    {
        SendMessageToAll(new MyMessage(MyMessageType.AskVote, showDeck, ""));
    }


    public static void SendVoteCard(int card)
    {
        SendMessageToPlayer(PlayerIndex.Player1, new MyMessage(MyMessageType.GiveVote, card, ""));
    }


    public static void SendShowResult()
    {
        SendMessageToAll(new MyMessage(MyMessageType.ShowResult));
    }


    public static void SendResetAllClients()
    {
        SendMessageToAll(new MyMessage(MyMessageType.ResetAllClient));
    }


    private static void ReceiveOnClientConnect()
    {
        GlobalObjects.IndexControl.GiveIndex();
        SendMessageToAll(new MyMessage(MyMessageType.SetPlayerNum, GlobalObjects.IndexControl.CurrentPlayers, ""));
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
            gameFlow.SetPlayerNum();
        }
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
        gameFlow.AskedGiveOutCard(title, value);
    }


    private static void ReceiveGiveCard(int from, int value)
    {
        gameFlow.GotGivenCard(from, value);
    }


    private static void ReceiveAskVote(int[] values)
    {
        gameFlow.AskedGiveVote(values);
    }


    private static void ReceiveGiveVote(int from, int value)
    {
        gameFlow.GotGivenVote(from, value);
    }


    private static void ReceiveShowResult()
    {
        gameFlow.ShowResult();
    }


    private static void ReceiveResetAllClient()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }


    public static void StartGame()
    {
        gameFlow.StartGame();
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
                    ReceiveAskVote(msg.values);
                    break;
                }
            case MyMessageType.GiveVote:
                {
                    ReceiveGiveVote(msg.from, msg.value);
                    break;
                }
            case MyMessageType.ShowResult:
                {
                    ReceiveShowResult();
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
