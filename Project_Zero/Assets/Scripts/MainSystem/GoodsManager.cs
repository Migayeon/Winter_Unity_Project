using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class GoodsManager : MonoBehaviour
{
    // 기본 재화
    public static int goodsAr = 20000;
    public static int goodsStone = 0;
    public static int goodsConstFame = 10;
    public static int GoodsConstFame
    {
        get { return goodsConstFame; }
        set
        {
            if (value < 10)
            {
                goodsConstFame = 10;
            }
            else
            {
                goodsConstFame = value;
            }
        }
    }
    public static int goodsCalculatedEndedFame = 0;
    public static int goodsStudent = 0;

    // 아르<->마정석 환전
    public static int exchangePercent = 0;
    public static int exchangeRate = 500;
    public static int maxRate = 30;
    public static int minRate = -25;
    private void CalculateEndedFame()
    {
        List<ProfessorSystem.Professor> pfList = PlayerInfo.ProfessorList;
        short openSubjectNum = 0;
        List<SubjectTree.State> openSubjectList = SubjectTree.subjectState;
        for (int i=0;i<openSubjectList.Count;i++)
        {
            if (openSubjectList[i] == SubjectTree.State.Open)
            {
                openSubjectNum++;
            }
        }
        
    }
    void Start()
    {
        // json 저장 시스템 구현되면 try ~ except문으로 각 재화 초기화
    }

    void Update()
    {
        // UI 구현되면 각 UI text에 value값 대입

    }
}
