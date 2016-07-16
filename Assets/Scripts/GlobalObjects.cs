using UnityEngine;
using System.Collections;

public static class GlobalObjects  {

    private static NetworkMessageControl networkMessage;
    public static NetworkMessageControl NetworkMessage
    {
        get { return networkMessage; }
    }

    private static PlayerIndexControl indexControl;
    public static PlayerIndexControl IndexControl
    {
        get { return indexControl; }
    }

    public static void SetMessageControl(NetworkMessageControl control)
    {
        networkMessage = control;
    }

    public static void SetIndexControl(PlayerIndexControl control)
    {
        indexControl = control; 
    }

}
