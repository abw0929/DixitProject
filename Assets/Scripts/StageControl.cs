using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StageControl : MonoBehaviour {

    [SerializeField]
    private Text connectionInfoText;

    [SerializeField]
    private GameObject resetButton;

    [SerializeField]
    private GameObject inputField;

    [SerializeField]
    private Text inputFieldText;

    [SerializeField]
    private GameObject inputNo;

    [SerializeField]
    private GameObject inputYes;

    [SerializeField]
    private GameObject[] cardImages;

    [SerializeField]
    private GameObject startButton;

    [SerializeField]
    private GameObject infoText;

    [SerializeField]
    private GameObject titleText;

    [SerializeField]
    private PreviewImageControl previewImage;


    private CardType[] cardImageTypes;


    private void Awake()
    {
        cardImageTypes = new CardType[7];
        SetInputVis(false);
        SetAllCardVis(false);
        SetStartButtonVis(false);
        SetInfoText(false, "");
        SetTitleText(false, "");
    }


    public void SetConnInfoVis(bool vis)
    {
        connectionInfoText.enabled = vis;
        resetButton.SetActive(vis);
    }


    public void SetInputVis(bool vis)
    {
        inputFieldText.text = "";

        inputField.SetActive(vis);
        inputYes.SetActive(vis);
        inputNo.SetActive(vis);
    }


    public void SetYesNoVis(bool vis)
    {
        inputYes.SetActive(vis);
        inputNo.SetActive(vis);
    }


    public void SetAllCardVis(bool vis)
    {
        foreach(GameObject card in cardImages)
        {
            card.SetActive(vis);
        }
    }


    public void SetCardVis(int index, bool vis)
    {
        cardImages[index].SetActive(vis);
    }


    public void SetCardImage(int index, CardType type)
    {
        cardImages[index].GetComponent<Image>().overrideSprite = TextureControl.GetCardTexture((int)type);
        cardImageTypes[index] = type;
    }


    public void PreviewImageOpen(int cardIndex)
    {
        previewImage.Play((MyCardIndex)cardIndex, cardImageTypes[cardIndex]);
    }


    public void PreviewImageFlyOut()
    {
        previewImage.FlyOut();
    }


    public void PreviewImageClose()
    {
        previewImage.Close();
    }


    public void SetStartButtonVis(bool vis)
    {
        startButton.SetActive(vis);
    }


    public void SetInfoText(bool vis, string msg)
    {
        infoText.SetActive(vis);
        infoText.GetComponent<Text>().text = msg;
    }


    public void SetTitleText(bool vis, string msg)
    {
        titleText.SetActive(vis);
        titleText.GetComponent<Text>().text = msg;
    }


    public string GetInputText()
    {
        string msg = inputFieldText.text;
        inputFieldText.text = "";

        return msg;
    }

}
