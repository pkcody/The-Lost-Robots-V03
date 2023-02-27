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

    // Start is called before the first frame update
    void Start()
    {
        

    }

    //join scene sounds
    public void Welcome()
    {
        MSTalk("Intro_Welcome");
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
            print("main tlak");
            painting = GameObject.FindObjectOfType<Painting>(true).gameObject;
            MSTalk("Intro_wonderful");
        }
        if (SceneManager.GetActiveScene().name == "Game")
        {
            MSTalk("Paint_OhNo");
            MSTalk("Crash");
        }
        if (SceneManager.GetActiveScene().name == "Quit")
        {
            MSTalk("Outro_quickreset");
        }

    }

    //public void MSPaintScene(ItemObject io)
    //{
    //    //print(io.UIimage.name);
    //    if (SceneManager.GetActiveScene().name == "Game")
    //    {
    //        //print("in");
    //        response = "Go ahead and draw out the planet you wish to visit today.";
    //        StartCoroutine(DisplayLine(response));
    //    }
    //    if ()// robot is number 1
    //    {
    //        //print("in");
    //        response = "If there is any white space, you're going to make my life harder.";
    //        StartCoroutine(DisplayLine(response));
    //    }
    //    if ()// robot is number 1
    //    {
    //        //print("in");
    //        response = "Have you finished painting yet?";
    //        StartCoroutine(DisplayLine(response));
    //    }
    //    if ()// robot is number 1
    //    {
    //        //print("in");
    //        response = "What is taking so long there’s not that many colors today?";
    //        StartCoroutine(DisplayLine(response));
    //    }
    //    if ()// robot is number 1
    //    {
    //        //print("in");
    //        response = "Finally, about time, let's goooo.";
    //        StartCoroutine(DisplayLine(response));
    //    }
    //    if ()// robot is number 1
    //    {
    //        //print("in");
    //        response = "Oh no there's been a malfunction. Robots hold on tight; this won't be a calculated landing.";
    //        StartCoroutine(DisplayLine(response));
    //    }

    //}

    //public void MSOutro(ItemObject io)
    //{
    //    //print(io.UIimage.name);
    //    if (SceneManager.GetActiveScene().name == "Game")
    //    {
    //        //print("in");
    //        response = "The Window has been accounted for";
    //        StartCoroutine(DisplayLine(response));
    //    }
    //    if ()// robot is number 1
    //    {
    //        //print("in");
    //        response = "The full canister of H2O has been accounted for.";
    //        StartCoroutine(DisplayLine(response));
    //    }
    //    if ()// robot is number 1
    //    {
    //        //print("in");
    //        response = "The plated quadruple battery has been accounted for.";
    //        StartCoroutine(DisplayLine(response));
    //    }
    //    if ()// robot is number 1
    //    {
    //        //print("in");
    //        response = "All the missing parts have been accounted for. Please hurry and get in so we can leave. ";
    //        StartCoroutine(DisplayLine(response));
    //    }
    //    if ()// robot is number 1
    //    {
    //        //print("in");
    //        response = "Thank you my little robots we couldn’t have continued our journey without you.";
    //        StartCoroutine(DisplayLine(response));
    //    }
    //    if ()// robot is number 1
    //    {
    //        //print("in");
    //        response = "Robots do a quick reset then we can look to explore the next planet.";
    //        StartCoroutine(DisplayLine(response));
    //    }

    //}

    // Update is called once per frame

}
