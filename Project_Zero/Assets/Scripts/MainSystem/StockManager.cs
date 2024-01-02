using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StockManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI currentTurn;
    [SerializeField] TextMeshProUGUI exchangeRate;
    [SerializeField] TextMeshProUGUI exchangePercent;
    void Start()
    {
        currentTurn.text = $"Turn: {TurnManager.turn}";
        exchangeRate.text = $"Current Price: {GoodsManager.exchangeRate}";
        exchangePercent.text = $"({GoodsManager.exchangePercent}%)";
    }

    public void MoveToAfterTurn()
    {
        SceneManager.LoadScene("AfterTurn");
    }
}
