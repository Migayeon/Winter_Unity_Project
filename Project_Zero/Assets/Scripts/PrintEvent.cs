using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Data;

public class PrintEvent : MonoBehaviour
{
    [SerializeField] Button button;
    DataTable dt = new DataTable();
    void Start()
    {
        button.onClick.AddListener(GetRandomEvent);
    }    
    public void GetRandomEvent()
    {
        string t0 = $"";
        string t1 = $"{GoodsManager.goodsAr}+10";
        t0 += t1;
        Debug.Log(t0);
        object res = dt.Compute(t1, "");
        Debug.Log(res);
    }
}