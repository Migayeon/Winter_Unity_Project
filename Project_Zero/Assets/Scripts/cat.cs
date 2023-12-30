using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Security.Cryptography;

public class cat : MonoBehaviour
{
    private int i;
    [SerializeField]
    private int catCnt;
    void Start()
    {
        for(int k = 0; k < catCnt; k++)
            Debug.Log("meow");
    }
    void Update()
    {
        for (int k = 0; k < catCnt; k++)
            Debug.Log("grrr");
        Debug.Log(i++);
    }
}
