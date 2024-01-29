using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class ClassEx : MonoBehaviour
{
    public GameObject window;
    public Text windowWord;
    public Button checkUI;

    public Button ClassExp;
    public Button OfficeExp;
    public Button marketingA;
    public Button marketingB;
    public Button marketingC;
    public Button marketingD;
    public Text requiredClassAr;
    public Text requiredClassMagicStone;
    public Text requiredProfessorAr;
    public Text requiredProfessorMagicStone;
    public Text requiredMarketA;
    public Text requiredMarketB;
    public Text requiredMarketC;
    public Text requiredMarketD;
    public Text preProfessor;
    public Text preStudent;
    public Text howManyStudentIncrease;
    public Text howManyProfessorIncrease;
    int marketingTurn = 3;
    int numA = 50;
    int numB = 5;


    static int duringMarketA = -10;
    static int duringMarketB = -10;
    static int duringMarketC = -10;
    static int duringMarketD = -10;



    static private int classAr = 5000;
    static private int classMagicStone = 10;
    static private int officeAr = 5000;
    static private int officeMagicStone = 10;
    static public int marA = 5000;
    static public int marB = 10000;
    static public int marC = 20000;
    static public int marD = 40000;

    void bankRunPos()
    {
        Debug.Log("파산");
        window.SetActive(true);
        windowWord.rectTransform.localPosition = new Vector3(0, 4, 0);
        windowWord.text = "Ar가 부족합니다";
    }
    void alreadyMarketing(int duringTurn)
    {
        Debug.Log("마케팅 진행중");
        Debug.Log($"남은 턴 수 : {-TurnManager.turn + duringTurn + marketingTurn}");
        windowWord.text = "이미 마케팅이 진행중입니다\n" +
            $"남은 턴수 : {-TurnManager.turn + duringTurn + marketingTurn}";
        window.SetActive(true);
        windowWord.rectTransform.localPosition = new Vector3(0, 12, 0);
    }

    void magicStonePos()
    {
        window.SetActive(true);
        windowWord.rectTransform.localPosition = new Vector3(0, 4, 0);
        windowWord.text = "마정석이 부족합니다";
    }

    public void ClassExpan()
    {
        
        int count = PlayerInfo.StudentGroupCount();
        if (GoodsManager.goodsAr - classAr < 0)
        {
            bankRunPos();
        }
        else if(GoodsManager.goodsStone - classMagicStone < 0)
        {
            magicStonePos();
        }
        else
        {
            GoodsManager.goodsAr -= classAr;
            AfterTurn.academy_Cost -= classAr;
            classAr *= 2;
            classMagicStone += 5;
            PlayerInfo.maxStudent += numA;
            requiredClassMagicStone.text = $"마정석{classMagicStone}개 필요";
            requiredClassAr.text = $"{classAr}Ar 필요";
            preStudent.text = $"{count}/{PlayerInfo.maxStudent}명";
        }
    }

    public void OfficeExpan()
    {
        
        
        int professCount = PlayerInfo.ProfessorCount();
        if (GoodsManager.goodsAr - officeAr < 0)
        {
            bankRunPos();
        }
        else if (GoodsManager.goodsStone - officeMagicStone < 0)
        {
            magicStonePos();
        }
        else
        {
            
            GoodsManager.goodsAr -= officeAr;
            AfterTurn.academy_Cost -= officeAr;
            PlayerInfo.maxProfessor += numB;
            officeAr *= 2;
            classMagicStone += 5;
            requiredProfessorAr.text = $"{officeAr}Ar 필요";
            requiredClassMagicStone.text = $"마정석{classMagicStone}개 필요";
            preProfessor.text = $"{professCount}/{PlayerInfo.maxProfessor}명";
        }

    }

    public void marketA()
    {
        int num = 10;
        
        if (TurnManager.turn - duringMarketA < marketingTurn)
        {
            alreadyMarketing(duringMarketA);
        }
        else
        {
            if (GoodsManager.goodsAr - marA < 0)
            {
                bankRunPos();


            }
            else
            {
                GoodsManager.goodsAr -= marA;
                GoodsManager.GoodsConstFame += num;
                marA += 1000;
                requiredMarketA.text = $"{marA}Ar 필요";
                duringMarketA = TurnManager.turn;

            }
        }
    }
    public void marketB()
    {
        int num = 10;

        if(TurnManager.turn - duringMarketB < marketingTurn)
        {
            alreadyMarketing(duringMarketB);
        }
        else
        {
            if (GoodsManager.goodsAr - marB < 0)
            {
                bankRunPos();
            }   
            else
            {
                GoodsManager.goodsAr -= marB;
                GoodsManager.GoodsConstFame += num;
                marB += 1000;
                requiredMarketB.text = $"{marB}Ar 필요";
                duringMarketB = TurnManager.turn;
            }
        }
    }

    public void marketC()
    {
        int num = 10;

        if (TurnManager.turn - duringMarketC < marketingTurn)
        {
            alreadyMarketing(duringMarketC);
        }
        else
        {
            if (GoodsManager.goodsAr - marC < 0)
            {
                bankRunPos();

            }
            else
            {
                GoodsManager.goodsAr -= marC;
                GoodsManager.GoodsConstFame += num;
                marC += 1000;
                requiredMarketC.text = $"{marC}Ar 필요";
                duringMarketC = TurnManager.turn;
            }
        }
    }
    public void marketD()
    {
        int num = 10;

        if (TurnManager.turn - duringMarketD < marketingTurn)
        {
            alreadyMarketing(duringMarketD);
        }
        else
        {
            if (GoodsManager.goodsAr - marD < 0)
            {
                bankRunPos();

            }
            else
            {
                GoodsManager.goodsAr -= marD;
                GoodsManager.GoodsConstFame += num;
                marD += 1000;
                requiredMarketD.text = $"{marD}Ar 필요";
                duringMarketD = TurnManager.turn;
            }
        }
    }

    public void checkUi()
    {
        window.SetActive(false);
    }
    

    private void Awake()
    {
        

        int studentCount = PlayerInfo.StudentGroupCount();
        int professCount = PlayerInfo.ProfessorCount();

        requiredClassAr.text = $"{classAr}Ar 필요";
        requiredClassMagicStone.text = $"마정석{classMagicStone}개 필요";
        requiredProfessorAr.text = $"{officeAr}Ar 필요";
        requiredProfessorMagicStone.text = $"마정석{officeMagicStone}개 필요";
        preStudent.text = $"{studentCount}/{PlayerInfo.maxStudent}명";
        preProfessor.text = $"{professCount}/{PlayerInfo.maxProfessor}명";
        howManyStudentIncrease.text = $"최대 학생수가 {numA}만큼 증가합니다.";
        howManyProfessorIncrease.text = $"최대 교수수가 {numB}만큼 증가합니다.";

        requiredMarketA.text = $"{marA}Ar 필요";
        requiredMarketB.text = $"{marB}Ar 필요";
        requiredMarketC.text = $"{marC}Ar 필요";
        requiredMarketD.text = $"{marD}Ar 필요";
        ClassExp.onClick.AddListener(ClassExpan);
        OfficeExp.onClick.AddListener(OfficeExpan);
        marketingA.onClick.AddListener(marketA);
        marketingB.onClick.AddListener(marketB);
        marketingC.onClick.AddListener(marketC);
        marketingD.onClick.AddListener(marketD);

        checkUI.onClick.AddListener(checkUi);
        window.SetActive(false);
        



    }





}
