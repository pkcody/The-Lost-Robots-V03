using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerProgress : MonoBehaviour
{
    public GameObject[] towerList = new GameObject[3];
    public GameObject tower1; // this is changing for random spawn bc of clones (clone wars)

    public List<GameObject> towerWaves;
    public List<GameObject> towerProgress;

    public List<GameObject> pauseTowerWaves;
    public List<GameObject> pauseTowerProgress;

    public int index = 0;
    
    public GameObject YayTowerUI;

    private void Start()
    {
        towerList[0] = tower1;
        index++;
    }
    
    public void TowerPlacedCheck(GameObject tower)
    {
        towerList[index] = tower;
        YayTowerUI.SetActive(true);

        towerWaves[index - 1].SetActive(false);
        pauseTowerWaves[index - 1].SetActive(false);
        towerProgress[index - 1].SetActive(false);
        pauseTowerProgress[index - 1].SetActive(false);

        towerWaves[index].SetActive(true);
        pauseTowerWaves[index].SetActive(true);
        towerProgress[index].SetActive(true);
        pauseTowerProgress[index].SetActive(true);


        PlayTowerSoundsPause();

        //FindObjectOfType<Mothership>().TryMotherShipEnd();

        Invoke("HideParents", 5f);
        index++;
    }


    public void PlayTowerSoundsPause()
    {
        if (index == 1 && towerList[index] == null)
        {
            FindObjectOfType<TowerSoundEffect>().firstTime1 = false;
            FindObjectOfType<TowerSoundEffect>().PlayTower1Sound();

            FindObjectOfType<Mothership>().tower1 = true;
        }
        else if (index == 1)
        {
            FindObjectOfType<TowerSoundEffect>().firstTime2 = false;
            FindObjectOfType<TowerSoundEffect>().PlayTower2Sound();
            MotherShipSubTitles.instance.GameSubT(3);

            FindObjectOfType<Mothership>().tower2 = true;
        }
        else if (index == 2)
        {
            FindObjectOfType<TowerSoundEffect>().firstTime3 = false;
            FindObjectOfType<TowerSoundEffect>().PlayTower3Sound();
            MotherShipSubTitles.instance.GameSubT(4);

            FindObjectOfType<Mothership>().tower3 = true;
        }
    }

    public void HideParents()
    {
        YayTowerUI.SetActive(false);
    }
}
