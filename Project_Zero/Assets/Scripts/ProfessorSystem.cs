using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Xml.Serialization;

public class ProfessorSystem : MonoBehaviour
{
    
    public const int professorStats = 6;
    public class Professor
    {
        [SerializeField]
        private long id ; //Professor ID (can be any int)

        [SerializeField]
        private string name; //Professor name

        [SerializeField]
        private int tenure; //Professor tenure ( >= 0), measured in *TURNS*

        [SerializeField]
        private int type; //Professor type (2: unique, 1: battle, 0: normal)

        [SerializeField]
        private List<int> stat = new List<int>(6); //professor stats

        [SerializeField]
        private int salary;

        [SerializeField]
        private bool away; //true = is away, false = is not away (able to teach)

        [SerializeField]
        private int awayTime;

        [SerializeField]
        private List<int> subjects = new List<int>(); //List of subjects that the professor is teaching

        public List<string> statList = new List<string>(6)
        {
            "lecture",
            "theory",
            "mana",
            "craft",
            "element",
            "attack"
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
            awayTime = 0;
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
            awayTime = 0;
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
        public int ProfessorGetTenureInTurns() { return tenure; }
        public int ProfessorGetTenureInYears() { return tenure / 4; }
        public int ProfessorGetType() { return type; }
        public List<int> ProfessorGetStats() { return stat; }
        public bool ProfessorGetAwayStatus() { return away; }
        public int ProfessorGetSalary() { return salary; }

        public int ProfessorGetAwayTime() { return awayTime; }
        public List<int> ProfessorGetSubjects() { return subjects; }
        public void ProfessorIncreaseTenure()
        {
            tenure++;
        }
        public void ProfessorIncreaseTenure(int increaseValue)
        {
            tenure += increaseValue;
        }

        public void ProfessorSetAwayStatus(bool status, int time)
        {
            away = status;
            awayTime = time;
        }
        
        public void ProfessorChangeAwayStatus(bool status)
        {
            away = status;
        }
        public void ProfessorChangeAwayTime()
        {
            awayTime--;
        }
        public void ProfessorChangeAwayTime(int val)
        {
            awayTime -= val;
        }
        public void ProfessorChangeStat(int idx, int changedValue)
        {
            stat[idx] = changedValue;
        }
        public void ProfessorChangeStat(string statname, int changedValue)
        {
            for (int i = 0; i < professorStats; ++i)
            {
                if (statList[i] == statname)
                {
                    stat[i] = changedValue;
                    break;
                }
            }
        }

        public void ProfessorChangeSalary(double mul)
        {
            salary = (int)(salary * mul);
        }

        public string ProfessorGetTypeInString()
        {
            List<string> ProfessorTypeList = new List<string>(3)
            {
                "일반",
                "전투",
                "유니크"
            };
            return ProfessorTypeList[type];
        }

        public void ProfessorAddSubject(int subjectID)
        {
            subjects.Add(subjectID);

        }
        public void ProfessorRemoveSubject(int subjectID)
        {
            subjects.Remove(subjectID);
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
                temp += statList[i];
                temp += " ";
                temp += Convert.ToString(stat[i]);
                temp += "     ";
            }
            Debug.Log(temp);
            Debug.Log(string.Format("Salary: {0}", salary));
        }

        public void UpgradeRandomStat(int upgradePoint)
        {
            System.Random rand = new System.Random();
            for(int i = 0; i < upgradePoint; i++)
                stat[rand.Next(professorStats - 1)]++;
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

