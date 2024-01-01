using System;
using System.Collections.Generic;

public static class Constants
{
    public const int professorStats = 6;
}

public class Professor
{
    private int id ; //Professor ID (can be any int)
    private string name; //Professor name
    private int tenure; //Professor tenure ( >= 0)
    private int type; //Professor type (1: Unique, 0: Normal)

    //insert Professor stats here
    private List<int> stat;

    private bool away; //true = is away, false = is not away (able to teach)
    public Professor() {}
    public Professor(int _id, string _name, int _tenure, int _type, List<int> _stat)
    {
        id = _id;
        name = _name;
        tenure = _tenure;
        type = _type;
        stat = new List<int>(_stat);
        away = false;
    }
    
    public int ProfessorGetID() { return id; }
    public string ProfessorGetName() { return name; }
    public int ProfessorGetTenure() { return tenure; }
    public int ProfessorGetType() { return type; }
    
    public void ProfessorIncreaseTenure()
    {
        tenure++;
    }

    public void ProfessorSetAwayStatus(bool status)
    {
        away = status;
    }
    public void ConsolePrintProfessorInfo()
    {
        Console.WriteLine($"ID: {id}");
        Console.WriteLine($"Name: {name}");
        Console.WriteLine($"Tenure: {tenure} year(s)");
        if (type == 1)
            Console.WriteLine("Type: Unique");
        else
            Console.WriteLine("Type: Normal");
        Console.Write("Stats: ");
        for (int i = 0; i < Constants.professorStats; ++i)
        {
            Console.Write(stat[i]);
            Console.Write(" ");
        }
    }
}

class ProfessorSystem
{
    public static void Main()
    {
        //code for testing Professor class using Console input/output
        /*
        int _id = Convert.ToInt32(Console.ReadLine());
        string _name = Console.ReadLine();
        int _tenure = Convert.ToInt32(Console.ReadLine());
        int _type = Convert.ToInt32(Console.ReadLine());
        List<int> _stat = new List<int>(Constants.professorStats);
        for (int i = 1; i <= Constants.professorStats; ++i)
        {
            _stat.Add(i);
        }
        Professor testprof = new(_id, _name, _tenure, _type, _stat);
        testprof.ConsolePrintProfessorInfo();
        */
    }
}

