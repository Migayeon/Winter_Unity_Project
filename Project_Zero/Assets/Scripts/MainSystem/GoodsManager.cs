using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodsManager : MonoBehaviour
{
    // 기본 재화
    public static int goodsAr = 20000;
    public static int goodsStone = 0;
    public static int goodsFame = 1;
    public static int goodsStudent = 0;

    // 아르<->마정석 환전
    public static int exchangePercent = 0;
    public static int exchangeRate = 500;
    public static int maxRate = 30;
    public static int minRate = -25;

    void Awake()
    {
        // json 저장 시스템 구현되면 try ~ except문으로 각 재화 초기화
    }
    public static void UpdateUI()
    {
        // UI 구현되면 각 UI text에 value값 대입
    }
}
