using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MyNetworkManager : NetworkManager {


    [SerializeField]
    private Text infoText;

    [SerializeField]
    private Text addressText;

    [SerializeField]
    private GameObject[] UIs;



    private void Start()
    {
        infoText.text = networkAddress;
    }


    public void StartListening()
    {
        StartHost();

        SetUIVis(false);
        infoText.text = "Waiting...";
    }


    public void ConnectTo()
    {
        StartClient();
        client.Connect("192.168.1.103", 7777);

        SetUIVis(false);
        infoText.text = "Waiting...";
    }


    public override void OnClientConnect(NetworkConnection conn)
    {
        if(NetworkServer.connections.Count >= 2)
            infoText.enabled = false;

        base.OnClientConnect(conn);
    }


    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);
        GameFlowControl.SendResetAllClients();
    }


    public override void OnServerDisconnect(NetworkConnection conn)
    {
        base.OnServerDisconnect(conn);
        GameFlowControl.SendResetAllClients();
    }


    public override void OnStopHost()
    {
        base.OnStopHost();
        GameFlowControl.SendResetAllClients();
    }


    public void SetUIVis(bool vis)
    {
        foreach (GameObject obj in UIs)
            obj.SetActive(vis);
    }


}
