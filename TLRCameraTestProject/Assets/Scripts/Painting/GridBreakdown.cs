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
    public List<Cell> tealBiomeCells = new List<Cell>();
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
        numCellRowsCols = 1000 / cellPixelSize; // at 100 then

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
                float tealCount = c.possibleBiome[Biome.Teal];
                float blueCount = c.possibleBiome[Biome.Blue];
                float totalCount = redCount + greenCount + tealCount + blueCount;

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
                else if (tealCount / totalCount >= (minBiomePercentage / 100))
                {
                    c.biome = Biome.Teal;
                    tealBiomeCells.Add(c);
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

    
    }


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
            {Biome.Teal, 0},
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
    Teal,
    Blue
}
