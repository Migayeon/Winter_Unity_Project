using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TestSubject : MonoBehaviour
{
    [System.Serializable]
    public class Test
    {
        public int id;
        public int tier;
        public string subjectName;
        public List<string> enforceType;
        public List<int> enforceAmount;
        public List<int> nextAvailableID;
        public int needToBeAvailable;
    }

    public void LoadJSON()
    {
        TextAsset loadedJson = Resources.Load<TextAsset>("Subjects/subject");
        List<Test> myTest = JsonUtility.FromJson<List<Test>>(loadedJson.ToString());
        Debug.Log(myTest);
    }
    /*
    public void TestS()
    {
        List<SubjectSystem.Subject> test=new List<SubjectSystem.Subject>();
        test=SubjectSystem.ReadSubject();
    }
    */
}
