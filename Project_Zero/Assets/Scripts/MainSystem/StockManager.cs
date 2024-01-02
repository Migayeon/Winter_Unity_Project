using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StockManager : MonoBehaviour
{
    // 마정석 환전과 관련된 스크립트
    [SerializeField] TextMeshProUGUI exchangeRate;
    [SerializeField] TextMeshProUGUI exchangePercent;

    void Awake()
    {
        
    }
    void Start()
    {
        exchangeRate.text = $"현재 가격: {GoodsManager.exchangeRate}";
        exchangePercent.text = $"({GoodsManager.exchangePercent}%)";
    }
    private void Update()
    {

    }
    public void MoveToAfterTurn()
    {
        SceneManager.LoadScene("AfterTurn");
    }

    // 구입 관련 함수
    public void PurchaseStone(int amount)
    {
        GoodsManager.goodsAr -= GoodsManager.exchangeRate * amount;
        GoodsManager.goodsStone += amount;
    }

    // 판매 관련 함수
    public void SaleStone()
    {
        
    }
}
