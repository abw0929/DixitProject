using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
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
    ShowResult = 20,
}


public class MyMessage
{
    public MyMessageType messageType;
    public int value;
    public string msg;
    public PlayerIndex from;

    public MyMessage(MyMessageType type)
    {
        this.messageType = type;
        this.value = 0;
        this.msg = "";
        this.from = GlobalVariables.PlayerIndex;
    }

    public MyMessage(MyMessageType type, int value, string msg)
    {
        this.messageType = type;
        this.value = value;
        this.msg = msg;
        this.from = GlobalVariables.PlayerIndex;
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
            }
            else
            {
                GameFlowControl.SendMessageToPlayer(PlayerIndex.Player1, new MyMessage(MyMessageType.OnClientConnect));
            }
        }
    }


    public void SendToAll(MyMessage msg)
    {
        if (isLocalPlayer)
        {
            CmdServer(PlayerIndex.All, msg);
        }
    }


    public void SendToPlayer(PlayerIndex playerIndex, MyMessage msg)
    {
        if (isLocalPlayer)
        {
            CmdServer(playerIndex, msg);
        }
    }


    [Command]
    private void CmdServer(PlayerIndex playerIndex, MyMessage msg)
    {
        RpcPlayers(playerIndex, msg);
    }


    [ClientRpc]
    private void RpcPlayers(PlayerIndex playerIndex, MyMessage msg)
    {
        GameFlowControl.ReceiveMessage(playerIndex, msg);
    }


}
