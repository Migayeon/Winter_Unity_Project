using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BankruptManager : MonoBehaviour
{
    [SerializeField]
    private Button menuButton;
    private void Start()
    {
        menuButton.onClick.RemoveAllListeners();
        menuButton.onClick.AddListener(TurnToMenu);
        AchievementManager.Achieve(2);
    }

    private void TurnToMenu()
    {
        SceneManager.LoadScene("Title");
    }
}
