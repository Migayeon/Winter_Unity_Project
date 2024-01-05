using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class ClassEx : MonoBehaviour
{
    public GameObject[] managementButton = new GameObject[4];

    public Button ClassExp;
    public Button OfficeExp;
    public Button marketingA;
    public Button marketingB;

    public void ClassExpan()
    {
        int Ar = 5000;
        if(GoodsManager.goodsAr - Ar < 0)
        {
            Debug.Log("파산");
        }
        else
        {
            GoodsManager.goodsAr -= Ar;
        }


    }

    private void Awake()
    {
        //ClassExp.onClick.AddListener();
    }





}
