using UnityEngine;
using System.Collections;
using UnityEngine.UI;

enum PreviewImageState
{
    Idle = 0,
    ZoomIn = 1,
    Viewing = 2,
    ZoomOut = 3,
    FlyingOut = 4,
}

public class PreviewImageControl : MonoBehaviour {

    private const int IMAGE_FULL_X = 0;
    private const int IMAGE_FULL_Y = 20;
    private const int IMAGE_FULL_W = 800;
    private const int IMAGE_FULL_H = 1070;

    private const int IMAGE_AWAY_Y = 1250;

    [SerializeField]
    private int animationFrames = 30;

    private MyCardIndex smallCardIndex = MyCardIndex.MyCard0;
    private CardType previewCard = CardType.Card00;
    private PreviewImageState state = PreviewImageState.Idle;

    private Image previewImage;


    private void Awake () {
        previewImage = GetComponent<Image>();
        previewImage.enabled = false;
	}


    public void Play(MyCardIndex cardIndex, CardType cardType)
    {
        if (state != PreviewImageState.Idle)
            return;

        smallCardIndex = cardIndex;
        previewCard = cardType;

        StopAllCoroutines();
        StartCoroutine(PreviewRoutine());
    }


    public void Close()
    {
        if (state != PreviewImageState.Viewing)
            return;

        StopAllCoroutines();
        StartCoroutine(CloseRoutine());
    }


    public void FlyOut()
    {
        if (state != PreviewImageState.Viewing)
            return;

        StopAllCoroutines();
        StartCoroutine(FlyOutRoutine());
    }


    private IEnumerator PreviewRoutine()
    {
        float xStep = (IMAGE_FULL_X - GlobalVariables.SMALL_CARD_POS[(int)smallCardIndex].x) / animationFrames;
        float yStep = (IMAGE_FULL_Y - GlobalVariables.SMALL_CARD_POS[(int)smallCardIndex].y) / animationFrames;
        float wStep = (IMAGE_FULL_W - GlobalVariables.SMALL_CARD_SIZE_W) / animationFrames;
        float hStep = (IMAGE_FULL_H - GlobalVariables.SMALL_CARD_SIZE_H) / animationFrames;

        previewImage.overrideSprite = TextureControl.GetCardTexture((int)previewCard);
        previewImage.enabled = true;
        state = PreviewImageState.ZoomIn;

        for (int i = 0; i <= animationFrames; i++)
        {
            previewImage.rectTransform.localPosition = new Vector3(GlobalVariables.SMALL_CARD_POS[(int)smallCardIndex].x + xStep * i,
                                                                   GlobalVariables.SMALL_CARD_POS[(int)smallCardIndex].y + yStep * i, 0);
            previewImage.rectTransform.sizeDelta = new Vector2(GlobalVariables.SMALL_CARD_SIZE_W + wStep * i,
                                                               GlobalVariables.SMALL_CARD_SIZE_H + hStep * i);
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }

        state = PreviewImageState.Viewing;
    }


    private IEnumerator CloseRoutine()
    {
        float xStep = (GlobalVariables.SMALL_CARD_POS[(int)smallCardIndex].x - IMAGE_FULL_X) / animationFrames;
        float yStep = (GlobalVariables.SMALL_CARD_POS[(int)smallCardIndex].y - IMAGE_FULL_Y) / animationFrames;
        float wStep = (GlobalVariables.SMALL_CARD_SIZE_W - IMAGE_FULL_W) / animationFrames;
        float hStep = (GlobalVariables.SMALL_CARD_SIZE_H - IMAGE_FULL_H) / animationFrames;

        state = PreviewImageState.ZoomOut;

        for (int i = 0; i <= animationFrames; i++)
        {
            previewImage.rectTransform.localPosition = new Vector3(IMAGE_FULL_X + xStep * i,
                                                                   IMAGE_FULL_Y + yStep * i, 0);
            previewImage.rectTransform.sizeDelta = new Vector2(IMAGE_FULL_W + wStep * i,
                                                               IMAGE_FULL_H + hStep * i);
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }

        previewImage.enabled = false;
        state = PreviewImageState.Idle;
    }


    private IEnumerator FlyOutRoutine()
    {
        float yStep = (IMAGE_AWAY_Y - IMAGE_FULL_Y) / animationFrames;

        state = PreviewImageState.FlyingOut;

        for (int i = 0; i <= animationFrames; i++)
        {
            previewImage.rectTransform.localPosition = new Vector3(IMAGE_FULL_X,
                                                                   IMAGE_FULL_Y + yStep * i, 0);
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }

        previewImage.enabled = false;
        state = PreviewImageState.Idle;
    }



}
