using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SystemInfoManager
{
    public static Dictionary<string, ProfessorData> basicProfessors;
    public static ProfessorConst basicProfessorsCnt;
    public static Dictionary<string, ProfessorData> standardProfessors;
    public static ProfessorConst standardProfessorsPercent;
    public static void InitSystemInfo()
    {
        string professorPath = Path.Combine(Application.dataPath, "SystemInfo/Professor");


        string professorBasicPath = Path.Combine(professorPath, "Basic");

        string professorStandardPath = Path.Combine(professorPath, "Standard");


        Dictionary<string, string> professorBasicInfoPath = new Dictionary<string, string>() {
            {"Normal", Path.Combine(professorBasicPath, "Normal.json") },
            {"Battle", Path.Combine(professorBasicPath, "Battle.json") },
            {"Unique", Path.Combine(professorBasicPath, "Unique.json") },
            {"Count", Path.Combine(professorBasicPath, "Count.json") }
        };

        Dictionary<string, string> professorStandardInfoPath = new Dictionary<string, string>() {
            {"Normal", Path.Combine(professorStandardPath, "Normal.json") },
            {"Battle", Path.Combine(professorStandardPath, "Battle.json") },
            {"Unique", Path.Combine(professorStandardPath, "Unique.json") },
            {"Percent", Path.Combine(professorStandardPath, "Percent.json") }
        };


        basicProfessors = new Dictionary<string, ProfessorData>() {
            {"Normal", JsonUtility.FromJson<ProfessorData>(File.ReadAllText(professorBasicInfoPath["Normal"])) },
            {"Battle", JsonUtility.FromJson<ProfessorData>(File.ReadAllText(professorBasicInfoPath["Battle"])) },
            {"Unique", JsonUtility.FromJson<ProfessorData>(File.ReadAllText(professorBasicInfoPath["Unique"])) }
        };
        basicProfessorsCnt = JsonUtility.FromJson<ProfessorConst>(File.ReadAllText(professorBasicInfoPath["Count"]));

        standardProfessors = new Dictionary<string, ProfessorData>() {
            {"Normal", JsonUtility.FromJson<ProfessorData>(File.ReadAllText(professorStandardInfoPath["Normal"])) },
            {"Battle", JsonUtility.FromJson<ProfessorData>(File.ReadAllText(professorStandardInfoPath["Battle"])) },
            {"Unique", JsonUtility.FromJson < ProfessorData >(File.ReadAllText(professorStandardInfoPath["Unique"])) }
        };
        standardProfessorsPercent = JsonUtility.FromJson<ProfessorConst>(File.ReadAllText(professorStandardInfoPath["Percent"]));
    }

    public class ProfessorData
    {
        public int StatPointMin;
        public int StatPointMax;
        public int SalarySale;
        public int DepositScale;
        public int[] MinStat;
    }

    public class ProfessorConst
    {
        public int Normal;
        public int Battle;
        public int Unique;
    }
}
