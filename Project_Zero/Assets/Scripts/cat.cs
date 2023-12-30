using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Security.Cryptography;

public class cat : MonoBehaviour
{
    private int i;
    private GameObject catObject;
    void Start()
    {
        catObject = gameObject;
        Debug.Log("meow");
    }
    void Update()
    {
        Debug.Log("grrr");
        Debug.Log(i++);
        Debug.Log(catObject.name);
    }
}
