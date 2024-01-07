using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AfterTurn : MonoBehaviour
{
    public Text turnText;           // 턴
    public Text studentRevenue;     // 학원비 수입
    public Text magicStoneRevenue;  // 마정석 판매 수입
    public Text professorRevenue;   // 교수 수입
    public Text professorCost;      // 교수 봉급 지급
    public Text academyCost;        // 학원 증축비
    public Text marketingCost;      // 마케팅 사용 금액
    public Text magicStoneCost;     // 마정석 구입 비용
    public Text totalResult;        // 총 결산

    int student_Rev;
    public static int magic_Rev = 0;
    int professor_Rev;
    int professor_Cost;
    public static int academy_Cost = 0;
    int marketing_Cost;
    public static int magic_Cost = 0;
    int total_Result;


    void Awake()
    { 
        // 각종 재화 변동 필요함...

        // 학생들 커리큘럼 진행
        student_Rev = GoodsManager.goodsAr;
        foreach (StudentGroup[] period in PlayerInfo.studentGroups)
        {
            foreach (StudentGroup group in period)
            {
                group.CurriculumSequence();
            }
        }
        student_Rev -= GoodsManager.goodsAr;

        // 마장석 구매 / 판매시 정산 완료. ( StockManager.cs 참고 )


        // 교수 출장으로 돈 벌어오는 시스템 구현 바람... To 시후
        professor_Rev = 0;


        // 교수 봉급 지급
        professor_Cost = GoodsManager.goodsAr;
        foreach(ProfessorSystem.Professor professor in PlayerInfo.ProfessorList)
        {
            professor.ProfessorSetDefaultSalary();
            GoodsManager.goodsAr -= professor.ProfessorGetSalary();
        }
        professor_Cost -= GoodsManager.goodsAr;

        // 건물 증축시 비용 적용 완료... ( ClassEx.cs 참고 )

        // 마케팅 비용 정산하는 것 구현 바람... To 동엽
        marketing_Cost = 0;

        // 총 결산 진행.
        total_Result = student_Rev + professor_Rev + magic_Rev +
            professor_Cost + marketing_Cost + magic_Cost + academy_Cost;

        // 정산창에 텍스트 적용
        turnText.text = TurnManager.turn.ToString();
        studentRevenue.text = string.Format("{0:N0}",student_Rev);
        professorRevenue.text = string.Format("{0:N0}",professor_Rev);
        magicStoneRevenue.text = string.Format("{0:N0}", magic_Rev);
        professorCost.text = string.Format("{0:N0}", professor_Cost);
        marketingCost.text = string.Format("{0:N0}", marketing_Cost);
        magicStoneCost.text = string.Format("{0:N0}", magic_Cost);
        academyCost.text = string.Format("{0:N0}", academy_Cost);
        totalResult.text = string.Format("{0:N0}", total_Result);
        if(total_Result > 0)
        {
            totalResult.color = new Color(32f,131f,32f);
            totalResult.text = "- " + totalResult.text;
        }
        else
        {
            totalResult.color = Color.red;
        }


        // 정산용 변수 초기화
        magic_Rev = 0;
        academy_Cost = 0;
        magic_Cost = 0;
        total_Result = 0;

        // BeforeTurn 불러오기, 1턴 추가
        TurnManager.turn++;
        //SceneManager.LoadScene("Curriculum");
    }
}
