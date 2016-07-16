using UnityEngine;
using System.Collections;

public class TextureControl : MonoBehaviour {


    [SerializeField]
    private Sprite[] cardTexture;


    private static TextureControl instance;

    private void Awake()
    {
        instance = GetComponent<TextureControl>();
    }


    public static Sprite GetCardTexture(int index)
    {
        return instance.InnerGetCardTexture(index);
    }


    private Sprite InnerGetCardTexture(int index)
    {
        return cardTexture[index];
    }

}
