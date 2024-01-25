using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;
public static class SubjectTree
{
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
    public static List<List<int>> subjectGroups = new List<List<int>>();

    public static SubjectInfo subjectsInfo;
    public static List<State> subjectState = new List<State>();
    public static List<int> subjectStateNeedCnt = new List<int>();

    public static List<int> studentInSubjectCnt = new List<int>();
    public static List<int> professorInSubjectCnt = new List<int>();
    public static Dictionary<long, List<bool>> professorsLecture = new Dictionary<long, List<bool>>();
    public static int subjectsCount = 0;

    public static void initSubjectsAndInfo()
    {
        subjects = new List<Subject>();
        subjectStateNeedCnt = new List<int>();
        subjectGroups = new List<List<int>>();
        string loadJson = File.ReadAllText(INFO_PATH);
        subjectsInfo = JsonUtility.FromJson<SubjectInfo>(loadJson);
        subjectsCount = subjectsInfo.count;
        for (int i = 0; i < subjectsInfo.groupCount + 1; i++)
            subjectGroups.Add(new List<int>());
        for (int subjectId = 0; subjectId < subjectsCount; subjectId++)
        {
            string subjectPath = Path.Combine(Application.dataPath, "Resources/Subjects/" + subjectId.ToString() + ".json");
            loadJson = File.ReadAllText(subjectPath);
            subjects.Add(JsonUtility.FromJson<Subject>(loadJson));
            subjectStateNeedCnt.Add(subjects[subjectId].needCount);
            if (subjects[subjectId].subjectGroupId != DONT_HAVE_GROUP)
            {
                subjectGroups[subjects[subjectId].subjectGroupId].Add(subjectId);
            }
        }
        for (int subjectId = 0; subjectId < subjectsCount; subjectId++)
        {
            foreach (int targetId in subjects[subjectId].nextSubjects)
            {
                if (!subjects[targetId].mustFulfillSubjects.Contains(subjectId))
                    subjects[targetId].mustFulfillSubjects.Add(subjectId);
            }
        }
        /*
        foreach (Subject subject in subjects)
        {
            for (int i = 0; i < subject.mustFulfillSubjects.Count; i++)
            {
                Debug.Log(subject.id.ToString() + " : " + subject.mustFulfillSubjects[i].ToString());
            }
        }
        */
    }

    public static void callOnlyOneTimeWhenGameStart()
    {
        studentInSubjectCnt = new List<int>();
        professorInSubjectCnt = new List<int>();
        for (int i = 0; i < subjectsCount; i++)
        {
            studentInSubjectCnt.Add(0);
            professorInSubjectCnt.Add(0);
        }
    }

