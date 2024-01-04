using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GoodsUIUpdate : MonoBehaviour
{
    // 재화 관련 UI 텍스트를 실제 수치에 맞게 적용
    [SerializeField] TextMeshProUGUI currentTurn;
    [SerializeField] TextMeshProUGUI currentAr;
    [SerializeField] TextMeshProUGUI currentStone;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    void Update()
    {
        currentTurn.text = $"{TurnManager.turn}";
        currentAr.text = $"{GoodsManager.goodsAr}";
        currentStone.text = $"{GoodsManager.goodsStone}";
    }
}
