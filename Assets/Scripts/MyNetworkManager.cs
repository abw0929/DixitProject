using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MyNetworkManager : NetworkManager {


    [SerializeField]
    private Text infoText;

    [SerializeField]
    private GameObject resetButton;

    [SerializeField]
    private Text addressText;

    [SerializeField]
    private GameObject[] UIs;



    private void Start()
    {
        GlobalObjects.SetNetworkManager(this);
        GameFlowControl.Reset();
        GlobalVariables.Reset();
        infoText.text = Network.player.ipAddress;
    }


    public void StartListening()
    {
        StartHost();

        SetUIVis(false);
        infoText.text = Network.player.ipAddress + "\nWaiting...";
        resetButton.SetActive(true);

        GameFlowControl.State = GameStates.Waiting;
    }


    public void ConnectTo()
    {
        StartClient();
        client.Connect(addressText.text, 7777);

        SetUIVis(false);
        infoText.text = "Waiting...";
        resetButton.SetActive(true);

        GameFlowControl.State = GameStates.Waiting;
    }



    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }


    public override void OnServerDisconnect(NetworkConnection conn)
    {
        base.OnServerDisconnect(conn);
        GameFlowControl.SendResetAllClients();
    }


    public override void OnClientError(NetworkConnection conn, int errorCode)
    {
        base.OnClientError(conn, errorCode);
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }


    public override void OnServerError(NetworkConnection conn, int errorCode)
    {
        base.OnServerError(conn, errorCode);
        GameFlowControl.SendResetAllClients();
    }


    public override void OnStopHost()
    {
        base.OnStopHost();
        GameFlowControl.SendResetAllClients();
    }


    public override void OnClientSceneChanged(NetworkConnection conn)
    {

    }


    public override void OnServerSceneChanged(string sceneName)
    {

    }


    public void SetUIVis(bool vis)
    {
        foreach (GameObject obj in UIs)
            obj.SetActive(vis);
    }


    public void Reset()
    {
        StopHost();
        StopServer();
        NetworkServer.Reset();
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

}
