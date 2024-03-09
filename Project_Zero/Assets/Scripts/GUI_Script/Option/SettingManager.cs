using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{
    const int N = 2; // 탭 개수

    // 옵션 탭 관련 변수
    public Button[] tabButton = new Button[N];
    public GameObject[] tab = new GameObject[N];

    // 사운드 탭 관련 변수
    public static Dictionary<string, float> volume = new Dictionary<string, float>()
    { { "BGM", 10.0f }, { "SFX", 10.0f }, { "Master", 10.0f } };
    public AudioMixer mixer;
    public Slider bgmSlider, masterSlider, sfxSlider;


    public void OpenTab(int index)
    {
        for (int i = 0; i < N; i++)
        {
            tabButton[i].interactable = true;
            tab[i].SetActive(false);
        }
        tabButton[index].interactable = false;
        tab[index].SetActive(true);
    }

    public void OpenSoundTab()
    {
        OpenTab(0);
        bgmSlider.onValueChanged.AddListener(delegate { ControlVolume(bgmSlider); });
        masterSlider.onValueChanged.AddListener(delegate { ControlVolume(masterSlider); });
        sfxSlider.onValueChanged.AddListener(delegate { ControlVolume(sfxSlider); });
        bgmSlider.value = volume["BGM"];
        sfxSlider.value = volume["SFX"];
        masterSlider.value = volume["Master"];
    }

    public void ControlVolume(Slider slider)
    {
        string type = slider.name;
        volume[type] = slider.value;
        if (slider.value == -40f)
            mixer.SetFloat(type, -80f);
        else
            mixer.SetFloat(type, volume[type]);
    }

    public void OpenSystemTab()
    {
        OpenTab(1);
    }

    public void ResetData()
    {
        PlayerPrefs.DeleteAll();
        AchievementManager.ResetAchievements();
        SceneManager.LoadScene("Title");
    }

    public void ShowCredit()
    {

    }

    public void SaveSetting()
    {
        string volumeString =
            volume["Master"].ToString() + "/" +
            volume["BGM"].ToString() + "/" +
            volume["SFX"].ToString();
        PlayerPrefs.SetString("setting", volumeString);
    }

    public static void LoadSetting()
    {
        if (PlayerPrefs.HasKey("setting"))
        {
            string[] volumeSetting = PlayerPrefs.GetString("setting").Split("/");
            volume["Master"] = float.Parse(volumeSetting[0]);
            volume["BGM"] = float.Parse(volumeSetting[1]);
            volume["SFX"] = float.Parse(volumeSetting[2]);
        }
        AudioMixer mixer = Resources.Load<AudioMixer>("Sound/Mixer/MyMixer");
        mixer.SetFloat("Master", volume["Master"]);
        mixer.SetFloat("BGM", volume["BGM"]);
        mixer.SetFloat("SFX", volume["SFX"]);
    }

    private void Awake()
    {
        /*
        for(int i = 0; i < 3; i++)
        {
            tabButton[i].onClick.RemoveAllListeners();
        }
        tabButton[0].onClick.AddListener(() => OpenTab(0));
        tabButton[1].onClick.AddListener(() => OpenTab(1));
        tabButton[2].onClick.AddListener(
            delegate
            {
                SceneManager.LoadScene("Credit");
            }
        );
        */
        OpenSoundTab();
    }
}
