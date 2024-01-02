using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;

public class SoundManager : MonoBehaviour
{
    public static Dictionary<string, float> volume = new Dictionary<string, float>() 
    { { "BGM", 10.0f }, { "SFX", 10.0f }, { "Master", 10.0f } };
    public AudioMixer mixer;
    public Slider bgmSlider, masterSlider, sfxSlider;

    public void ControlVolume(Slider slider)
    {
        string type = slider.name;
        volume[type] = slider.value;
        if (slider.value == -40f)
            mixer.SetFloat(type, -80f);
        else
            mixer.SetFloat(type, volume[type]);
    }

    private void Awake()
    {
        bgmSlider.onValueChanged.AddListener(delegate { ControlVolume(bgmSlider); });
        masterSlider.onValueChanged.AddListener(delegate { ControlVolume(masterSlider); });
        sfxSlider.onValueChanged.AddListener(delegate { ControlVolume(sfxSlider); });
        bgmSlider.value = volume["BGM"];
        sfxSlider.value = volume["SFX"];
        masterSlider.value = volume["Master"];
    }
}
