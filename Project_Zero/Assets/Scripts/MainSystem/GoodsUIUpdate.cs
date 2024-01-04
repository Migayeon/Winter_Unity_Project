using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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
    public static void HideUI()
    {
        GameObject.Find("CurrentTurn").SetActive(false);
        GameObject.Find("Ar").SetActive(false);
        GameObject.Find("Stone").SetActive(false);
    }

    public static void ShowUI()
    {
        GameObject.Find("CurrentTurn").SetActive(true);
        GameObject.Find("Ar").SetActive(true);
        GameObject.Find("Stone").SetActive(true);
    }
}
