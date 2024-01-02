using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StockManager : MonoBehaviour
{
    // 마정석 환전과 관련된 스크립트
    int purchaseAmount = 0;
    int saleAmount = 0;
    // 환전 정보
    [SerializeField] TextMeshProUGUI exchangeRate;
    [SerializeField] TextMeshProUGUI exchangePercent;
    [SerializeField] TextMeshProUGUI purchaseAmountText;

    // 구매 시스템
    [SerializeField] Button purchaseButton;
    [SerializeField] GameObject checkPurchase;
    [SerializeField] Button checkPurchaseYes;
    [SerializeField] Button checkPurchaseNo;
    void Awake()
    {
        purchaseButton.onClick.AddListener(() => AbleUI(checkPurchase));
    }
    void Start()
    {
        exchangeRate.text = $"현재 가격: {GoodsManager.exchangeRate}";
        exchangePercent.text = $"({GoodsManager.exchangePercent}%)";
    }
    private void Update()
    {
        try
        {
            purchaseAmountText.text = purchaseAmount.ToString();

        }
        catch
        {

        }

    }
    public void AbleUI(GameObject target)
    {
        target.SetActive(true);
    }
    public void DisableUI(GameObject target)
    {
        target.SetActive(false);
    }
    // 구입 관련 함수
    public void PurchaseStone()
    {
        GoodsManager.goodsAr -= GoodsManager.exchangeRate * purchaseAmount;
        GoodsManager.goodsStone += purchaseAmount;
    }
    public void AddPurchaseAmount(int amount)
    {
        if (purchaseAmount + amount < 0)
        {
            purchaseAmount = 0;
        }
        else if (purchaseAmount + amount > GoodsManager.goodsAr / GoodsManager.exchangeRate) 
        {
            purchaseAmount = GoodsManager.goodsAr / GoodsManager.exchangeRate;
        }
        else
        {
            purchaseAmount += amount;
        }
    }
    // 판매 관련 함수
    public void SaleStone()
    {
        
    }
}
