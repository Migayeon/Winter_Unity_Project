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
        private long id ; //Professor ID (can be any int)
        private string name; //Professor name
        private int tenure; //Professor tenure ( >= 0), measured in *TURNS*
        private int type; //Professor type (2: unique, 1: battle, 0: normal)
        private List<int> stat = new List<int>(6); //professor stats
        private int salary;
        private bool away; //true = is away, false = is not away (able to teach)
        private int awayTime;
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
                temp += statlist[i];
                temp += " ";
                temp += Convert.ToString(stat[i]);
                temp += "     ";
            }
            Debug.Log(temp);
            Debug.Log(string.Format("Salary: {0}", salary));
        }

        /* 추가한 내용 : 교수 데이터 string 형태로 리턴하는 함수 / string 형태를 읽어 교수를 형성하는 함수 */
        public string ProfessorDataToString()
        {
            string data =
                id.ToString() + '/' +
                name + '/' +
                tenure.ToString() + '/' +
                type.ToString();

            foreach (int i in stat)
                data += '/' + i.ToString();

            data += '/' + salary.ToString() + '/' + (away ? 1 : 0).ToString();

            foreach (int i in subjects)
                data += "/" + i.ToString();

            return data;
        }

        public Professor (string data)
        {
            string[] dataList = data.Split("/");
            id = long.Parse(dataList[0]);
            name = dataList[1];
            tenure = int.Parse(dataList[2]);
            type = int.Parse(dataList[3]);
            stat = new List<int>();
            for (int i = 4; i < 10; i++)
            {
                stat.Add(int.Parse(dataList[i]));
            }
            salary = int.Parse(dataList[10]);
            away = (dataList[11] == "1") ? true : false;
            subjects = new List<int>();
            for (int i = 12; i < dataList.Length; i++)
            {
                subjects.Add(int.Parse(dataList[i]));
            }
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

