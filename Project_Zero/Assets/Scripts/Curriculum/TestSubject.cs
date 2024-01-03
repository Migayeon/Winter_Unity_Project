using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSubject : MonoBehaviour
{
    void Start()
    {
        
    }
    void Update()
    {
        
    }
    public void TestS()
    {
        List<SubjectSystem.Subject> test=new List<SubjectSystem.Subject>();
        test=SubjectSystem.ReadSubject();
    }
}
