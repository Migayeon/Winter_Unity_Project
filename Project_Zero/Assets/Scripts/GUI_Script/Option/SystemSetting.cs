using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SystemSetting : MonoBehaviour
{
    public Button resetButton;
    public Button yesButton, noButton;
    public GameObject message;

    public void ResetQuestion()
    {
        message.SetActive(true);
        yesButton.onClick.AddListener(YesReset);
        yesButton.onClick.AddListener(NoReset);
    }

    public void YesReset()
    {
        yesButton.onClick.RemoveAllListeners();
        noButton.onClick.RemoveAllListeners();
        message.SetActive(false);

        PlayerPrefs.DeleteAll();
        /*
          
            json 파일 초기화 구현
          
         */
    }

    public void NoReset()
    {
        yesButton.onClick.RemoveAllListeners();
        noButton.onClick.RemoveAllListeners();
        message.SetActive(false);
    }

    private void Awake()
    {
        message.SetActive(false);
        resetButton.onClick.AddListener(ResetQuestion);
    }


}
