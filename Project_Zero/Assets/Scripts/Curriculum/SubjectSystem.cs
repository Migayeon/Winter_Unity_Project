using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class SubjectSystem : MonoBehaviour
{
    public const int subjectStats = 5;
    public const string fileName = "Assets\\Resources\\Subjects\\Subject.csv";
    public class Subject
    {
        public int id;
        public long tier;
        public string subjectName;
        public List<string> enforceStatType;
        public List<int> enforceStatAmount;
        public List<int> nextAvailableID;
        public int needToBeAvailable;  // 해금해야 하는 선행 연구 수
        public bool isAvailable;
        public ProfessorSystem.Professor manager;  // 담당 교수
        public Dictionary<int, string> enforceStatList = new Dictionary<int, string>()
        {
            {0, "theory"},
            {1, "mana"},
            {2, "craft"},
            {3, "element"},
            {4, "attack"},
        };
        public Subject() 
        {

        }
        public Subject(int id, long tier, string subjectName,
            List<int> nextAvailableID, int needToBeAvailable)
        {
            this.id = id;
            this.tier = tier;
            this.subjectName = subjectName;
            this.nextAvailableID = nextAvailableID.ToList();
            this.needToBeAvailable = needToBeAvailable;
            this.isAvailable = false;
            manager = new ProfessorSystem.Professor();
            enforceStatAmount = new List<int>()
            {
                0,0,0,0,0
            };
        }
        public void SetEnforceAmount(List<string> type, List<int> amounts)
        {
            for (int i = 0; i < type.Count; i++) 
            {
                enforceStatAmount[enforceStatList.FirstOrDefault(x => x.Value == type[i]).Key] = amounts[i];
            }
        }
        public void TurnAvailable() { isAvailable = true; }
    }

    /*
    public static List<Subject> ReadSubject()
    {

    }
    */
}
