using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static float bgmVolume, masterVolume, sfxVolume;
    public AudioMixer mixer;
    public Slider bgmSlider;
    // Start is called before the first frame update
    public void ControlBGM()
    {
        bgmVolume = bgmSlider.value;
        if (bgmSlider.value == -40f)
            mixer.SetFloat("BGM", -80f);
        else
            mixer.SetFloat("BGM", bgmVolume);
       
    }

    private void Awake()
    {
        bgmSlider.onValueChanged.AddListener(delegate { ControlBGM(); });
        mixer.SetFloat("BGM", bgmSlider.value);
    }
}
