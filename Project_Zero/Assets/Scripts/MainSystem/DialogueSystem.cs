using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueSystem : MonoBehaviour
{
    public static string situation = null;
    public GameObject dialogue;
    
    private void Awake()
    {
        dialogue.SetActive(false);
        if(situation != null)
        {
            dialogue.SetActive(true);
            
        }
    }
}
