using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBreakdown : MonoBehaviour
{
    public static GridBreakdown instance;

    public static int cellPixelSize = 50;
    public List<Cell> listCells;
    public Cell[,] Grid = new Cell[cellPixelSize, cellPixelSize];
    public int numCellRowsCols;
    public int mapResolution = 1000;
    public float minBiomePercentage = 75;

    public List<Cell> redBiomeCells = new List<Cell>();
    public List<Cell> greenBiomeCells = new List<Cell>();
    public List<Cell> blueBiomeCells = new List<Cell>();
    public List<Cell> mixedBiomeCells = new List<Cell>();


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //#if UNITY_EDITOR
    public void GenerateGrid()
    {
        numCellRowsCols = 1000 / cellPixelSize;

        for (int row = 0; row < numCellRowsCols; row++)
        {
            for (int col = 0; col < numCellRowsCols; col++)
            {
                
                Grid[row, col] = new Cell(row, col);
                //Grid[row, col].biome = GetCellBiome(row, col);
                //print($"{Grid[row, col].row} + {Grid[row, col].col}");
                listCells.Add(Grid[row, col]);
            }
        }

    }


    public Cell FindPixelsCell(int x, int y)
    {
        int row = y / cellPixelSize;
        int col = x / cellPixelSize;
        
        return Grid[row, col];
    }

    public void SetCellsBiome()
    {
        for (int row = 0; row < numCellRowsCols; row++)
        {
            for (int col = 0; col < numCellRowsCols; col++)
            {
                Cell c = Grid[row, col];
                float redCount = c.possibleBiome[Biome.Red];
                float greenCount = c.possibleBiome[Biome.Green];
                float blueCount = c.possibleBiome[Biome.Blue];
                float totalCount = redCount + greenCount + blueCount;

                if (redCount / totalCount >= (minBiomePercentage / 100))
                {
                    c.biome = Biome.Red;
                    redBiomeCells.Add(c);
                }
                else if (greenCount / totalCount >= (minBiomePercentage / 100))
                {
                    c.biome = Biome.Green;
                    greenBiomeCells.Add(c);
                }
                else if (blueCount / totalCount >= (minBiomePercentage / 100))
                {
                    c.biome = Biome.Blue;
                    blueBiomeCells.Add(c);
                }
                else
                {
                    c.biome = Biome.Mixed;
                    mixedBiomeCells.Add(c);
                }
            }
        }

        
        //PrintAllCellBiomes();

        FindObjectOfType<RandomBiomeBasedSpawning>().SpawnBiomeGOs();
    }

    public void PrintAllCellBiomes()
    {
        for (int row = 0; row < numCellRowsCols; row++)
        {
            for (int col = 0; col < numCellRowsCols; col++)
            {
                Cell c = Grid[row, col];
                print($"{c.row} {c.col} {c.biome}");
            }
        }
    }

    //public Biome GetCellBiome(int row, int col)
    //{
    //    int rowMod = row * cellPixelSize;
    //    int colMod = col * cellPixelSize;

    //    int redCount = 0;
    //    int greenCount = 0;
    //    int blueCount = 0;

    //    for (int y = colMod; y < colMod + cellPixelSize; y++)
    //    {
    //        for (int x = rowMod; x < rowMod + cellPixelSize; x++)
    //        {
    //            Vector2 coord = new Vector2(x, y);
    //            if(redCoords.Contains(coord))
    //            {
    //                redCount++;
    //                redCoords.Remove(coord);
    //            }
    //            else if(greenCoords.Contains(coord))
    //            {
    //                greenCount++;
    //                greenCoords.Remove(coord);
    //            }
    //            if(blueCoords.Contains(coord))
    //            {
    //                blueCount++;
    //                blueCoords.Remove(coord);
    //            }
    //        }
    //    }
    //    print($"row: {row}  ,  col:  {col}");
    //    print($"r:{redCount} g:{greenCount} b:{blueCount}");
        

    //    int totalSum = redCount + greenCount + blueCount;
    //    print(totalSum);
    //    if (redCount / totalSum >= (minBiomePercentage / 100))
    //    {
    //        print($"rd:{redCount / totalSum}");
    //        print("Red Cell");
    //        return Biome.Red;
    //    }
    //    else if (greenCount / totalSum >= (minBiomePercentage / 100))
    //    {
    //        print($"gd:{greenCount / totalSum}");
    //        print("Green Cell");
    //        return Biome.Green;
    //    }
    //    else if (blueCount / totalSum >= (minBiomePercentage / 100))
    //    {
    //        print($"bd:{blueCount / totalSum}");
    //        print("Blue Cell");
    //        return Biome.Blue;
    //    }
    //    else
    //    {
    //        return Biome.Mixed;
    //    }

        





        //int rowMod = row * cellPixelSize;
        //int colMod = col * cellPixelSize;

        //float[,] cellPixels = new float[cellPixelSize, cellPixelSize];

        //for (int y = colMod; y < colMod + cellPixelSize; y++)
        //{
        //    for (int x = rowMod; x < rowMod + cellPixelSize; x++)
        //    {
        //        // HUE - H are degrees
        //        float H, S, V;
        //        Color.RGBToHSV(tex.GetPixel(x, y), out H, out S, out V);

        //        if (x != 0 && y != 0)
        //        {
        //            cellPixels[x - rowMod, y - colMod] = H;
        //        }
        //        else if (x == 0 && y != 0)
        //        {
        //            cellPixels[0, y - colMod] = H;
        //        }
        //        else if (x != 0 && y == 0)
        //        {
        //            cellPixels[x - rowMod, 0] = H;
        //        }
        //        else
        //        {
        //            cellPixels[0, 0] = H;
        //        }

        //    }
        //}

        //return cellPixels;

        //return;
    }

//    public float[,] GetCellPixels(int row, int col, Texture2D tex)
//    {
//        int rowMod = row * cellPixelSize;
//        int colMod = col * cellPixelSize;

//        float[,] cellPixels = new float[cellPixelSize, cellPixelSize];

//        for (int y = colMod; y < colMod + cellPixelSize; y++)
//        {
//            for (int x = rowMod; x < rowMod + cellPixelSize; x++)
//            {
//                // HUE - H are degrees
//                float H, S, V;
//                Color.RGBToHSV(tex.GetPixel(x, y), out H, out S, out V);

//                if (x != 0 && y != 0)
//                {
//                    cellPixels[x - rowMod, y - colMod] = H;
//                }
//                else if (x == 0 && y != 0)
//                {
//                    cellPixels[0, y - colMod] = H;
//                }
//                else if (x != 0 && y == 0)
//                {
//                    cellPixels[x - rowMod, 0] = H;
//                }
//                else
//                {
//                    cellPixels[0, 0] = H;
//                }

//            }
//        }

//        return cellPixels;
//    }
//    //#endif
//}

public class Cell : MonoBehaviour
{
    //public List<Vector2> coords;
    public Dictionary<Biome, int> possibleBiome; 
    public int row;
    public int col;
    public Biome biome;

    public Cell(int row, int col)
    {
        this.row = row;
        this.col = col;
        possibleBiome = new Dictionary<Biome, int>
        {
            {Biome.Red, 0},
            {Biome.Green, 0},
            {Biome.Blue, 0},
            {Biome.Mixed, 0}
        };
    }
}

public enum Biome
{
    Mixed,
    Red,
    Green,
    Blue
}
