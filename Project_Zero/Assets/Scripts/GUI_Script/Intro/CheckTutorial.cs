using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CheckTutorial : MonoBehaviour
{
    public Button yesButton;
    public Button noButton;

    public void YesTutorial()
    {
        LoadingSceneManager.LoadScene("Main");
        DialogueSystem.situation = "Tutorial";
    }

    public void NoTutorial()
    {
        LoadingSceneManager.LoadScene("Main");
        DialogueSystem.situation = null;
    }

    private void Awake()
    {
        yesButton.onClick.AddListener(YesTutorial);
        noButton.onClick.AddListener(NoTutorial);
    }
}
