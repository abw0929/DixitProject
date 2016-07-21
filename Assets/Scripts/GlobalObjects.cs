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

    private static MyNetworkManager myNetworkManager;
    public static MyNetworkManager NetworkManager
    {
        get { return myNetworkManager; }
    }


    public static void SetMessageControl(NetworkMessageControl control)
    {
        networkMessage = control;
    }

    public static void SetIndexControl(PlayerIndexControl control)
    {
        indexControl = control; 
    }

    public static void SetNetworkManager(MyNetworkManager manager)
    {
        myNetworkManager = manager;
    }

}
