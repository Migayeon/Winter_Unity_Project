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


    // 구매 시스템
    [SerializeField] Button purchaseButton;
    [SerializeField] GameObject checkPurchase;
    [SerializeField] Button checkPurchaseYes;
    [SerializeField] Button checkPurchaseNo;
    [SerializeField] TextMeshProUGUI purchaseAmountText;

    // 판매 시스템
    [SerializeField] Button saleButton;
    [SerializeField] GameObject checkSale;
    [SerializeField] Button checkSaleYes;
    [SerializeField] Button checkSaleNo;
    [SerializeField] TextMeshProUGUI saleAmountText;
    [SerializeField] TextMeshProUGUI expectArAmount;
    void Awake()
    {
        purchaseButton.onClick.AddListener(() => AbleUI(checkPurchase));
        checkPurchaseYes.onClick.AddListener(PurchaseStone);
        checkPurchaseNo.onClick.AddListener(() => DisableUI(checkPurchase));

        saleButton.onClick.AddListener(() => AbleUI(checkSale));
        checkSaleYes.onClick.AddListener(SaleStone);
        checkSaleNo.onClick.AddListener(() => DisableUI(checkSale));
    }
    void Start()
    {
        exchangeRate.text = $"현재 가격: {GoodsManager.exchangeRate}";
        if (GoodsManager.exchangePercent >= 0)
        {
            exchangePercent.text = $"(+{GoodsManager.exchangePercent}%)";
            exchangePercent.color = new Color32(255, 0, 0, 255);
        }
        else
        {
            exchangePercent.text = $"({GoodsManager.exchangePercent}%)";
            exchangePercent.color = new Color32(0, 100, 255, 255);
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
        AfterTurn.magic_Cost -= GoodsManager.exchangeRate * purchaseAmount;
        GoodsManager.goodsStone += purchaseAmount;
        purchaseAmount = 0;
        purchaseAmountText.text = purchaseAmount.ToString();
        DisableUI(checkPurchase);
    }
    public void AddPurchaseAmount(int amount)
    {
        if (GoodsManager.goodsAr >= 0)
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
            purchaseAmountText.text = purchaseAmount.ToString();
        }
    }
    // 판매 관련 함수
    public void SaleStone()
    {
        GoodsManager.goodsStone -= saleAmount;
        GoodsManager.goodsAr += (int)(GoodsManager.exchangeRate * saleAmount * 0.97);
        AfterTurn.magic_Rev += (int)(GoodsManager.exchangeRate * saleAmount * 0.97);
        saleAmount = 0;
        saleAmountText.text = saleAmount.ToString();
        DisableUI(checkSale);
    }
    public void AddSaleAmount(int amount)
    {
        if (GoodsManager.goodsStone >= 0)
        {
            if (saleAmount + amount < 0)
            {
                saleAmount = 0;
            }
            else if (saleAmount + amount > GoodsManager.goodsStone)
            {
                saleAmount = GoodsManager.goodsStone;
            }
            else
            {
                saleAmount += amount;
            }
            saleAmountText.text = saleAmount.ToString();
            expectArAmount.text = $"아르 획득량: {(int)(GoodsManager.exchangeRate * saleAmount * 0.97)}";
        }
    }
}
