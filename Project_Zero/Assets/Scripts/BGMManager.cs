using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMManager : MonoBehaviour
{
    public static AudioSource audioSource = null;
    private static string currentBGM = "";

    private Scene scene;
    Dictionary<string, string> sceneBGM = new Dictionary<string, string>()
    {
        { "CHTest", "reminiscence" },
        { "Title", "start" },
        { "Savedata","start" },
        { "Intro", "opening" },
        { "Main", "Creep" },
        {"Setting","reminiscence" }
    
    };

    public static BGMManager instance;
    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this.gameObject);
        //Debug.Log(sceneName);
        audioSource = GetComponent<AudioSource>();
        currentBGM = audioSource.clip.name;
        DontDestroyOnLoad(gameObject);
        
        /*
        string BGMname = "";
        try
        {
            BGMname = sceneBGM[sceneName];
        }
        catch { }
        SetBGM(BGMname);
        */
    }
    private void LoadedSceneEvent(Scene scene, LoadSceneMode mode)
    {
        Debug.Log(scene.name);
        string BGMname = "";
        try
        {
            BGMname = sceneBGM[scene.name];
        }
        catch { }
        SetBGM(BGMname);
    }
    private void Start()
    {
        SceneManager.sceneLoaded += LoadedSceneEvent;
    }

    public static void SetBGM(string name)
    {
        if (currentBGM == name)
        {
            Debug.Log(name);
            return;
        }
        else
        {
            try
            {
                audioSource.clip = Resources.Load<AudioClip>("Sound/BGM/" + name);
                currentBGM = name;
            }
            catch
            {
                if (name == "")
                {
                    audioSource.clip = null;
                }
            }
            audioSource.Play();
        }
        Debug.Log(name);
    }
}
