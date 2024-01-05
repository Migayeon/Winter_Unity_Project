using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class ClassEx : MonoBehaviour
{

    public Button ClassExp;
    public Button OfficeExp;
    public Button marketingA;
    public Button marketingB;
    public Text requiredClass;
    public Text requiredProfessor;
    public Text requiredMarketA;
    public Text requiredMarketB;


    static private int classAr = 5000;
    static private int officeAr = 5000;


    public void ClassExpan()
    {
        int num = 50;
        
        if(GoodsManager.goodsAr - classAr < 0)
        {
            Debug.Log("파산");
        }
        else
        {
            GoodsManager.goodsAr -= classAr;
            classAr *= 2;
            PlayerInfo.maxStudent += num;
            requiredClass.text = $"{classAr}";
        }
    }

    public void OfficeExpan()
    {
        
        int num = 50;
        if(GoodsManager.goodsAr - officeAr < 0)
        {
            Debug.Log("파산");
        }
        else
        {
            
            GoodsManager.goodsAr -= officeAr;
            PlayerInfo.maxProfessor += num;
            officeAr *= 2;
            requiredProfessor.text = $"{officeAr}";
        }

    }

    public void marketA()
    {


    }
    public void marketB()
    {

    }


    private void Awake()
    {
        requiredClass.text = $"{classAr}";
        requiredProfessor.text = $"{officeAr}";
        ClassExp.onClick.AddListener(ClassExpan);
        
        OfficeExp.onClick.AddListener(OfficeExpan);
    }





}
