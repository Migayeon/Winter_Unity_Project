using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
public class SubjectManager : MonoBehaviour
{
    public void Awake()
    {
        SubjectTree.initSubjectsAndInfo();
        Subject test = SubjectTree.getSubject(0);
        print(test.enforceContents);
    }
}
public static class SubjectTree
{
    /* << Attributes >>
     *  - List<Subject> subjects : 과목들을 저장합니다.
     *  - Dictionary<string, int> subjectsInfo : 전체적인 정보를 저장합니다.
     *  - List<State> subjectState : 과목의 상태를 저장합니다. (닫힘, 열림, 준비됨)
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
    private const int DONT_HAVE_GROUP = 0;
    private const int NORMAL_ROOT = 0;
    private static string INFO_PATH = Path.Combine(Application.dataPath, "Resources/Subjects/subjectsInfo.json");
    public static List<Subject> subjects = new List<Subject>();
    public static SubjectInfo subjectsInfo;
    public static List<State> subjectState = new List<State>();
    public static int subjectsCount = 0;
    
    public static void initSubjectsAndInfo()
    {
        subjects = new List<Subject>();
        string loadJson = File.ReadAllText(INFO_PATH);
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
        if (subjects[id].subjectGroupId != DONT_HAVE_GROUP) {
            for (int i = 0; i < subjectsCount; i++)
            {
                if (subjects[i].subjectGroupId == subjects[id].subjectGroupId)
                    subjectState[id] = State.Closed;
            }
        }
        foreach (int nextNode in subjects[id].nextSubjects)
        {
            subjectState[nextNode] = State.ReadyToOpen;
        }
    }
    public static void closeSubject(int id)
    {
        subjectState[id] = State.ReadyToOpen;
        if (subjects[id].subjectGroupId != DONT_HAVE_GROUP)
        {
            for (int i = 0; i < subjectsCount; i++)
            {
                if (subjects[i].subjectGroupId == subjects[id].subjectGroupId)
                    subjectState[id] = State.ReadyToOpen;
            }
        }
        foreach (int nextNode in subjects[id].nextSubjects)
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
            rst.Add(subjects[i].needCount);
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
            if (subjects[i].needCount == 0)
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
            List<int> next = subjects[nowNodeId].nextSubjects;
            for (int i = 0; i < next.Count; i++)
            {
                int index = next[i];
                rst[index] = State.ReadyToOpen;
                if (--cntList[index] == 0 && rst[i] == State.Open)
                {
                    searchQ.Enqueue(index);
                    flatSearchList[i] = true;
                }
            }
            if (subjects[nowNodeId].subjectGroupId != DONT_HAVE_GROUP)
            {
                if (!isSameGroup[subjects[nowNodeId].subjectGroupId] && rst[nowNodeId] == State.Open)
                    isSameGroup[subjects[nowNodeId].subjectGroupId] = true;
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
            if (subjects[i].needCount == 0 && flatIdList[i])
            {
                searchQ.Enqueue(i);
                flatSearchList[i] = true;
            }
        }
        while (searchQ.Count > 0)
        {
            int nowNodeId = searchQ.Dequeue();
            List<int> next = subjects[nowNodeId].nextSubjects;
            for (int i = 0; i < next.Count; i++)
            {
                int index = next[i];
                if (--cntList[index] == 0 && flatIdList[index])
                {
                    searchQ.Enqueue(index);
                    flatSearchList[index] = true;
                }
            }
        }

        List<bool> isSameGroup = new List<bool>(subjectsCount);
        foreach (int id in subjectsId)
        {
            if (subjects[id].subjectGroupId != DONT_HAVE_GROUP)
            {
                if (!isSameGroup[subjects[id].subjectGroupId])
                    isSameGroup[subjects[id].subjectGroupId] = true;
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

public class Subject
{
    public const int MAGIC_THEORY = 0, MANA_TELE = 1, HAND_CRAFT = 2, ELEMENT = 3, CHANT_MAGIC = 4;
    public int id;
    public int tier;
    public string name;
    public List<int> enforceContents;
    public List<int> nextSubjects;
    public int subjectGroupId;
    public int root;
    public int needCount;

    public Subject(int Id, int Tier, string Name, List<int> EnforceContents, List<int> NextSubjects, int SubjectGroupId, int Root, int NeedCount)
    {
        id = Id;
        tier = Tier;
        name = Name;
        enforceContents = EnforceContents;
        nextSubjects = NextSubjects;
        subjectGroupId = SubjectGroupId;
        root = Root;
        needCount = NeedCount;
    }
}