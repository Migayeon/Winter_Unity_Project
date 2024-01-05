using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public static int dataIndex = 1;
    public static int cost = 60;
    public static string playerName = "default";
    public static string arcademyName = "default";
    public static int maxStudent = 300;
    public static int maxProfessor = 30;

    public static List<ProfessorSystem.Professor> ProfessorList = new List<ProfessorSystem.Professor>();
    public static List<StudentGroup[]> studentGroups = new List<StudentGroup[]>();
    /*
    public class Player
    {
        public  int dataIndex;
        public  string playerName;
        public  string arcademyName;
    }
    */
}
