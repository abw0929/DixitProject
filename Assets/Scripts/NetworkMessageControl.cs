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
    ResetAllClient = 99,
}

[Serializable]
public struct MyMessage
{
    public int messageType;
    public int value;
    public int[] values;
    public string msg;
    public int from;

    public MyMessage(MyMessageType type)
    {
        this.messageType = (int)type;
        this.value = 0;
        this.values = new int[0];
        this.msg = "";
        this.from = (int)GlobalVariables.PlayerIndex;
    }

    public MyMessage(MyMessageType type, int value, string msg)
    {
        this.messageType = (int)type;
        this.value = value;
        this.values = new int[0];
        this.msg = msg;
        this.from = (int)GlobalVariables.PlayerIndex;
    }

    public MyMessage(MyMessageType type, int[] values, string msg)
    {
        this.messageType = (int)type;
        this.value = 0;
        this.values = values;
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
