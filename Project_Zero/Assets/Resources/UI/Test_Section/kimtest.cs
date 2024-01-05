using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]

class info
{
    public int testclass;
    public string testname;
    public List<int> require = new List<int>();
}

public class kimtest : MonoBehaviour
{
    public GameObject testcase;
    public Transform content;

    List<int> getStat()
    {
        List<int> stat = new List<int>();
        return stat;
        // [이론, 마나, 손재주, 원소, 영창]
        // [theory, mana, craft, element, attack]
    }

    // Start is called before the first frame update
    void Start()
    {
        //Instantiate(prefab);
        for (int i = 1; i < 16; i++)
        {
            var loadedJson = Resources.Load<TextAsset>("TestCase/" + i.ToString());
            
            info testInfo = JsonUtility.FromJson<info>(loadedJson.ToString());
            Debug.Log($"{testInfo.testclass}, {testInfo.testname}, {testInfo.require}");

            var loadedSprite = Resources.Load<Sprite>("UI/Test_Section/" + testInfo.testclass.ToString());
            Debug.Log(testInfo.testclass.ToString());
            GameObject test = Instantiate(testcase, content);

            test.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = testInfo.testname;
            test.transform.GetChild(2).GetComponent<Image>().sprite = loadedSprite;
        }
    }

    public void afterStudentClicked()
    {
        getStat();
        // 학생 스텟 기반으로 확률 계산
        // 진학사처럼 표현
    }
}
