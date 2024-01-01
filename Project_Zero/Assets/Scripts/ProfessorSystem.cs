using System;

public class Professor
{
    private int id ; //Professor ID (can be any int)
    private string name; //Professor name
    private int tenure; //Professor tenure ( >= 0)
    private int type; //Professor type (1: Unique, 0: Normal)

    //insert Professor stats here

    public Professor() {}
    public Professor(int _id, string _name, int _tenure, int _type)
    {
        id = _id;
        name = _name;
        tenure = _tenure;
        type = _type;
    }
    
    public int ProfessorGetID() { return id; }
    public string ProfessorGetName() { return name; }
    public int ProfessorGetTenure() { return tenure; }
    public int ProfessorGetType() { return type; }

    public void ProfessorIncreaseTenure()
    {
        tenure++;
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
        Professor testprof = new(_id, _name, _tenure, _type);
        testprof.ConsolePrintProfessorInfo();
        */
    }
}

