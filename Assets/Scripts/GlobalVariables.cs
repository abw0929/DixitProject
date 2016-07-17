using UnityEngine;
using System.Collections;


public enum PlayerNumberType
{
    ThreePlayers = 3,
    FourPlayers = 4,
    FivePlayers = 5,
    SixPlayers = 6
}

public enum PlayerIndex
{
    None = -1,
    All = 0,
    Player1 = 1,
    Player2 = 2,
    Player3 = 3,
    Player4 = 4,
    Player5 = 5,
    Player6 = 6
}

public enum MyCardType
{
    MyCardSix = 6,
    MyCardSeven = 7
}

public enum MyCardIndex
{
    MyCard0 = 0,
    MyCard1 = 1,
    MyCard2 = 2,
    MyCard3 = 3,
    MyCard4 = 4,
    MyCard5 = 5,
    MyCard6 = 6,
}

public enum SendCardType
{
    SendCardOne = 1,
    SendCardTwo = 2
}

public enum PublicCardType
{
    PublicCardFour = 4,
    PublicCardFive = 5,
    PublicCardSix = 6
}

public enum CardType
{
    Card00 = 0, Card01 = 1, Card02 = 2, Card03 = 3, Card04 = 4,
    Card05 = 5, Card06 = 6, Card07 = 7, Card08 = 8, Card09 = 9,
    Card10 = 10, Card11 = 11, Card12 = 12, Card13 = 13, Card14 = 14,
    Card15 = 15, Card16 = 16, Card17 = 17, Card18 = 18, Card19 = 19,
    Card20 = 20, Card21 = 21, Card22 = 22, Card23 = 23, Card24 = 24,
    Card25 = 25, Card26 = 26, Card27 = 27, Card28 = 28, Card29 = 29,
    Card30 = 30, Card31 = 31, Card32 = 32, Card33 = 33, Card34 = 34,
    Card35 = 35, Card36 = 36, Card37 = 37, Card38 = 38, Card39 = 39,
    Card40 = 40, Card41 = 41, Card42 = 42, Card43 = 43, Card44 = 44,
    Card45 = 45, Card46 = 46, Card47 = 47, Card48 = 48, Card49 = 49,
    Card50 = 50, Card51 = 51, Card52 = 52, Card53 = 53, Card54 = 54,
    Card55 = 55, Card56 = 56, Card57 = 57, Card58 = 58, Card59 = 59,
    Card60 = 60, Card61 = 61, Card62 = 62, Card63 = 63, Card64 = 64,
    Card65 = 65, Card66 = 66, Card67 = 67, Card68 = 68, Card69 = 69,
    Card70 = 70, Card71 = 71, Card72 = 72, Card73 = 73, Card74 = 74,
    Card75 = 75, Card76 = 76, Card77 = 77, Card78 = 78, Card79 = 79,
    Card80 = 80, Card81 = 81, Card82 = 82, Card83 = 83,
}

public static class GlobalVariables {

    private static PlayerNumberType playerType = PlayerNumberType.ThreePlayers;
    public static PlayerNumberType PlayerType
    {
        get { return playerType; }
    }

    private static int playerNum = 3;
    public static int PlayerNum
    {
        get { return playerNum; }
    }

    private static PlayerIndex playerIndex = PlayerIndex.None;
    public static PlayerIndex PlayerIndex
    {
        get { return playerIndex; }
    }

    private static MyCardType myCardType = MyCardType.MyCardSeven;
    public static MyCardType MyCard
    {
        get { return myCardType; }
    }

    private static int myCardNum = 7;
    public static int MyCardNum
    {
        get { return myCardNum; }
    }

    private static SendCardType sendCardType = SendCardType.SendCardTwo;
    public static SendCardType SendCard
    {
        get { return sendCardType; }
    }

    private static int sendCardNum = 2;
    public static int SendCardNum
    {
        get { return sendCardNum; }
    }

    private static PublicCardType publicCardType = PublicCardType.PublicCardFive;
    public static PublicCardType PublicCard
    {
        get { return publicCardType; }
    }

    private static int publicCardNum = 5;
    public static int PublicCardNum
    {
        get { return publicCardNum; }
    }

    public const int TOTAL_CARD_NUM = 84;

    public const float SMALL_CARD_SIZE_W = 205;
    public const float SMALL_CARD_SIZE_H = 304;

    public static readonly Vector2[] SMALL_CARD_POS = new Vector2[]
    {
        new Vector2 ( -294f, 403f ), new Vector2 ( 0f, 403f ), new Vector2 ( 294f, 403f ),
        new Vector2 ( -294f, 30f ), new Vector2 ( 0f, 30f ), new Vector2 ( 294f, 30f ),
        new Vector2 ( -294f, -354f )
    };


    public static void SetPlayerNum(PlayerNumberType type)
    {
        playerNum = (int)type;
        playerType = type;

        if(type == PlayerNumberType.ThreePlayers)
        {
            myCardNum = 7;
            publicCardNum = 5;
            sendCardNum = 2;
            myCardType = MyCardType.MyCardSeven;
            publicCardType = PublicCardType.PublicCardFive;
            sendCardType = SendCardType.SendCardTwo;
        }
        else
        {
            myCardNum = 6;
            publicCardNum = playerNum;
            sendCardNum = 1;
            myCardType = MyCardType.MyCardSix;
            publicCardType = (PublicCardType)playerNum;
            sendCardType = SendCardType.SendCardOne;
        }

        Debug.Log("Current PlayerNumber : " + type);
    }


    public static void SetPlayerIndex(PlayerIndex index)
    {
        playerIndex = index;
        Debug.Log("PlayerIndex Set To : " + index);
    }

}
