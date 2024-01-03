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
    public const string fileName = "Assets\\Resources\\Subjects//Subject.csv";
    public class Subject
    {
        private int id;
        private long tier;
        private string subjectName;
        private List<string> enforceStatType;
        private List<int> enforceStatAmount;
        private List<int> nextAvailableID;
        private int needToBeAvailable;  // 해금해야 하는 선행 연구 수
        private bool isAvailable;
        private ProfessorSystem.Professor manager;  // 담당 교수
        private Dictionary<int, string> enforceStatList = new Dictionary<int, string>()
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
    public static List<Subject> ReadSubject()
    {
        List<Subject> returnSubjects = new List<Subject>();
        StreamReader sr = new StreamReader(fileName);
        string line = "";
        string[] row;
        sr.ReadLine();
        while ((line = sr.ReadLine()) != null)
        {
            int id;
            int tier;
            string name;
            List<string> type = new List<string>();
            List<int> amount = new List<int>();
            List<int> nextID = new List<int>();
            int need;

            string[] temp;
            row = line.Split(',');
            id = Convert.ToInt32(row[0]);
            tier = Convert.ToInt32(row[1]);
            name = row[2];
            type = row[3].Split('+').ToList<string>();
            temp = row[4].Split('+');
            for (int i = 0; i < temp.Length; i++) 
            {
                amount.Append(Convert.ToInt32(temp[i]));
            }
            temp = row[5].Split("+");
            for (int i=0; i < temp.Length; i++)
            {
                nextID.Append(Convert.ToInt32(temp[i]));
            }
            need = Convert.ToInt32(row[6]);

            Subject subject = new Subject(id, tier, name, nextID, need);
            subject.SetEnforceAmount(type, amount);
            returnSubjects.Append(subject);
        }
        sr.Close();
        return returnSubjects;
    }
}
