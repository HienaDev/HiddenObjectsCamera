using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.IO;
using System;

public class RenderTextureCapture : MonoBehaviour
{
    [SerializeField] private RenderTexture captureTexture;
    [SerializeField] private GameObject image;
    [SerializeField] private bool saveToPc;
    [SerializeField] private GameObject photoPolaroidPrefab;
    [SerializeField] private Transform polaroidPrefabParent;

    [SerializeField] private string newFolder = "GrandmaGameObjects";

    public List<Sprite> sprites = new List<Sprite>();




    public void ExportPhoto(string type)
    {
        
       toTexture2D(captureTexture);

    }

    private Texture2D toTexture2D(RenderTexture rTex)
    {
        Texture2D tex = new Texture2D(1600, 900);//, TextureFormat.RGB24, false, true);
        RenderTexture.active = rTex;
        Rect rect = new Rect(0, 0, 1600, 900);
        tex.ReadPixels(rect, 0, 0);
        tex.Apply();

        sprites.Add(Sprite.Create(tex, rect, Vector2.zero));
        GameObject polaroid = Instantiate(photoPolaroidPrefab, polaroidPrefabParent);
        polaroid.GetComponent<Image>().sprite = sprites.Last();
        Debug.Log("sprite added");
        return tex;
    }
}
