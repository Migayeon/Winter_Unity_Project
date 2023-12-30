using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Security.Cryptography;

public class cat : MonoBehaviour
{
    private int i;
    void Start()
    {
        Debug.Log("meow");
    }
    void Update()
    {
        Debug.Log("grrr");
        Debug.Log(i++);
    }
}
