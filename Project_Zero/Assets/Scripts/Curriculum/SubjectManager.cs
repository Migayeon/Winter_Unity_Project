using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;


public class SubjectManager : MonoBehaviour
{
    [System.Serializable]
    public class SubjectTree
    {
        /* << Attributes >>
         *  - List<Subject> subjects : 과목들을 저장합니다.
         *  - Dictionary<string, int> subjectsInfo : 전체적인 정보를 저장합니다.
         *  - ulong subjectState : 어떤 과목이 현재 열려있는 지 저장합니다.
         *  
         *  << Funtions >>
         *  - init() : 
         * 
         */
        public List<Subject> subjects = new List<Subject>();
        public Dictionary<string, int> subjectsInfo;
        public List<bool> subjectState = new List<bool>();
        public int subjectsCount = 0;
        public void init()
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

        public bool isSubjectOpen(int id)
        {
            return subjectState[id];
        }
        public void openSubject(int id)
        {
            subjectState[id] = true;
        }
        public void closeSubject(int id)
        {
            subjectState[id] = false;
        }
        public void changeSubjectState(int id)
        {
            subjectState[id] = !subjectState[id];
        }

        public List<int> getEnforceType(int id)
        {
            return subjects[id].enforceType;
        }

        private List<int> newCntList()
        {
            List<int> rst = new List<int>();
            for (int i = 0; i < subjectsCount; i++)
                rst.Add(subjects[i].needToBeAvailable);
            return rst;
        }
        private List<bool> flattenList(List<int> idList)
        {
            List<bool> rst = new List<bool>();
            for (int i = 0; i < subjectsCount; i++)
                rst.Add(false);
            for (int i = 0; i < idList.Count; i++)
                rst[idList[i]] = true;
            return rst;
        }

        public bool isVaildCurriculum(List<int> subjectsId)
        {
            List<int> cntList = newCntList();
            List<bool> flatList = flattenList(subjectsId);
            Queue<int> searchQ = new Queue<int>();
            Queue<int> resultQ = new Queue<int>();
            for (int i = 0; i < subjectsCount; i++)
            {
                if (subjects[i].needToBeAvailable == 0)
                    searchQ.Enqueue(i);
            }
            while (searchQ.Count > 0)
            {
                for (int i = 0; i < subjectsCount; i++)
                {
                    if (cntList[i] == 0) continue;
                    flatList[i] = false;
                    cntList[i]--;
                }
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
        public int needToBeAvailable;

        public Subject(int id, int tier, string subjectName, List<int> enforceType, List<int> enforceAmount, List<int> nextAvailableId, int relativeSubjectGroupId, int needToBeAvailable)
        {
            this.id = id;
            this.tier = tier;
            this.subjectName = subjectName;
            this.enforceType = enforceType;
            this.enforceAmount = enforceAmount;
            this.nextAvailableId = nextAvailableId;
            this.relativeSubjectGroupId = relativeSubjectGroupId;
            this.needToBeAvailable = needToBeAvailable;
        }
    }
}
