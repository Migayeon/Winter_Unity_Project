using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moominTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SubjectTree.initSubjectsAndInfo();
        SubjectTree.callOnlyOneTimeWhenGameStart();
        SubjectTree.initSubjectStates(new List<int>());
        string json = SubjectTree.save();
        print(json);
        SubjectTree.initSubjectsAndInfo();
        SubjectTree.callOnlyOneTimeWhenGameStart();
        SubjectTree.initSubjectStates(new List<int>());
        SubjectTree.load(json);
        print(SubjectTree.save());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
