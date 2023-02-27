using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothMapGeneration : MonoBehaviour
{
    public static SmoothMapGeneration instance;
    // Terrain
    private Terrain terrain;
    private TerrainData terData;

    // Textures used to read/write
    private Texture2D tex;
    private Texture2D redTex;
    private Texture2D greenTex;
    private Texture2D blueTex;
    private int mapResolution = 1000;


    public void Start()
    {
        terrain = GetComponent<Terrain>();
        byte[] fileData;

        terData = terrain.terrainData;

        if (Application.isEditor)
        {
            //for unity  

            //Get Red Tex
            fileData = System.IO.File.ReadAllBytes("Assets\\Textures\\RedGroundTex.png");
            redTex = new Texture2D(mapResolution, mapResolution);
            redTex.LoadImage(fileData);

            //Get Green Tex
            fileData = System.IO.File.ReadAllBytes("Assets\\Textures\\GreenGroundTex.png");
            greenTex = new Texture2D(mapResolution, mapResolution);
            greenTex.LoadImage(fileData);

            //Get Blue Tex
            fileData = System.IO.File.ReadAllBytes("Assets\\Textures\\BlueGroundTex.png");
            blueTex = new Texture2D(mapResolution, mapResolution);
            blueTex.LoadImage(fileData);

        }
        else
        {
            //for executable

            //Get Red Tex
            fileData = System.IO.File.ReadAllBytes(Application.streamingAssetsPath + "/RedGroundTex.png");
            redTex = new Texture2D(mapResolution, mapResolution);
            redTex.LoadImage(fileData);

            //Get Green Tex
            fileData = System.IO.File.ReadAllBytes(Application.streamingAssetsPath + "/GreenGroundTex.png");
            greenTex = new Texture2D(mapResolution, mapResolution);
            greenTex.LoadImage(fileData);

            //Get Blue Tex
            fileData = System.IO.File.ReadAllBytes(Application.streamingAssetsPath + "/BlueGroundTex.png");
            blueTex = new Texture2D(mapResolution, mapResolution);
            blueTex.LoadImage(fileData);
        }

        CreateMap();
    }

    public void CreateMap()
    {
        byte[] fileData;

        if (Application.isEditor)
        {
            //for unity
            fileData = System.IO.File.ReadAllBytes("Assets\\coloredPng.png");
        }
        else
        {
            //for executable
            //fileData = System.IO.File.ReadAllBytes(Application.streamingAssetsPath + "/coloredPng.png");
            string filepath = Application.dataPath.Substring(0, Application.dataPath.Length - 23);
            fileData = System.IO.File.ReadAllBytes(filepath + "/coloredPng.png");
        }

        tex = new Texture2D(mapResolution, mapResolution);
        tex.LoadImage(fileData);
        terData = terrain.terrainData;

        Color pixelColor;
        Color biomeColor;


        GridBreakdown.instance.GenerateGrid();
        Texture2D biomeMap = new Texture2D(mapResolution, mapResolution, TextureFormat.RGB24, false);
        for (int y = 0; y < mapResolution; y++)
        {
            for (int x = 0; x < mapResolution; x++)
            {
                Cell c = GridBreakdown.instance.FindPixelsCell(x, y);

                int randX = Random.Range(0, redTex.width);
                int randY = Random.Range(0, redTex.height);
                pixelColor = tex.GetPixel(x, y);

                if (pixelColor.r == Mathf.Max(pixelColor.r, pixelColor.g, pixelColor.b)) // Red
                {
                    biomeColor = redTex.GetPixel(randX, randY);
                    c.possibleBiome[Biome.Red] += 1;
                }
                else if (pixelColor.g == Mathf.Max(pixelColor.r, pixelColor.g, pixelColor.b)) // Green
                {
                    biomeColor = greenTex.GetPixel(randX, randY);
                    c.possibleBiome[Biome.Green] += 1;
                }
                else if (pixelColor.b == Mathf.Max(pixelColor.r, pixelColor.g, pixelColor.b)) // Blue
                {
                    biomeColor = blueTex.GetPixel(randX, randY);
                    c.possibleBiome[Biome.Blue] += 1;
                }
                else // pixel still white, set to green
                {
                    biomeColor = greenTex.GetPixel(randX, randY);
                    c.possibleBiome[Biome.Green] += 1;
                }



                biomeMap.SetPixel(x, y, biomeColor);
            }
        }

        biomeMap.Apply();
        terData.terrainLayers[0].diffuseTexture = biomeMap;
        
        

        if (Application.isEditor)
        {
            System.IO.File.WriteAllBytes("Assets\\SmoothMapGeneration.png", biomeMap.EncodeToPNG());
        }
        else
        {
            string filepath = Application.dataPath.Substring(0, Application.dataPath.Length - 23);
            System.IO.File.WriteAllBytes(filepath + "/SmoothMapGeneration.png", biomeMap.EncodeToPNG());
            //System.IO.File.WriteAllBytes(Application.streamingAssetsPath + "/SmoothMapGeneration.png", biomeMap.EncodeToPNG());

        }
        GridBreakdown.instance.SetCellsBiome();
    }
}
