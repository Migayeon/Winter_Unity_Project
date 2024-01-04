using System;
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
    public enum State
    {
        Closed,
        Open,
        ReadyToOpen
    }
    private const int DONT_HAVE_GROUP = -1;
    public static List<Subject> subjects = new List<Subject>();
    public static SubjectInfo subjectsInfo;
    public static List<State> subjectState = new List<State>();
    public static int subjectsCount = 0;
    
    public static void initSubjectsAndInfo()
    {
        subjects = new List<Subject>();
        string infoFilePath = Path.Combine(Application.dataPath, "Resources/Subjects/subjectsInfo.json");
        string loadJson = File.ReadAllText(infoFilePath);
        subjectsInfo = JsonUtility.FromJson<SubjectInfo>(loadJson);
        subjectsCount = subjectsInfo.count;
        for (int i = 0; i < subjectsCount; i++)
        {
            string subjectPath = Path.Combine(Application.dataPath, "Resources/Subjects/" + i.ToString() + ".json");
            loadJson = File.ReadAllText(subjectPath);
            subjects.Add(JsonUtility.FromJson<Subject>(loadJson));
        }
    }

    public static State isSubjectOpen(int id)
    {
        return subjectState[id];
    }
    public static void openSubject(int id)
    {
        subjectState[id] = State.Open;
        if (subjects[id].relativeSubjectGroupId != DONT_HAVE_GROUP) {
            for (int i = 0; i < subjectsCount; i++)
            {
                if (subjects[i].relativeSubjectGroupId == subjects[id].relativeSubjectGroupId)
                    subjectState[id] = State.Closed;
            }
        }
        foreach (int nextNode in subjects[id].nextAvailableId)
        {
            subjectState[nextNode] = State.ReadyToOpen;
        }
    }
    public static void closeSubject(int id)
    {
        subjectState[id] = State.ReadyToOpen;
        if (subjects[id].relativeSubjectGroupId != DONT_HAVE_GROUP)
        {
            for (int i = 0; i < subjectsCount; i++)
            {
                if (subjects[i].relativeSubjectGroupId == subjects[id].relativeSubjectGroupId)
                    subjectState[id] = State.ReadyToOpen;
            }
        }
        foreach (int nextNode in subjects[id].nextAvailableId)
        {
            subjectState[nextNode] = State.Closed;
        }
    }

    public static Subject getSubject(int id)
    {
        return subjects[id];
    }

    private static List<int> newCntList()
    {
        List<int> rst = new List<int>();
        for (int i = 0; i < subjectsCount; i++)
            rst.Add(subjects[i].needToBeAvailable);
        return rst;
    }
    private static List<bool> flattenList(List<int> idList)
    {
        List<bool> rst = new List<bool>();
        for (int i = 0; i < subjectsCount; i++)
            rst.Add(false);
        for (int i = 0; i < idList.Count; i++)
            rst[idList[i]] = true;
        return rst;
    }
    public static List<State> initSubjectStates(List<int> openedSubjectsId)
    {
        List<State> rst = new List<State>();
        for (int i = 0; i < subjectsCount; i++)
            rst.Add(State.Closed);
        for (int i = 0; i < openedSubjectsId.Count; i++)
            rst[openedSubjectsId[i]] = State.Open;
        List<int> cntList = newCntList();
        List<bool> flatSearchList = flattenList(new List<int>());
        List<bool> isSameGroup = new List<bool>(subjectsInfo.groupCount);
        Queue<int> searchQ = new Queue<int>();
        for (int i = 0; i < subjectsCount; i++)
        {
            if (subjects[i].needToBeAvailable == 0)
            {
                if (rst[i] == State.Open)
                {
                    searchQ.Enqueue(i);
                    flatSearchList[i] = true;
                }
                else
                {
                    rst[i] = State.ReadyToOpen;
                }
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
                rst[index] = State.ReadyToOpen;
                if (cntList[index] == 0 && rst[i] == State.Open)
                {
                    searchQ.Enqueue(index);
                    flatSearchList[i] = true;
                }
            }
            if (subjects[nowNodeId].relativeSubjectGroupId != DONT_HAVE_GROUP)
            {
                if (!isSameGroup[subjects[nowNodeId].relativeSubjectGroupId] && rst[nowNodeId] == State.Open)
                    isSameGroup[subjects[nowNodeId].relativeSubjectGroupId] = true;
                else
                    rst[nowNodeId] = State.Closed;
            }
        }
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
            Debug.Log("nowNodeId : " + nowNodeId.ToString());
            List<int> next = subjects[nowNodeId].nextAvailableId;
            for (int i = 0; i < next.Count; i++)
            {
                int index = next[i];
                if (cntList[index]-- == 0 && flatIdList[index])
                {
                    searchQ.Enqueue(index);
                    flatSearchList[index] = true;
                }
            }
        }

        List<bool> isSameGroup = new List<bool>(subjectsCount);
        foreach (int id in subjectsId)
        {
            if (subjects[id].relativeSubjectGroupId != DONT_HAVE_GROUP)
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
public class SubjectInfo
{
    public int count;
    public string[] enforceTypeName;
    public int groupCount;
    public SubjectInfo(int Count, string[] EnforceTypeName, int GroupCount)
    {
        count = Count;
        enforceTypeName = EnforceTypeName;
        groupCount = GroupCount;
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
    public void Awake()
    {
        SubjectTree.initSubjectsAndInfo();
    }
}


