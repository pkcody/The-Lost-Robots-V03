using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class BiomeTracker : MonoBehaviour
{
    public Texture2D tex;
    public string currentBiome = "";
    public string nextBiome = "";

    public int offset = 500;

    public AudioSource _as;
    public List<AudioClip> audioClips;

    Coroutine audioCoroutine = null;
    Coroutine postCoroutine = null;

    //public PostProcessVolume defaultPost;
    public Volume defaultPost;
    public AudioClip defaultAudio;

    //Post
    //public PostProcessVolume 
    //public List<PostProcessVolume> postBiomes;
    public List<Volume> postBiomes;

    public void Default()
    {
        _as.clip = defaultAudio;
        _as.Play();
        Volume ppv = defaultPost;
        while (ppv.weight <= 1)
        {
            ppv.weight += 1 * Time.deltaTime;

        }

    }

    public void PlayThisAudio()
    {
        Volume ppv = defaultPost;

        while (ppv.weight > 0)
        {
            ppv.weight -= 1 * Time.deltaTime;

        }
        _as.Play();
    }

    public IEnumerator CheckBiome()
    {
        //Cell cell = GridBreakdown.instance.FindPixelsCell((int)transform.position.x + offset, (int)transform.position.z + 500);
        //print(cell.biome);


        //print(tex.GetPixel((int)transform.position.x + offset, (int)transform.position.z + 500));

        if (!PlayerSpawning.instance.BIGTutorialON)
        {
            Color pixelColor = tex.GetPixel((int)transform.position.x + offset, (int)transform.position.z + 500);
            if (pixelColor.r == Mathf.Max(pixelColor.r, pixelColor.g, pixelColor.b))
            {
                print("red b");
                nextBiome = "red";
            }
            else if (pixelColor.g == Mathf.Max(pixelColor.r, pixelColor.g, pixelColor.b) && pixelColor.g < 0.5f)
            {
                print("green b");
                nextBiome = "green";

            }
            else if (pixelColor.g == Mathf.Max(pixelColor.r, pixelColor.g, pixelColor.b) && pixelColor.g > 0.5f)
            {
                print("teal b");
                nextBiome = "teal";

            }
            else
            {
                print("blue b");
                nextBiome = "blue";

            }

            if (nextBiome != currentBiome)
            {
                UpdateBiome(nextBiome, currentBiome);
            }


        }
        yield return new WaitForSeconds(8f);

        StartCoroutine(CheckBiome());


    }
    IEnumerator FadeOutIn(string biome, float duration)
    {
        //vol out
        while (_as.volume > 0)
        {
            //_as.volume -= startVolume * Time.deltaTime / duration;

            _as.volume -= duration * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        //find new clip


        _as.clip = audioClips.Find(clipName => clipName.name.Contains(biome));
        _as.Play();
        //back in
        while (_as.volume <= .15f)
        {
            //_as.volume += startVolume * Time.deltaTime / duration;
            _as.volume += duration * Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }
        print("playing audio" + _as.isPlaying);

    }

    IEnumerator FadePost(string cur, string old, float duration)
    {
        //fade in
        Volume ppv = postBiomes.Find(clipName => clipName.name.Contains(old));
        print("ppv old" + ppv.name);
        while (ppv.weight > 0)
        {
            ppv.weight -= duration * Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        ppv = postBiomes.Find(clipName => clipName.name.Contains(cur));
        print("ppv old" + ppv.name);

        //fade out
        while (ppv.weight <= 1)
        {
            ppv.weight += duration * Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }
    }

    public void UpdateBiome(string biome, string old)
    {
        currentBiome = nextBiome;

        if (audioCoroutine != null)
        {
            StopCoroutine(audioCoroutine);
            StopCoroutine(postCoroutine);
        }
        audioCoroutine = StartCoroutine(FadeOutIn(biome, 1.5f));
        postCoroutine = StartCoroutine(FadePost(biome, old, 1.5f));
    }
}
