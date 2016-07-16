using UnityEngine;
using System.Collections;
using System;


public enum GameStats
{
    Waiting = 0,
    StartGame = 1,
    WaitingTitle = 2,
    WaitingAllCards = 3,
    ShowResult = 4,
    GameEnd = 5
}


public static class GameFlowControl {


    private static GameFlow gameFlow;
	

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
                Debug.Log(string.Format("Receive: {0} {1} value : {2} msg : {3}", msg.messageType, playerIndex, msg.value, msg.msg));
                OnMessage(msg);
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    private static void OnClientConnect()
    {
        GlobalObjects.IndexControl.GiveIndex();
        SendMessageToAll(new MyMessage(MyMessageType.SetPlayerNum, GlobalObjects.IndexControl.CurrentPlayers, ""));
    }


    private static void SetPlayerIndex(int value)
    {
        GlobalObjects.IndexControl.SetIndex((PlayerIndex)value);
    }


    private static void SetPlayerNum(int value)
    {
        if(value >= 3)
            GlobalVariables.SetPlayerNum((PlayerNumberType)(value));
    }


    private static void OnMessage(MyMessage msg)
    {
        switch (msg.messageType)
        {
            case MyMessageType.OnClientConnect:
                {
                    OnClientConnect();
                    break;
                }
            case MyMessageType.SetPlayerIndex:
                {
                    SetPlayerIndex(msg.value);
                    break;
                }
            case MyMessageType.SetPlayerNum:
                {
                    SetPlayerNum(msg.value);
                    break;
                }
        }
    }


}
