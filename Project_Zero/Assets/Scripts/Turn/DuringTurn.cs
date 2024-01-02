using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DuringTurn : MonoBehaviour
{
    public Button endTurn;
    void Awake()
    {
        endTurn.onClick.AddListener(EndTurn);
    }

    public void EndTurn()
    {
        SceneManager.LoadScene("AfterTurn");
    }
}
