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
        SceneManager.LoadScene("Main");
    }

    public void NoTutorial()
    {
        SceneManager.LoadScene("Main");
    }

    private void Awake()
    {
        yesButton.onClick.AddListener(YesTutorial);
        noButton.onClick.AddListener(NoTutorial);
    }
}
