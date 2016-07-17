using UnityEngine;
using System.Collections;

public class ButtonControl : MonoBehaviour {

    [SerializeField]
    private GameFlow gameFlow;

    private int currentClickCard = 0;
	
    public void StartGame()
    {
        GameFlowControl.SendStartGame();
    }


    public void CardButtonClick(int index)
    {
        if(GameFlowControl.State == GameStates.WaitingTitle)
        {
            gameFlow.SelectTitle(index);
            currentClickCard = index;
        }
        else if(GameFlowControl.State == GameStates.WaitingAllCards &&
            (int)GlobalVariables.PlayerIndex != GameFlowControl.TitlePlayer)
        {
            gameFlow.SelectGiveOutCard(index);
            currentClickCard = index;
        }
        else if (GameFlowControl.State == GameStates.WaitingAllVotes &&
            (int)GlobalVariables.PlayerIndex != GameFlowControl.TitlePlayer)
        {
            gameFlow.SelectVoteCard(index);
            currentClickCard = index;
        }
    }


    public void YesButtonClick()
    {
        if (GameFlowControl.State == GameStates.WaitingTitle)
        {
            gameFlow.SendTitle(currentClickCard);
        }
        else if (GameFlowControl.State == GameStates.WaitingAllCards)
        {
            gameFlow.SendGiveCard(currentClickCard);
        }
        else if (GameFlowControl.State == GameStates.WaitingAllVotes)
        {
            gameFlow.SendVoteCard(currentClickCard);
        }
    }


    public void NoButtonClick()
    {
        if (GameFlowControl.State == GameStates.WaitingTitle)
        {
            gameFlow.CloseSelect();
        }
        else if (GameFlowControl.State == GameStates.WaitingAllCards)
        {
            gameFlow.CloseSelect();
        }
        else if (GameFlowControl.State == GameStates.WaitingAllVotes)
        {
            gameFlow.CloseSelect();
        }
    }

}
