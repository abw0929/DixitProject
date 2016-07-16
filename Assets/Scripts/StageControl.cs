using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StageControl : MonoBehaviour {

    [SerializeField]
    private GameObject inputField;

    [SerializeField]
    private GameObject[] cardImages;

    [SerializeField]
    private PreviewImageControl previewImage;


    private CardType[] cardImageTypes;


    private void Awake()
    {
        cardImageTypes = new CardType[7];
        SetInputVis(false);
        SetAllCardVis(false);
    }


    public void SetInputVis(bool vis)
    {
        inputField.SetActive(vis);
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

}
