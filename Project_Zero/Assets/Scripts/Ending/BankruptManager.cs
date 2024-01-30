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
    }

    private void TurnToMenu()
    {
        SceneManager.LoadScene("Title");
    }
}
