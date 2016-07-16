using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerIndexControl : MonoBehaviour {


    private int currentGiveIndex = 1;
    public int CurrentPlayers
    { get { return currentGiveIndex; } }



	void Start () {
        GlobalObjects.SetIndexControl(this);
	}

	
	
    public void SetIndex(PlayerIndex index)
    {
        if(GlobalVariables.PlayerIndex == PlayerIndex.None)
        {
            GlobalVariables.SetPlayerIndex(index);
        }
    }


    public void ResetAll()
    {
        currentGiveIndex = 1;
        GlobalVariables.SetPlayerIndex(PlayerIndex.None);
    }


    public void GiveIndex()
    {
        GameFlowControl.SendMessageToAll(new MyMessage(MyMessageType.SetPlayerIndex, ++currentGiveIndex, ""));
    }


}
