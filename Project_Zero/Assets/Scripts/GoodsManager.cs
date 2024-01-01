using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodsManager : MonoBehaviour
{
    private int goodsAr { get; set; }
    private int goodsStone { get; set; }
    private int goodsFame { get; set; }
    private int goodsStudent { get; set; }
    void Start()
    {
        goodsAr = 0;
        goodsStone = 0;
        goodsFame = 0;
        goodsStudent = 0;
        // json 구현되면 try ~ except문으로 각 재화 초기화
    }
    public void GoodsChange(string goodsType, int rate)
    {
        if (goodsType == "Ar") goodsAr += rate;
        if (goodsType=="Stone") goodsStone += rate;
        if (goodsType=="Fame") goodsFame += rate;
        if (goodsType=="Student") goodsStudent += rate;
    }
    public static void UpdateUI()
    {
        // UI 구현되면 각 UI text에 value값 대입
    }
}
