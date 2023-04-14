using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateMiniMap : MonoBehaviour
{
    public RawImage smoothMap;
    private void Start()
    {
        
        CreateMap();
    }
    public void CreateMap()
    {
        byte[] fileData;

        if (Application.isEditor)
        {
            //for unity
            fileData = System.IO.File.ReadAllBytes("Assets\\SmoothMapGeneration.png");
        }
        else
        {
            //for executable
            //fileData = System.IO.File.ReadAllBytes(Application.streamingAssetsPath + "/coloredPng.png");
            string filepath = Application.dataPath.Substring(0, Application.dataPath.Length - 23);
            fileData = System.IO.File.ReadAllBytes(filepath + "/SmoothMapGeneration.png");
        }


        Texture2D tex = new Texture2D(1000, 1000);
        tex.LoadImage(fileData);
        smoothMap.texture = tex;
    }
}
