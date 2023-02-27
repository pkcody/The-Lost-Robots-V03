using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomBiomeBasedSpawning : MonoBehaviour
{
    [HideInInspector]
    public RandomBiomeBasedSpawning instance;

    public List<Vector2> doNotSpawnInCells = new List<Vector2>();
    private List<Cell> doNotSpawnGOsInCells = new List<Cell>();

    public GameObject motherShip;

    [Header("Red Biome Spawns")]
    public List<GameObject> redBiomePrefabs;
    public Transform redBiomeParent;
    public int redBiomeSpawnMinAmount = 10;
    public int redBiomeSpawnMaxAmount = 15;
    public int redBiomeMinDistanceBetweenSpawns = 10;
    public int redBiomeAttemptsToSpawnNotNearAnotherSpawn = 5;
    public List<GameObject> redBiomeSpawnedGOs = new List<GameObject>();

    [Header("Green Biome Spawns")]
    public List<GameObject> greenBiomePrefabs;
    public Transform greenBiomeParent;
    public int greenBiomeSpawnMinAmount = 15;
    public int greenBiomeSpawnMaxAmount = 25;
    public int greenBiomeMinDistanceBetweenSpawns = 7;
    public int greenBiomeAttemptsToSpawnNotNearAnotherSpawn = 5;
    public List<GameObject> greenBiomeSpawnedGOs = new List<GameObject>();

    [Header("Blue Biome Spawns")]
    public List<GameObject> blueBiomePrefabs;
    public Transform blueBiomeParent;
    public int blueBiomeSpawnMinAmount = 5;
    public int blueBiomeSpawnMaxAmount = 10;
    public int blueBiomeMinDistanceBetweenSpawns = 15;
    public int blueBiomeAttemptsToSpawnNotNearAnotherSpawn = 5;
    public List<GameObject> blueBiomeSpawnedGOs = new List<GameObject>();


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


        //Object[] redBiome_Resources = Resources.LoadAll("RedResources", typeof(GameObject));
        //Object[] greenBiome_Resources = Resources.LoadAll("GreenResources", typeof(GameObject));
        //Object[] blueBiome_Resources = Resources.LoadAll("BlueResources", typeof(GameObject));

        //Debug.Log(Application.dataPath + "data_path");

        ////Object[] redBiome_Resources = Resources.LoadAll("C:/Users/kcody/Desktop/TheLostRobotsBuild/RedResources", typeof(GameObject));
        ////Object[] greenBiome_Resources = Resources.LoadAll("C:/Users/kcody/Desktop/TheLostRobotsBuild/GreenResources", typeof(GameObject));
        ////Object[] blueBiome_Resources = Resources.LoadAll("C:/Users/kcody/Desktop/TheLostRobotsBuild/BlueResources", typeof(GameObject));


        //Debug.Log(redBiome_Resources.Length + "FunkyMonkey");
        //Debug.Log(greenBiome_Resources.Length);
        //Debug.Log(blueBiome_Resources.Length);

        //Instantiate("RedResources")

        //Object[] redBiome_Resources;
        //Object[] greenBiome_Resources;
        //Object[] blueBiome_Resources;

        //if (Application.isEditor)
        //{
        //    redBiome_Resources = Resources.LoadAll("RedResources", typeof(GameObject));
        //    greenBiome_Resources = Resources.LoadAll("GreenResources", typeof(GameObject));
        //    blueBiome_Resources = Resources.LoadAll("BlueResources", typeof(GameObject));
        //}
        //else
        //{
        //    //Application.persistentDataPath
        //    redBiome_Resources = Application.dataPath("/RedResources", typeof(GameObject));
        //    greenBiome_Resources = Resources.LoadAll(Application.streamingAssetsPath + "/GreenResources", typeof(GameObject));
        //    blueBiome_Resources = Resources.LoadAll(Application.streamingAssetsPath + "/BlueResources", typeof(GameObject));

        //    //redBiome_Resources = System
        //    //greenBiome_Resources = Resources.LoadAll("GreenResources", typeof(GameObject));
        //    //blueBiome_Resources = Resources.LoadAll("BlueResources", typeof(GameObject));
        //}



        //foreach (var go in redBiome_Resources)
        //{
        //    redBiomePrefabs.Add(go as GameObject);
        //}
        //foreach (var go in greenBiome_Resources)
        //{
        //    greenBiomePrefabs.Add(go as GameObject);
        //}
        //foreach (var go in blueBiome_Resources)
        //{
        //    blueBiomePrefabs.Add(go as GameObject);
        //}
    }


    public Vector3 RandomPrefabSpawnPosInCell(Cell c)
    {
        int xMin = c.col * GridBreakdown.cellPixelSize - 495;
        int xMax = c.col * GridBreakdown.cellPixelSize - 405 - GridBreakdown.cellPixelSize;
        int zMin = c.row * GridBreakdown.cellPixelSize - 495;
        int zMax = c.row * GridBreakdown.cellPixelSize - 405 - GridBreakdown.cellPixelSize;

        //Debug.Log($"xmin {xMin} xmax {xMax}");
        //Debug.Log($"zmin {zMin} zmax {zMax}");
        float randXinCell = Random.Range(xMin, xMax);
        float randZinCell = Random.Range(zMin, zMax);

        return new Vector3(randXinCell, 30f, randZinCell);
    }

    public Vector3 MoveGOToTerrainHeight(GameObject go)
    {
        //Debug.DrawRay(go.transform.position, new Vector3(0, -70, 0), Color.magenta, 15);
        Vector3 raycastPos = new Vector3(go.transform.position.x, go.transform.position.y - .25f, go.transform.position.z);
        Physics.Raycast(raycastPos, Vector3.down, out RaycastHit hit, 150);

        return new Vector3(go.transform.position.x, hit.point.y, go.transform.position.z);
    }

    public void SpawnBiomeGOs()
    {
        // Get all my no spawn Cells
        foreach (Vector2 emptyCell in doNotSpawnInCells)
        {
            Cell c = GridBreakdown.instance.Grid[(int)emptyCell.x, (int)emptyCell.y];
            doNotSpawnGOsInCells.Add(c);
            Debug.Log("" + c.row + c.col + c.biome);
        }

        Debug.Log(doNotSpawnGOsInCells.Count + "ahhhhhh");

        SpawnMothership();
        RedBiomeSpawn();
        GreenBiomeSpawn();
        BlueBiomeSpawn();
        ClearFloaters();

    }

    public void ClearFloaters()
    {
        foreach(GameObject go in redBiomeSpawnedGOs)
        {
            Physics.Raycast(go.transform.position - new Vector3(0, -0.25f, 0), Vector3.down, out RaycastHit hit, 100);
            if (hit.distance > 1)
            {
                Destroy(go);
                Debug.Log("Cleaned up");
            }
        }
        foreach(GameObject go in greenBiomeSpawnedGOs)
        {
            Physics.Raycast(go.transform.position - new Vector3(0, -0.25f, 0), Vector3.down, out RaycastHit hit, 100);
            if (hit.distance > 1)
            {
                Destroy(go);
                Debug.Log("Cleaned up");
            }
        }
        foreach(GameObject go in blueBiomeSpawnedGOs)
        {
            Physics.Raycast(go.transform.position - new Vector3(0, -0.25f, 0), Vector3.down, out RaycastHit hit, 100);
            if (hit.distance > 1)
            {
                Destroy(go);
                Debug.Log("Cleaned up");
            }
        }
    }

    public void SpawnMothership()
    {
        Cell spawnCell = doNotSpawnGOsInCells[0];
        int x = spawnCell.col * GridBreakdown.cellPixelSize - 450;
        int z = spawnCell.row * GridBreakdown.cellPixelSize - 450;
        Vector3 spawnPos = new Vector3(x, 30f, z);
        motherShip.transform.position = spawnPos;
        motherShip.transform.position = MoveGOToTerrainHeight(motherShip);
    }

    private void RedBiomeSpawn()
    {
        Debug.Log("RedBiomeSpawn");
        redBiomeParent = GameObject.Find("RedBiomeParent").transform;
        foreach (Cell c in GridBreakdown.instance.redBiomeCells)
        {
            foreach (Vector2 v in doNotSpawnInCells)
            {
                if (c.row != v.x || c.col != v.y)
                {
                    Debug.Log($"red {c.row} {c.col}");
                    List<GameObject> redBiomeGOsInCell = new List<GameObject>();
                    for (int i = 0; i < Random.Range(redBiomeSpawnMinAmount, redBiomeSpawnMaxAmount); i++)
                    {
                        for (int a = 0; a < redBiomeAttemptsToSpawnNotNearAnotherSpawn; a++)
                        {

                            Vector3 spawnPos = RandomPrefabSpawnPosInCell(c);

                            if (redBiomeGOsInCell.Count == 0)
                            {
                                GameObject pickedRedBiomePrefab = redBiomePrefabs[Random.Range(0, redBiomePrefabs.Count)];
                                GameObject redBiomeGO = Instantiate(pickedRedBiomePrefab, spawnPos, Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)), redBiomeParent);
                                redBiomeGOsInCell.Add(redBiomeGO);
                                redBiomeSpawnedGOs.Add(redBiomeGO);
                                break;
                            }
                            else
                            {
                                foreach (GameObject existingSpawn in redBiomeGOsInCell)
                                {
                                    if ((spawnPos - existingSpawn.transform.position).magnitude < redBiomeMinDistanceBetweenSpawns)
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        GameObject pickedRedBiomePrefab = redBiomePrefabs[Random.Range(0, redBiomePrefabs.Count)];
                                        GameObject redBiomeGO = Instantiate(pickedRedBiomePrefab, spawnPos, Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)), redBiomeParent);
                                        redBiomeGOsInCell.Add(redBiomeGO);
                                        redBiomeSpawnedGOs.Add(redBiomeGO);
                                        break;
                                    }

                                }
                                Debug.Log("Unable to spawn Red");
                                break;
                            }
                        }
                    }
                }
                else
                {
                    //Debug.Log($"Cannot spawn in {c.row} {c.col}");
                }
            }
        }

        foreach(GameObject redBiomeGO in redBiomeSpawnedGOs)
        {
            redBiomeGO.transform.position = MoveGOToTerrainHeight(redBiomeGO);
        }
    }

    private void GreenBiomeSpawn()
    {
        Debug.Log("GreenBiomeSpawn");
        greenBiomeParent = GameObject.Find("GreenBiomeParent").transform;
        foreach (Cell c in GridBreakdown.instance.greenBiomeCells)
        {
            foreach(Vector2 v in doNotSpawnInCells)
            {
                if(c.row != v.x || c.col != v.y)
                {
                    Debug.Log($"green {c.row} {c.col}");
                    List<GameObject> greenBiomeGOsInCell = new List<GameObject>();
                    for (int i = 0; i < Random.Range(greenBiomeSpawnMinAmount, greenBiomeSpawnMaxAmount); i++)
                    {
                        for (int a = 0; a < greenBiomeAttemptsToSpawnNotNearAnotherSpawn; a++)
                        {

                            Vector3 spawnPos = RandomPrefabSpawnPosInCell(c);

                            if (greenBiomeGOsInCell.Count == 0)
                            {
                                GameObject pickedGreenBiomePrefab = greenBiomePrefabs[Random.Range(0, greenBiomePrefabs.Count)];
                                GameObject greenBiomeGO = Instantiate(pickedGreenBiomePrefab, spawnPos, Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)), greenBiomeParent);
                                greenBiomeGOsInCell.Add(greenBiomeGO);
                                greenBiomeSpawnedGOs.Add(greenBiomeGO);
                                break;
                            }
                            else
                            {
                                foreach (GameObject existingSpawn in greenBiomeGOsInCell)
                                {
                                    if ((spawnPos - existingSpawn.transform.position).magnitude < greenBiomeMinDistanceBetweenSpawns)
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        GameObject pickedGreenBiomePrefab = greenBiomePrefabs[Random.Range(0, greenBiomePrefabs.Count)];
                                        GameObject greenBiomeGO = Instantiate(pickedGreenBiomePrefab, spawnPos, Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)), greenBiomeParent);
                                        greenBiomeGOsInCell.Add(greenBiomeGO);
                                        greenBiomeSpawnedGOs.Add(greenBiomeGO);
                                        break;
                                    }
                                }
                                Debug.Log("Unable to spawn Green");
                                break;
                            }
                        }
                    }
                }
                else
                {
                    //Debug.Log($"Cannot spawn in {c.row} {c.col}");
                }
                
            }
            
        }

        foreach (GameObject greenBiomeGO in greenBiomeSpawnedGOs)
        {
            greenBiomeGO.transform.position = MoveGOToTerrainHeight(greenBiomeGO);
        }
    }

    private void BlueBiomeSpawn()
    {
        Debug.Log("BlueBiomeSpawn");
        blueBiomeParent = GameObject.Find("BlueBiomeParent").transform;
        foreach (Cell c in GridBreakdown.instance.blueBiomeCells)
        {
            foreach (Vector2 v in doNotSpawnInCells)
            {
                if (c.row != v.x || c.col != v.y)
                {
                    Debug.Log($"blue {c.row} {c.col}");
                    List<GameObject> blueBiomeGOsInCell = new List<GameObject>();
                    for (int i = 0; i < Random.Range(blueBiomeSpawnMinAmount, blueBiomeSpawnMaxAmount); i++)
                    {
                        for (int a = 0; a < blueBiomeAttemptsToSpawnNotNearAnotherSpawn; a++)
                        {

                            Vector3 spawnPos = RandomPrefabSpawnPosInCell(c);

                            if (blueBiomeGOsInCell.Count == 0)
                            {
                                GameObject pickedBlueBiomePrefab = blueBiomePrefabs[Random.Range(0, blueBiomePrefabs.Count)];
                                GameObject blueBiomeGO = Instantiate(pickedBlueBiomePrefab, spawnPos, Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)), blueBiomeParent);
                                blueBiomeGOsInCell.Add(blueBiomeGO);
                                blueBiomeSpawnedGOs.Add(blueBiomeGO);
                                break;
                            }
                            else
                            {
                                foreach (GameObject existingSpawn in blueBiomeGOsInCell)
                                {
                                    if ((spawnPos - existingSpawn.transform.position).magnitude < blueBiomeMinDistanceBetweenSpawns)
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        GameObject pickedBlueBiomePrefab = blueBiomePrefabs[Random.Range(0, blueBiomePrefabs.Count)];
                                        GameObject blueBiomeGO = Instantiate(pickedBlueBiomePrefab, spawnPos, Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)), blueBiomeParent);
                                        blueBiomeSpawnedGOs.Add(blueBiomeGO);
                                        blueBiomeGOsInCell.Add(blueBiomeGO);
                                        break;
                                    }
                                }
                                Debug.Log("Unable to spawn Blue");
                                break;
                            }
                        }
                    }
                }
                else
                {
                    //Debug.Log($"Cannot spawn in {c.row} {c.col}");
                }
            }
        }

        foreach (GameObject blueBiomeGO in blueBiomeSpawnedGOs)
        {
            blueBiomeGO.transform.position = MoveGOToTerrainHeight(blueBiomeGO);
        }
    }
}
