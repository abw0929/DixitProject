using UnityEngine;
using System.Collections;

public class TextureControl : MonoBehaviour {


    [SerializeField]
    private Texture2D[] cardTextures;


    private static TextureControl instance;

    private void Awake()
    {
        instance = GetComponent<TextureControl>();
    }


    public static Texture2D GetCardTexture(int index)
    {
        return instance.InnerGetCardTexture(index);
    }


    private Texture2D InnerGetCardTexture(int index)
    {
        return cardTextures[index];
    }

}
