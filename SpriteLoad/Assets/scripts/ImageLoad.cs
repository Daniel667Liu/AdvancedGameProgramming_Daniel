using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ImageLoad : MonoBehaviour
{
    public SpriteRenderer[] WearingParts;
    public Sprite[] wearingSprites;
    private string[] m_ImagePath;
    public Vector2 spritesPivot;
    // Start is called before the first frame update
    void Start()
    {
        wearingSprites = new Sprite[WearingParts.Length];
        m_ImagePath = new string[WearingParts.Length];
    }

    // Update is called once per frame
    void Update()
    {
    
    }

    public void ButtonFunc() 
    {
        FindPath();
        FillSprites();
        DressUp();
    }
    public void DressUp() 
    {
        for (int i = 0; i < WearingParts.Length; i++) 
        {
            WearingParts[i].sprite = wearingSprites[i];
        }
    }

    public void FindPath() 
    {
        for (int i = 0; i < WearingParts.Length; i++) 
        {
            string path = WearingParts[i].gameObject.name;
            m_ImagePath[i] = Application.streamingAssetsPath +"/"+ path+".png";
        }
    }

    public Sprite CreateSprite(string imgPath) 
    {
        byte[] pngByte = System.IO.File.ReadAllBytes(imgPath);
        Texture2D tex2D = new Texture2D(2, 2);
        tex2D.LoadImage(pngByte);
        //warning:pay attention to pivot setup
        Sprite sprite = Sprite.Create(tex2D, new Rect(0f, 0f, tex2D.width, tex2D.height), new Vector2(0.5f, 0.5f), 100.0f);
        return sprite;
    }

    public void FillSprites()
    {
        for (int i = 0; i < WearingParts.Length; i++) 
        {
            wearingSprites[i] = CreateSprite(m_ImagePath[i]);
        }
    }

    public void ResetCharacter() 
    {
        foreach (SpriteRenderer renderer in WearingParts) 
        {
            renderer.sprite = null;
        }
    }
}
