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
            CalculateEndedFame();
        }
    }
    public static int goodsCalculatedEndedFame = 0;
    public static int goodsStudent = 0;

    // 아르<->마정석 환전
    public static int exchangePercent = 0;
    public static int exchangeRate = 500;
    public static int maxRate = 30;
    public static int minRate = -25;
    public static void CalculateEndedFame()
    {
        int passedRatio = 1;
        if (PlayerInfo.GraduatedStudentTotalNum() > 0)
        {
            passedRatio = ((PlayerInfo.nineSuccess +
            2 * PlayerInfo.sevenSuccess * PlayerInfo.sevenSuccess + 10 * PlayerInfo.fiveSuccess * PlayerInfo.fiveSuccess
            / PlayerInfo.GraduatedStudentTotalNum()));
        }
        Debug.Log(passedRatio);
        int topPfNum = 3;
        List<ProfessorSystem.Professor> pfSortedList = PlayerInfo.ProfessorList.OrderByDescending(x => x.ProfessorGetSalary()).ToList();
        int topPfStatSum = 0;
        if (pfSortedList.Count < 3){ topPfNum = pfSortedList.Count; }
        else { topPfNum = 3; }
        for (int i=0;i< topPfNum; i++)
        {
            topPfStatSum += pfSortedList[i].ProfessorGetSalary();
        }
        short openSubjectNum = 0;
        List<SubjectTree.State> openSubjectList = SubjectTree.subjectState;
        for (int i=0;i<openSubjectList.Count;i++)
        {
            if (openSubjectList[i] == SubjectTree.State.Open)
            {
                openSubjectNum++;
            }
        }
        if (topPfNum > 0)
        {
            Debug.Log("Im okay");
            Debug.Log($"{topPfStatSum} , {topPfNum} , {openSubjectNum} ");
            goodsCalculatedEndedFame = ((topPfStatSum / topPfNum)/10) + openSubjectNum + passedRatio + GoodsConstFame;
        }
        else
        {
            goodsCalculatedEndedFame = GoodsConstFame + passedRatio;
        }
        Debug.Log($"명성: {goodsCalculatedEndedFame}");
    }
}
