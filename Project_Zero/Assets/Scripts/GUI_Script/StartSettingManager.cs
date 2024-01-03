using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartSettingManager : MonoBehaviour
{
    public Text message;
    public Button confirmButton;
    public InputField inputField;

    public void InputMyName()
    {
        PlayerInfo.playerName = inputField.textComponent.text;
        inputField.textComponent.text = "";
        inputField.text = "";
        confirmButton.onClick.RemoveAllListeners();
        message.text = "아카데미의 이름을 정해주세요. (최대 8글자)";
        confirmButton.onClick.AddListener(InputArcademyName);
    }

    public void InputArcademyName()
    {
        PlayerInfo.arcademyName = inputField.textComponent.text;
        inputField.textComponent = null;
        SceneManager.LoadScene("Tutorial");
    }

    private void Awake()
    {
        message.text = "당신의 이름은 무엇입니까? (최대 8글자)";
        confirmButton.onClick.AddListener(InputMyName); 
    }

}
