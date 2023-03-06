using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Painting : MonoBehaviour
{
    public CustomCursor cursor;
    private Camera cam;
    public Slider radiusSlider;

    public int radius;

    public Color redColor;
    public Color blueColor;
    public Color greenColor;
    public Color currColor;

    public Texture2D tex;
    public Renderer rend;

    private void OnEnable()
    {
        StartCoroutine(MotherShipStory.instance.PaintingCommentary());
        MotherShipSubTitles.instance.MainMenuSubT(2);

    }
    private void OnDisable()
    {
        StopCoroutine(MotherShipStory.instance.PaintingCommentary());

    }

    void Start()
    {
        foreach (GameObject go in PlayerSpawning.instance.players)
        {
            if (go != null)
            {
                go.GetComponent<PlayerInput>().SwitchCurrentActionMap("Painting");
                go.GetComponent<PlayerInput>().defaultActionMap = "Painting";
                go.GetComponent<PlayerPainting>().enabled = true;
            }
        }

        cam = GetComponent<Camera>();
        currColor = Color.white;

        //ChangeRadiusSize();
        tex = rend.material.mainTexture as Texture2D;
    }

    
    public void SetTextureColor()
    {
        Color[] checkColors = tex.GetPixels();

        for (int i = 0; i < checkColors.Length; i++)
        {
            //print(checkColors[i].linear);
            if (checkColors[i] == Color.white)
            {
                checkColors[i] = Color.green;
                print("there is WHITE");
                //return;
            }
            
        }
        if (Application.isEditor)
        {
            //for unity
            System.IO.File.WriteAllBytes("Assets\\coloredPng.png", tex.EncodeToPNG());
        }
        else
        {
            //for executable
            //System.IO.File.WriteAllBytes(Application.streamingAssetsPath + "/coloredPng.png", tex.EncodeToPNG());
            string filepath = Application.dataPath.Substring(0, Application.dataPath.Length - 23);
            System.IO.File.WriteAllBytes(filepath + "/coloredPng.png", tex.EncodeToPNG());
        }

        print("aply");
    }
    
    public void ClearTexture()
    {
        Color[] resetColors = tex.GetPixels();

        for (int i = 0; i < resetColors.Length; i++)
        {
            resetColors[i] = Color.white;
        }

        tex.SetPixels(resetColors);
        tex.Apply();
    }
}
