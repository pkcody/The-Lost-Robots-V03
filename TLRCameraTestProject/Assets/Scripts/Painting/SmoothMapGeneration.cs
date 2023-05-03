using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;


public class SmoothMapGeneration : MonoBehaviour
{
    public static SmoothMapGeneration instance;
    // Terrain
    private Terrain terrain;
    private TerrainData terData;

    //tracking
    public BiomeTracker btr;

    public NavMeshSurface surface;


    // Textures used to read/write
    private Texture2D tex;
    private Texture2D redTex;
    private Texture2D greenTex;
    private Texture2D tealTex;
    private Texture2D blueTex;
    private int mapResolution = 1000;
    private int texRes = 1000;

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
            redTex = new Texture2D(texRes, texRes);
            redTex.LoadImage(fileData);

            //Get Green Tex
            fileData = System.IO.File.ReadAllBytes("Assets\\Textures\\GreenGroundTex.png");
            greenTex = new Texture2D(texRes, texRes);
            greenTex.LoadImage(fileData);

            //Get Teal Tex
            fileData = System.IO.File.ReadAllBytes("Assets\\Textures\\TealGroundTex.png");
            tealTex = new Texture2D(texRes, texRes);
            tealTex.LoadImage(fileData);

            //Get Blue Tex
            fileData = System.IO.File.ReadAllBytes("Assets\\Textures\\BlueGroundTex.png");
            blueTex = new Texture2D(texRes, texRes);
            blueTex.LoadImage(fileData);


        }
        else
        {
            //for executable

            //Get Red Tex
            fileData = System.IO.File.ReadAllBytes(Application.streamingAssetsPath + "/RedGroundTex.png");
            redTex = new Texture2D(texRes, texRes);
            redTex.LoadImage(fileData);

            //Get Green Tex
            fileData = System.IO.File.ReadAllBytes(Application.streamingAssetsPath + "/GreenGroundTex.png");
            greenTex = new Texture2D(texRes, texRes);
            greenTex.LoadImage(fileData);

            //Get Teal Tex
            fileData = System.IO.File.ReadAllBytes(Application.streamingAssetsPath + "/TealGroundTex.png");
            tealTex = new Texture2D(texRes, texRes);
            tealTex.LoadImage(fileData);

            //Get Blue Tex
            fileData = System.IO.File.ReadAllBytes(Application.streamingAssetsPath + "/BlueGroundTex.png");
            blueTex = new Texture2D(texRes, texRes);
            blueTex.LoadImage(fileData);
        }

        btr = FindObjectOfType<BiomeTracker>();

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
        //tex = new Texture2D(resres, resres);
        tex.LoadImage(fileData);
        terData = terrain.terrainData;

        Color pixelColor;
        Color biomeColor;


        float[,,] map = new float[mapResolution, mapResolution, 4]; //// change this inclused the 4 maps layers


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
                    
                    // Terrain layers
                    map[y, x, 0] = 0f;
                    map[y, x, 1] = 1f;
                    map[y, x, 2] = 0f;
                    map[y, x, 3] = 0f;
                    
                    c.possibleBiome[Biome.Red] += 1;
                }
                else if (pixelColor.g == Mathf.Max(pixelColor.r, pixelColor.g, pixelColor.b) && pixelColor.g < 0.5f) // Green
                {
                    biomeColor = greenTex.GetPixel(randX, randY);
                    
                    map[y, x, 0] = 0f;
                    map[y, x, 1] = 0f;
                    map[y, x, 2] = 1f;
                    map[y, x, 3] = 0f;


                    c.possibleBiome[Biome.Green] += 1;
                }
                else if (pixelColor.g == Mathf.Max(pixelColor.r, pixelColor.g, pixelColor.b) && pixelColor.g > 0.5f) // Teal
                {
                    //print("teal");
                    biomeColor = tealTex.GetPixel(randX, randY);

                    map[y, x, 0] = 0f;
                    map[y, x, 1] = 0f;
                    map[y, x, 2] = 0f;
                    map[y, x, 3] = 1f;


                    c.possibleBiome[Biome.Teal] += 1;
                }
                else if (pixelColor.b == Mathf.Max(pixelColor.r, pixelColor.g, pixelColor.b)) // Blue
                {
                    biomeColor = blueTex.GetPixel(randX, randY);
                    
                    map[y, x, 0] = 1f;
                    map[y, x, 1] = 0f;
                    map[y, x, 2] = 0f;
                    map[y, x, 3] = 0f;


                    c.possibleBiome[Biome.Blue] += 1;
                }
                else // pixel still white, set to blue
                {
                    biomeColor = blueTex.GetPixel(randX, randY);
                    
                    map[y, x, 0] = 1f;
                    map[y, x, 1] = 0f;
                    map[y, x, 2] = 0f;
                    map[y, x, 3] = 0f;


                    c.possibleBiome[Biome.Blue] += 1;
                }

                biomeMap.SetPixel(x, y, biomeColor);
            }
        }

        biomeMap.Apply();

        btr.tex = biomeMap;
        StartCoroutine(btr.CheckBiome());

        terData.SetAlphamaps(0, 0, map);


        if (Application.isEditor)
        {
            System.IO.File.WriteAllBytes("Assets\\SmoothMapGeneration.png", biomeMap.EncodeToPNG());
        }
        else
        {
            string filepath = Application.dataPath.Substring(0, Application.dataPath.Length - 23);
            System.IO.File.WriteAllBytes(filepath + "/SmoothMapGeneration.png", biomeMap.EncodeToPNG());

        }
        GridBreakdown.instance.SetCellsBiome();

        //Update nav try
        surface.BuildNavMesh();
    }
}