    public static bool isSubjectOpen(int id)
    {
        return subjectState[id] == State.Open;
    }
    public static void openSubject(int id)
    {
        if (subjects[id].subjectGroupId != DONT_HAVE_GROUP)
        {
            for (int i = 0; i < subjectsCount; i++)
            {
                if (subjects[i].subjectGroupId == subjects[id].subjectGroupId)
                    subjectState[i] = State.Closed;
            }
        }
        subjectState[id] = State.Open;
        foreach (int nextNode in subjects[id].nextSubjects)
        {
            Debug.Log(subjectStateNeedCnt.Count);
            if (--subjectStateNeedCnt[nextNode] == 0)
                subjectState[nextNode] = State.ReadyToOpen;
        }
    }
    public static void closeSubject(int id)
    {
        if (subjects[id].subjectGroupId != DONT_HAVE_GROUP)
        {
            for (int i = 0; i < subjectsCount; i++)
            {
                if (subjects[i].subjectGroupId == subjects[id].subjectGroupId)
                    subjectState[i] = State.ReadyToOpen;
            }
        }
        subjectState[id] = State.ReadyToOpen;
        foreach (int nextNode in subjects[id].nextSubjects)
        {
            subjectStateNeedCnt[nextNode]++;
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
    public static void initSubjectStates(List<int> openedSubjectsId = null)
    {
        subjectState = new List<State>();
        subjectStateNeedCnt = new List<int>();
        for (int i = 0; i < subjectsCount; i++)
        {
            subjectState.Add(State.Closed);
            subjectStateNeedCnt.Add(subjects[i].needCount);
        }
        for (int i = 0; i < openedSubjectsId.Count; i++)
            subjectState[openedSubjectsId[i]] = State.Open;
        for (int i = 0; i < subjectsCount; i++)
            subjectState.Add(State.Closed);
        List<int> cntList = newCntList();
        List<bool> flatSearchList = flattenList(new List<int>());
        List<bool> isSameGroup = new List<bool>();
        for (int i = 0; i < subjectsInfo.groupCount; i++)
            isSameGroup.Add(false);
        Queue<int> searchQ = new Queue<int>();
        for (int i = 0; i < subjectsCount; i++)
        {
            if (subjects[i].needCount == 0)
            {
                if (subjectState[i] == State.Open)
                {
                    searchQ.Enqueue(i);
                    flatSearchList[i] = true;
                }
                else
                {
                    subjectState[i] = State.ReadyToOpen;
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
                subjectState[index] = State.ReadyToOpen;
                if (--cntList[index] == 0 && subjectState[i] == State.Open)
                {
                    searchQ.Enqueue(index);
                    flatSearchList[i] = true;
                }
            }
            if (subjects[nowNodeId].subjectGroupId != DONT_HAVE_GROUP)
            {
                if (!isSameGroup[subjects[nowNodeId].subjectGroupId] && subjectState[nowNodeId] == State.Open)
                    isSameGroup[subjects[nowNodeId].subjectGroupId] = true;
                else
                    subjectState[nowNodeId] = State.Closed;
            }
        }
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

        List<bool> isSameGroup = new List<bool>();
        for (int i = 0; i < subjectsCount; i++)
            isSameGroup.Add(false);
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

    public static void addSubjectStudentCnt(List<int> curriculum)
    {
        foreach(int subjectId in curriculum)
            studentInSubjectCnt[subjectId]++;
    }

    public static void removeSubjectStudentCnt(List<int> curriculum)
    {
        foreach (int subjectId in curriculum)
            studentInSubjectCnt[subjectId]--;
    }

    public static void addProfessorAt(long professorId, int subjectId)
    {
        if (professorsLecture.ContainsKey(professorId))
        {
            professorInSubjectCnt[subjectId]++;
            professorsLecture[professorId][subjectId] = true;
        }
        else
        {
            professorsLecture[professorId] = new List<bool>();
            for (int i = 0; i < subjectsCount; i++)
                professorsLecture[professorId].Add(i == subjectId);
        }
    }
    public static void removeProfessor(long professorId)
    {
        professorsLecture.Remove(professorId);
        for (int subjectId = 0; subjectId < subjectsCount; subjectId++)
        {
            if (professorsLecture[professorId][subjectId])
                professorInSubjectCnt[subjectId]--;
        }
    }
    public static void removeProfessorAt(long professorId, int subjectId)
    {
        if (professorsLecture.ContainsKey(professorId))
        {
            professorInSubjectCnt[subjectId]--;
            professorsLecture[professorId][subjectId] = false;
            if (!professorsLecture[professorId].Contains(true))
                professorsLecture.Remove(professorId);
        }
    }
    public static bool canRemoveProfessor(long professorId, List<int> curriculum)
    {
        for (int subjectId = 0; subjectId < subjectsCount; subjectId++)
        {
            if (!curriculum.Contains(subjectId)) continue;
            if (professorsLecture.ContainsKey(professorId))
            {
                if (professorsLecture[professorId][subjectId] && professorInSubjectCnt[subjectId] == 1)
                    return false;
            }
        }
        return true;
    }

    public static bool canFreeProfessorInSubject(long professorId, int querySubjectId)
    {
        if (professorsLecture.ContainsKey(professorId))
        {
            if (studentInSubjectCnt[querySubjectId] > 0 && professorsLecture[professorId][querySubjectId] && professorInSubjectCnt[querySubjectId] == 1)
                return false;
        }
        return true;
    }

    public static List<long> getProfesorListBySubjectId(int subjectId)
    {
        List<long> rst = new List<long>();
        foreach (long professorId in professorsLecture.Keys)
        {
            if (professorsLecture[professorId][subjectId])
                rst.Add(professorId);
        }
        return rst;
    }

    public static List<int> getCurriculumFor(int queryId)
    {
        List<int> rst = new List<int>();
        List<bool> groupChecked = new List<bool>();
        List<bool> visited = new List<bool>();
        for (int i = 0; i < subjectsInfo.groupCount + 1; i++)
            groupChecked.Add(false);
        for (int i = 0; i < subjectsCount; i++)
            visited.Add(false);
        postOrder(queryId, ref rst, ref groupChecked, ref visited, queryId);
        return rst;
    }

    public static void postOrder(int queryId, ref List<int> result, ref List<bool> groupChecked, ref List<bool> visited, int origin)
    {
        if (visited[queryId]) return;
        List<int> nextNodes = subjects[queryId].mustFulfillSubjects;
        if (subjects[queryId].subjectGroupId == DONT_HAVE_GROUP)
        {
            foreach (int subjectId in nextNodes)
                postOrder(subjectId, ref result, ref groupChecked, ref visited, origin);
            result.Add(queryId);
            visited[queryId] = true;
        }
        else
        {
            int groupId = subjects[queryId].subjectGroupId;
            if (!groupChecked[groupId])
            {
                foreach (int subjectId in nextNodes)
                    postOrder(subjectId, ref result, ref groupChecked, ref visited, origin);
                groupChecked[groupId] = true;
                visited[queryId] = true;
                if (queryId != origin) {
                    List<int> nowGroup = subjectGroups[groupId];
                    List<int> tmp = new List<int>();
                    for (int i = 0; i < nowGroup.Count; i++)
                    {
                        if (subjectState[nowGroup[i]] == State.Open)
                            tmp.Add(nowGroup[i]);
                    }
                    result.Add(tmp[new System.Random().Next(0, tmp.Count)]);
                }
                else
                    result.Add(origin);
            }
        }
    }

    public class SaveData
    {
        public int professorCnt = 0;
        public List<long> professorsId = new List<long>();
        public List<string> lecturesId = new List<string>();
        public List<int> subjectStates = new List<int>();
    }

    public static string save()
    {
        SaveData rst = new SaveData();
        rst.professorCnt = professorsLecture.Count;
        rst.professorsId = professorsLecture.Keys.ToArray().ToList();
        for (int i = 0; i < professorsLecture.Keys.Count; i++)
        {
            List<bool> teachingState = professorsLecture[professorsLecture.Keys.ToList<long>()[i]];
            rst.lecturesId.Add("");
            for (int j = 0; j < subjectsCount; j += 3)
                rst.lecturesId[i] += (
                    Convert.ToInt32(teachingState[j + 2]) +
                    Convert.ToInt32(teachingState[j + 1]) * 2 +
                    Convert.ToInt32(teachingState[j]) * 4
                    ).ToString();
        }
        Dictionary<State, bool> convertToInt = new Dictionary<State, bool>(3)
        {
            {State.Closed, false},
            {State.Open, true},
            {State.ReadyToOpen, false}
        };
        List<bool> filter = subjectState.Select(s => convertToInt[s]).ToList<bool>();
        for (int i = 0; i < subjectsCount; i++)
        {
            if (filter[i])
                rst.subjectStates.Add(i);
        }
        return JsonUtility.ToJson(rst,true);
    }

    public static void load(string jsonContents)
    {
        initSubjectsAndInfo();
        callOnlyOneTimeWhenGameStart();
        SaveData data = JsonUtility.FromJson<SaveData>(jsonContents);
        for (int i = 0; i < data.professorCnt; i++)
        {
            List<bool> lectureState = new List<bool>();
            for (int j = 0; j < Math.Ceiling((double) subjectsCount / 3); j++)
            {
                int tmp = Convert.ToInt32(data.lecturesId[i][j]);
                lectureState.Add((tmp & 4) == 1);
                if ((tmp & 1) == 1) professorInSubjectCnt[j * 3] ++;
                if (lectureState.Count == subjectsCount) break;
                lectureState.Add((tmp & 2) == 1);
                if ((tmp & 2) == 1) professorInSubjectCnt[j * 3 + 1]++;
                if (lectureState.Count == subjectsCount) break;
                lectureState.Add((tmp & 1) == 1);
                if ((tmp & 4) == 1) professorInSubjectCnt[j * 3 + 2]++;
                if (lectureState.Count == subjectsCount) break;
            }
            professorsLecture[data.professorsId[i]] = lectureState;
        }
        initSubjectStates(data.subjectStates);
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
    public const int MAGIC_THEORY = 0, MANA_TELE = 1, HAND_CRAFT = 2, ELEMENT = 3, CHANT_MAGIC = 4;
    public int id;
    public int tier;
    public string name;
    public List<int> enforceContents;
    public List<int> nextSubjects;
    public List<int> mustFulfillSubjects;
    public int subjectGroupId;
    public int root;
    public int needCount;

    public Subject(int Id, int Tier, string Name, List<int> EnforceContents, List<int> NextSubjects, int SubjectGroupId, int Root, int NeedCount, List<int> mustFulfillSubjects)
    {
        id = Id;
        tier = Tier;
        name = Name;
        enforceContents = EnforceContents;
        nextSubjects = NextSubjects;
        subjectGroupId = SubjectGroupId;
        root = Root;
        needCount = NeedCount;
        this.mustFulfillSubjects = mustFulfillSubjects;
    }
}
