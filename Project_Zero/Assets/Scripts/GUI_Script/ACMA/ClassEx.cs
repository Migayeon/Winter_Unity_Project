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
    public Text requiredClass;
    public Text requiredProfessor;
    public Text requiredMarketA;
    public Text requiredMarketB;
    public Text requiredMarketC;
    public Text requiredMarketD;
    public Text preProfessor;
    public Text preStudent;
    int marketingTurn = 3;


    static int duringMarketA = -10;
    static int duringMarketB = -10;
    static int duringMarketC = -10;
    static int duringMarketD = -10;



    static private int classAr = 5000;
    static private int officeAr = 5000;
    static private int marA = 5000;
    static private int marB = 10000;
    static private int marC = 20000;
    static private int marD = 40000;


    public void ClassExpan()
    {
        int num = 50;
        int count = PlayerInfo.ProfessorCount();
        if (GoodsManager.goodsAr - classAr < 0)
        {
            Debug.Log("파산");
            window.SetActive(true);
            windowWord.text = "Ar가 부족합니다";
            
        }
        else
        {
            GoodsManager.goodsAr -= classAr;
            classAr *= 2;
            PlayerInfo.maxStudent += num;
            requiredClass.text = $"{classAr}";
            preStudent.text = $"{count}/{PlayerInfo.maxStudent}";
        }
    }

    public void OfficeExpan()
    {
        
        int num = 50;
        int professCount = PlayerInfo.ProfessorCount();
        if (GoodsManager.goodsAr - officeAr < 0)
        {
            Debug.Log("파산");
            window.SetActive(true);
            windowWord.text = "Ar가 부족합니다";
        }
        else
        {
            
            GoodsManager.goodsAr -= officeAr;
            PlayerInfo.maxProfessor += num;
            officeAr *= 2;
            requiredProfessor.text = $"{officeAr}";
            preProfessor.text = $"{professCount}/{PlayerInfo.maxProfessor}";
        }

    }

    public void marketA()
    {
        int num = 10;
        
        if (TurnManager.turn - duringMarketA < marketingTurn)
        {
            Debug.Log("마케팅 진행중");
            Debug.Log($"남은 턴 수 : {-TurnManager.turn + duringMarketA + marketingTurn}");
            window.SetActive(true);
            //windowWord.text = $"";
        }
        else
        {
            if (GoodsManager.goodsAr - marA < 0)
            {
                Debug.Log("파산");
                windowWord.text = "Ar가 부족합니다";
                window.SetActive(true);


            }
            else
            {
                GoodsManager.goodsAr -= marA;
                GoodsManager.goodsConstFame += num;
                marA += 1000;
                requiredMarketA.text = $"{marA}";
                duringMarketA = TurnManager.turn;
            }
        }
    }
    public void marketB()
    {
        int num = 10;

        if(TurnManager.turn - duringMarketB < marketingTurn)
        {
            Debug.Log("마케팅 진행중");
            Debug.Log($"남은 턴 수 : {-TurnManager.turn + duringMarketB + marketingTurn}");
        }
        else
        {
            if (GoodsManager.goodsAr - marB < 0)
            {
                Debug.Log("파산");
                window.SetActive(true);
                windowWord.text = "Ar가 부족합니다";

            }   
            else
            {
                GoodsManager.goodsAr -= marB;
                GoodsManager.goodsConstFame += num;
                marB += 1000;
                requiredMarketB.text = $"{marB}";
                duringMarketB = TurnManager.turn;
            }
        }
    }

    public void marketC()
    {
        int num = 10;

        if (TurnManager.turn - duringMarketC < marketingTurn)
        {
            Debug.Log("마케팅 진행중");
            Debug.Log($"남은 턴 수 : {-TurnManager.turn + duringMarketC + marketingTurn}");
        }
        else
        {
            if (GoodsManager.goodsAr - marC < 0)
            {
                Debug.Log("파산");
                windowWord.text = "Ar가 부족합니다";
                window.SetActive(true);

            }
            else
            {
                GoodsManager.goodsAr -= marC;
                GoodsManager.goodsConstFame += num;
                marC += 1000;
                requiredMarketC.text = $"{marC}";
                duringMarketC = TurnManager.turn;
            }
        }
    }
    public void marketD()
    {
        int num = 10;

        if (TurnManager.turn - duringMarketD < marketingTurn)
        {
            Debug.Log("마케팅 진행중");
            Debug.Log($"남은 턴 수 : {-TurnManager.turn + duringMarketD + marketingTurn}");
        }
        else
        {
            if (GoodsManager.goodsAr - marD < 0)
            {
                Debug.Log("파산");
                windowWord.text = "Ar가 부족합니다";
                window.SetActive(true);

            }
            else
            {
                GoodsManager.goodsAr -= marD;
                GoodsManager.goodsConstFame += num;
                marD += 1000;
                requiredMarketD.text = $"{marD}";
                duringMarketC = TurnManager.turn;
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

        requiredClass.text = $"{classAr}";
        requiredProfessor.text = $"{officeAr}";
        preStudent.text = $"{studentCount}/{PlayerInfo.maxStudent}";
        preProfessor.text = $"{professCount}/{PlayerInfo.maxProfessor}";

        requiredMarketA.text = $"{marA}";
        requiredMarketB.text = $"{marB}";
        requiredMarketC.text = $"{marC}";
        requiredMarketD.text = $"{marD}";
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
