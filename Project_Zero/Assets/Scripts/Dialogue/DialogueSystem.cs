using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{
    static string dialogueName = "1_1"; // Enter dialogue file name
    string[] dialogue = {""}; // 0th element is character, 1st element is content of dialouge

    StreamReader reader = new StreamReader($"Assets\\Resources\\Dialogue\\{dialogueName}.csv"); // Open .csv file

    public Text character; // Unity assignment 
    public Text message;

    // Dialogue Reading Function
    public void ReadDialogue() 
    {
        if (dialogue[0] == "end") // Dialogue end
        {
            return;
        }
        dialogue = reader.ReadLine().Split(",");
        character.text = dialogue[0];
        message.text = dialogue[1];
    }
}
