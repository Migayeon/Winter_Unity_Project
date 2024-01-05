using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ProfessorSystem : MonoBehaviour
{
    
    public const int professorStats = 6;
    public class Professor
    {
        private long id ; //Professor ID (can be any int)
        private string name; //Professor name
        private int tenure; //Professor tenure ( >= 0)
        private int type; //Professor type (2: unique, 1: battle, 0: normal)
        private List<int> stat = new List<int>(6); //professor stats
        private int salary;
        private bool away; //true = is away, false = is not away (able to teach)
        private List<int> subjects = new List<int>(); //List of subjects that the professor is teaching

        public static Dictionary<int, string> statlist = new Dictionary<int, string>(6)
        {
            {0, "lecture"},
            {1, "theory"},
            {2, "mana"},
            {3, "craft"},
            {4, "element"},
            {5, "attack"},
        };

        public static Dictionary<int, string> ProfessorTypeList = new Dictionary<int, string>(3)
        {
            {0, "일반" },
            {1, "전투" },
            {2, "유니크"},
        };
        public Professor() {}
        public Professor(long _id, string _name, int _tenure, int _type, List<int> _stat)
        {
            id = _id;
            name = _name;
            tenure = _tenure;
            type = _type;
            stat = new List<int>(_stat);
            away = false;
            //insert salary related issue  here
            ProfessorSetDefaultSalary();
        }
        public Professor(long _id, string _name, int _tenure, int _type, List<int> _stat, int _salary)
        {
            id = _id;
            name = _name;
            tenure = _tenure;
            type = _type;
            stat = new List<int>(_stat);
            away = false;
            salary = _salary;
        }
        
        public void ProfessorSetDefaultSalary()
        {
            //change according to salary rules
            int sum = 0;
            for (int i = 0; i < professorStats; ++i)
            {
                sum += stat[i];
            }
            salary = sum;
        }
        public long ProfessorGetID() { return id; }
        public string ProfessorGetName() { return name; }
        public int ProfessorGetTenure() { return tenure; }
        public int ProfessorGetType() { return type; }
        public List<int> ProfessorGetStats() { return stat; }
        public bool ProfessorGetAwayStatus() { return away; }
        public int ProfessorGetSalary() { return salary; }

        public void ProfessorIncreaseTenure()
        {
            tenure++;
        }
        public void ProfessorIncreaseTenure(int increaseValue)
        {
            tenure += increaseValue;
        }

        public void ProfessorSetAwayStatus(bool status)
        {
            away = status;
        }

        public void ProfessorChangeStat(int idx, int changedValue)
        {
            stat[idx] = changedValue;
        }
        public void ProfessorChangeStat(string statname, int changedValue)
        {
            for (int i = 0; i < professorStats; ++i)
            {
                if (statlist[i] == statname)
                {
                    stat[i] = changedValue;
                    break;
                }
            }
        }

        public void ProfessorChangeSalary(double mul)
        {
            salary = (int)((double)salary * mul);
        }

        public string ProfessorGetTypeInString()
        {
            return ProfessorTypeList[type];
        }

        public void ProfessorAddSubject(int subjectID)
        {
            subjects.Add(subjectID);

        }
        public void ProfessorRemoveSubject(int subjectID)
        {
            bool RemoveCheck = subjects.Remove(subjectID);
        }

        public void UnityDebugLogProfessorInfo()
        {
            Debug.Log(string.Format("ID: {0}", id));
            Debug.Log(string.Format("Name: {0}", name));
            Debug.Log(string.Format("Tenure: {0} year(s)", tenure));
            if (type == 1)
            {
                Debug.Log("Type: unique");
            }
            else
            {
                Debug.Log("Type: normal");
            }
            Debug.Log("Stats Information");
            string temp = "Professor Stats : ";
            for (int i = 0; i < professorStats; ++i)
            {
                temp += statlist[i];
                temp += " ";
                temp += Convert.ToString(stat[i]);
                temp += "     ";
            }
            Debug.Log(temp);
            Debug.Log(string.Format("Salary: {0}", salary));
        }
    }
    
    void Start()
    {
        //code for testing Professor class using Console input/output
        /*
        List<int> temp = new List<int>(6) { 1, 2, 3, 4, 5, 6};
        Professor myprofrofessor = new Professor(20231547, "Buru Chang", 1, 1, temp);
        myprofrofessor.UnityDebugLogProfessorInfo();
        */
        
    }
}

/*
unique probability = 5%

*/

