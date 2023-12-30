using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cat : MonoBehaviour
{
    private int i;
    void Start()
    {
        Debug.Log("meow");
    }
    void Update()
    {
        Debug.Log(i++);
    }
}
