using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSoundEffect : MonoBehaviour
{
    public AudioSource _as;
    public List<AudioClip> audioClips;
    public bool ImTower1;

    public bool tutorialON = true;

    public bool firstTime1 = false;
    public bool firstTime2 = false;
    public bool firstTime3 = false;

    public GameObject poofCreate;

    private void Start()
    {
        poofCreate.SetActive(true);
    }


    private void OnTriggerEnter(Collider cc)
    {
        
        if (!tutorialON)
        {
            PlayTower1Sound();
        }
        
            
    }
    public void PlayTower1Sound()
    {
        if (ImTower1)
        {
            if (!firstTime1)
            {
                _as.PlayOneShot(_as.clip, 0.5f);
                firstTime1 = true;
            }

        }


    }


    public void PlayTower2Sound()
    {
        if (!firstTime2)
        {
            _as.clip = audioClips.Find(clipName => clipName.name == "MS_HalfSound");
            _as.PlayOneShot(_as.clip, 0.5f);
            firstTime2 = true;

        }


    }
    public void PlayTower3Sound()
    {
        if (!firstTime3)
        {
            _as.clip = audioClips.Find(clipName => clipName.name == "MS_FullRequest");
            _as.PlayOneShot(_as.clip, 0.5f);
            firstTime3 = true;

        }

    }
}
