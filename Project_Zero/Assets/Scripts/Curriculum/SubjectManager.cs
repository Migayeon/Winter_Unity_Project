using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public static class SubjectTree
{
    /* << Attributes >>
     *  - List<Subject> subjects : 과목들을 저장합니다.
     *  - Dictionary<string, int> subjectsInfo : 전체적인 정보를 저장합니다.
     *  - ulong subjectState : 어떤 과목이 현재 열려있는 지 저장합니다.
     *  
     *  << Funtions >>
     *  - void init() : 정보를 json파일로부터 불러옵니다.
     *  
     *  - bool isSubjectOpen(int id) : 강좌가 개설되어 있는 지 확인합니다.
     *  - void openSubject(int id) : 강좌를 개설합니다.
     *  - void closeSubject(int id) : 강좌를 폐쇄합니다.
     *  - void changeSubjectState(int id) : 강좌 상태를 변경합니다.
     *  
     *  - List<int> getEnforceType(int id) : return EnforceType
     *  
     *  - bool isVaildCurriculum(List<int> subjectsId) : 해당 커리큘럼이 유효한 지 판단하여 반환합니다.
     */
    public static List<Subject> subjects = new List<Subject>();
    public static Dictionary<string, int> subjectsInfo;
    public static List<bool> subjectState = new List<bool>();
    public static int subjectsCount = 0;
    public static void init()
    {
        subjects = new List<Subject>();
        TextAsset loadedJson = Resources.Load<TextAsset>("Subjects/subjectsInfo.json");
        subjectsInfo = JsonUtility.FromJson<Dictionary<string, int>>(loadedJson.ToString());
        subjectsCount = subjectsInfo["Count"];
        for (int i = 0; i < subjectsCount; i++)
        {
            loadedJson = Resources.Load<TextAsset>(string.Concat("Subjects/", i.ToString(), ".json"));
            subjects.Add(JsonUtility.FromJson<Subject>(loadedJson.ToString()));
        }
    }

    public static bool isSubjectOpen(int id)
    {
        return subjectState[id];
    }
    public static void openSubject(int id)
    {
        subjectState[id] = true;
    }
    public static void closeSubject(int id)
    {
        subjectState[id] = false;
    }
    public static void changeSubjectState(int id)
    {
        subjectState[id] = !subjectState[id];
    }
    public static int getTier(int id)
    {
        return subjects[id].tier;
    }
    public static string getSubjectName(int id)
    {
        return subjects[id].subjectName;
    }

    public static List<int> getEnforceType(int id)
    {
        return subjects[id].enforceType;
    }
    public static List<int> getEnforceAmount(int id)
    {
        return subjects[id].enforceAmount;
    }
    public static List<int> getNextAvailableId(int id)
    {
        return subjects[id].nextAvailableId;
    }

    public static int relativeSubjectGroupId(int id)
    public static int root;
    public static int needToBeAvailable;

    private static List<int> newCntList()
    {
        List<int> rst = new List<int>();
        for (int i = 0; i < subjectsCount; i++)
            rst.Add(subjects[i].needToBeAvailable);
        return rst;
    }
    private static List<bool> flattenList(List<int> idList)
    {
        List<bool> rst = new List<bool>(subjectsCount);
        for (int i = 0; i < idList.Count; i++)
            rst[idList[i]] = true;
        return rst;
    }

    public static bool isVaildCurriculum(List<int> subjectsId)
    {
        List<int> cntList = newCntList();
        List<bool> flatIdList = flattenList(subjectsId);
        List<bool> flatSearchList = flattenList(new List<int>());
        Queue<int> searchQ = new Queue<int>();
        for (int i = 0; i < subjectsCount; i++)
        {
            if (subjects[i].needToBeAvailable == 0 && flatIdList[i])
            {
                searchQ.Enqueue(i);
                flatSearchList[i] = true;
            }
        }
        while (searchQ.Count > 0)
        {
            int nowNodeId = searchQ.Dequeue();
            cntList[nowNodeId]--;
            List<int> next = subjects[nowNodeId].nextAvailableId;
            for (int i = 0; i < next.Count; i++)
            {
                int index = next[i];
                if (cntList[index] == 0 && !flatSearchList[index] && flatIdList[index])
                    searchQ.Enqueue(index);
            }
        }
        List<bool> isSameGroup = new List<bool>(subjectsInfo["groupCount"]);
        foreach (int id in subjectsId)
        {
            if (subjects[id].relativeSubjectGroupId != -1)
            {
                if (!isSameGroup[subjects[id].relativeSubjectGroupId])
                    isSameGroup[subjects[id].relativeSubjectGroupId] = true;
                else
                    return false;
            }
            if (!flatSearchList[id])
                return false;
        }
        return true;
    }
}

[System.Serializable]
public class Subject
{
    public int id;
    public int tier;
    public string subjectName;
    public List<int> enforceType;
    public List<int> enforceAmount;
    public List<int> nextAvailableId;
    public int relativeSubjectGroupId;
    public int root;
    public int needToBeAvailable;

    public Subject(int id, int tier, string subjectName, List<int> enforceType, List<int> enforceAmount, List<int> nextAvailableId, int relativeSubjectGroupId, int root, int needToBeAvailable)
    {
        this.id = id;
        this.tier = tier;
        this.subjectName = subjectName;
        this.enforceType = enforceType;
        this.enforceAmount = enforceAmount;
        this.nextAvailableId = nextAvailableId;
        this.relativeSubjectGroupId = relativeSubjectGroupId;
        this.root = root;
        this.needToBeAvailable = needToBeAvailable;
    }
}

public class SubjectManager : MonoBehaviour
{
   
}


