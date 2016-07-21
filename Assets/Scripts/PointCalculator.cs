using UnityEngine;
using System.Collections.Generic;

public class PointCalculator {


    private static int[] currentPoints;
    public static int[] CurrentPoints
    {
        get { return currentPoints; }
    }


    public void SetPlayerNum(int num)
    {
        currentPoints = new int[num];
    }
	

    public void ResetPoints()
    {
        for(int i = 0; i < GlobalVariables.PlayerNum; i++)
        {
            currentPoints[i] = 0;
        }
    }


    public void GetPoints(int[] showDeck, int[] cardGiver, int title, Dictionary<PlayerIndex, int> votes,
        out int[] points, out int[] getPoints)
    {
        int[] myShowDeck = new int[GlobalVariables.PublicCardNum];
        int[] mycardGiver = new int[GlobalVariables.PublicCardNum];
        showDeck.CopyTo(myShowDeck, 0);
        cardGiver.CopyTo(mycardGiver, 0);

        for (int i = GlobalVariables.PublicCardNum - 1; i > 0; i--)
        {
            for(int j = 0; j < i; j++)
            {
                if(mycardGiver[j] > mycardGiver[j+1])
                {
                    int temp = mycardGiver[j];
                    mycardGiver[j] = mycardGiver[j + 1];
                    mycardGiver[j + 1] = temp;
                    temp = myShowDeck[j];
                    myShowDeck[j] = myShowDeck[j + 1];
                    myShowDeck[j + 1] = temp;
                }
            }
        }

        getPoints = new int[GlobalVariables.PlayerNum];
        int[] extraPoints = new int[GlobalVariables.PlayerNum];
        int[] voteForTitle = new int[GlobalVariables.PlayerNum];

        int guessRightCount = 0;
        int guessWrongCount = 0;

        foreach(KeyValuePair<PlayerIndex, int> vote in votes)
        {
            if (vote.Value == title)
            {
                guessRightCount++;
                voteForTitle[(int)vote.Key - 1] = 1;
            }
            else
            {
                guessWrongCount++;
                for (int i = 0; i < GlobalVariables.PublicCardNum; i++)
                {
                    if(vote.Value == myShowDeck[i])
                    {
                        extraPoints[mycardGiver[i] - 1]++;
                    }
                }
            }
        }

        if (guessRightCount == GlobalVariables.PlayerNum - 1 || guessWrongCount == GlobalVariables.PlayerNum - 1)
        {
            for (int i = 0; i < GlobalVariables.PlayerNum; i++)
            {
                if ((i + 1) != GameFlowControl.TitlePlayer)
                {
                    getPoints[i] = 2;
                    currentPoints[i] += getPoints[i];
                }
            }
        }
        else
        {
            for (int i = 0; i < GlobalVariables.PlayerNum; i++)
            {
                if (voteForTitle[i] == 1 || (i + 1) == GameFlowControl.TitlePlayer)
                {
                    getPoints[i] = 3 + extraPoints[i];
                    currentPoints[i] += getPoints[i];
                }
            }
        }

        points = currentPoints;
    }



}
