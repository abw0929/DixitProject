using UnityEngine.Networking;
using System;


public enum MyMessageType
{
    OnClientConnect = 0,
    SetPlayerIndex = 1,
    SetPlayerNum = 2,
    StartGame = 10,
    DealCard = 11,
    AskTitle = 12,
    GiveTitle = 13,
    AskCard = 14,
    GiveCard = 15,
    AskVote = 16,
    GiveVote = 17,
    ShowResult = 20,
    StartNextRound = 21,
    ResetAllClient = 99,
}

[Serializable]
public struct MyMessage
{
    public int messageType;
    public int value;
    public int[] values;
    public int[] values2;
    public string msg;
    public int from;

    public MyMessage(MyMessageType type)
    {
        this.messageType = (int)type;
        this.value = 0;
        this.values = new int[0];
        this.values2 = new int[0];
        this.msg = "";
        this.from = (int)GlobalVariables.PlayerIndex;
    }

    public MyMessage(MyMessageType type, int value, string msg)
    {
        this.messageType = (int)type;
        this.value = value;
        this.values = new int[0];
        this.values2 = new int[0];
        this.msg = msg;
        this.from = (int)GlobalVariables.PlayerIndex;
    }

    public MyMessage(MyMessageType type, int[] values, string msg)
    {
        this.messageType = (int)type;
        this.value = 0;
        this.values = values;
        this.values2 = new int[0];
        this.msg = msg;
        this.from = (int)GlobalVariables.PlayerIndex;
    }

    public MyMessage(MyMessageType type, int[] values, int[] values2, string msg)
    {
        this.messageType = (int)type;
        this.value = 0;
        this.values = values;
        this.values2 = values2;
        this.msg = msg;
        this.from = (int)GlobalVariables.PlayerIndex;
    }
}


public class NetworkMessageControl : NetworkBehaviour {


    private void Start()
    {
        if (isLocalPlayer)
        {
            GlobalObjects.SetMessageControl(this);
            if (isServer)
            {
                GlobalObjects.IndexControl.SetIndex(PlayerIndex.Player1);
                GameFlowControl.SendSetPlayerNum();
            }
            else
            {
                GameFlowControl.SendOnClientConnect();
            }
        }
    }


    public void SendToAll(MyMessage msg)
    {
        if (isLocalPlayer)
        {
            CmdToServer(PlayerIndex.All, msg);
        }
    }


    public void SendToPlayer(PlayerIndex playerIndex, MyMessage msg)
    {
        if (isLocalPlayer)
        {
            CmdToServer(playerIndex, msg);
        }
    }


    [Command]
    private void CmdToServer(PlayerIndex playerIndex, MyMessage msg)
    {
        RpcPlayers(playerIndex, msg);
    }


    [ClientRpc]
    private void RpcPlayers(PlayerIndex playerIndex, MyMessage msg)
    {
        GameFlowControl.ReceiveMessage(playerIndex, msg);
    }


}
