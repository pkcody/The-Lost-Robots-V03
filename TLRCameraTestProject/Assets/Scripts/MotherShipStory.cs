using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MotherShipStory : MonoBehaviour
{
    public static MotherShipStory instance;
    public AudioSource _as;
    public List<AudioClip> audioClips;

    public GameObject painting;
    public int paintLine = 0;
    public List<AudioClip> paintLineAudio;
    public int paintIndex = 0;

    public Queue<AudioClip> clipQ = new Queue<AudioClip>();

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

        //DontDestroyOnLoad(this);
    }

    //join scene sounds
    public void Welcome()
    {
        MSTalk("Intro_Welcome");

        MotherShipSubTitles.instance.JoinSubT();
    }

    public IEnumerator PaintingCommentary()
    {
        yield return new WaitForSeconds(5f);
        if (painting != null)
        {
            if (painting.activeSelf)
            {
                paintLine++;
                if (paintLine == 3)
                {
                    MSTalk(paintLineAudio[paintIndex]);
                    paintLine = 0;
                    paintIndex = (paintIndex + 1) % paintLineAudio.Count;
                }
            }
        }
        

        StartCoroutine(PaintingCommentary());
        
    }

    //all sounds use this
    public void MSTalk(string s)
    {
        clipQ.Enqueue(audioClips.Find(clipName => clipName.name == s));
    }

    public void MSTalk(AudioClip ac)
    {
        clipQ.Enqueue(ac);
    }

    void Update()
    {
        if (!_as.isPlaying && clipQ.Count > 0)
        {
            _as.clip = clipQ.Dequeue();
            _as.PlayOneShot(_as.clip, 0.5f);
        }
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene().name == "Join")
        {
            Invoke("Welcome", 2f);
        }
        print(SceneManager.GetActiveScene().name);
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            //print("main tlak");
            painting = GameObject.FindObjectOfType<Painting>(true).gameObject;
            MSTalk("Intro_wonderful");

            MotherShipSubTitles.instance.MainMenuSubT(1);

        }
        
        if (SceneManager.GetActiveScene().name == "Quit")
        {
            MSTalk("Outro_quickreset");
            MotherShipSubTitles.instance.EndSubT();

        }

    }

    public void GameActuallyStarting()
    {
        MSTalk("Paint_OhNo");
        MotherShipSubTitles.instance.GameSubT(1);
        StartCoroutine(DelayCrash());
    }

    IEnumerator DelayCrash()
    {
        yield return new WaitForSeconds(16f);
        MSTalk("Crash");

    }

}
