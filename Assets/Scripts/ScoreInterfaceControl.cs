using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreInterfaceControl : MonoBehaviour {


    [SerializeField]
    private GameObject[] playerObjects;

    [SerializeField]
    private Text[] playerNameTexts;

    [SerializeField]
    private Text[] playerScoreTexts;

    [SerializeField]
    private Text[] playerGetPointTexts;


    void Start () {
        GlobalObjects.SetScoreInterface(this);
	}


    public void SetAllVis(bool vis)
    {
        foreach(GameObject obj in playerObjects)
        {
            obj.SetActive(vis);
        }
    }


    public void SetVis(int index, bool vis)
    {
        playerObjects[index].SetActive(vis);
    }


    public void SetPlayerName(int index)
    {
        playerNameTexts[index].gameObject.SetActive(true);
    }


    public void SetScore(int index, int score)
    {
        playerScoreTexts[index].text = score.ToString();
    }


    public void SetGetPoint(int index, int score, bool vis)
    {
        playerGetPointTexts[index].text = "+" + score.ToString();
        playerGetPointTexts[index].gameObject.SetActive(vis);
    }
	
	
}
