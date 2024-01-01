using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DuringTurn : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI exchangeRate;
    [SerializeField] TextMeshProUGUI exchangePercent;
    void Start()
    {
        exchangeRate.text = $"Current Price: {GoodsManager.exchangeRate}";
        exchangePercent.text = $"({GoodsManager.exchangePercent}%)";
    }

    public void MoveToAfterTurn()
    {
        SceneManager.LoadScene("AfterTurn");
    }
}
