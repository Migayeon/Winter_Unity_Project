using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrintEvent : MonoBehaviour
{
    [SerializeField] Button button;
    void Start()
    {
        button.onClick.AddListener(GetRandomEvent);
    }    
    public void GetRandomEvent()
    {
        (string, int) eventID;
        eventID = EventManager.GetEvent();
        Debug.Log($"Event Type:{eventID.Item1} Event ID:{eventID.Item2}");
    }
}
