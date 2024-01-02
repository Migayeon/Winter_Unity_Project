using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{
    const int N = 2;

    public Button[] tabButton = new Button[N];
    public Canvas[] tabCanvas = new Canvas[N];
    public Button xButton;

    public void OpenTab(int index)
    {
        for(int i = 0; i <  N; i++)
        {
            tabButton[i].image.color = Color.white;
            tabCanvas[i].enabled = false;
        }
        tabButton[index].image.color = Color.cyan;
        tabCanvas[index].enabled = true;
    }

    private void Awake()
    {
        tabButton[0].onClick.AddListener(() => OpenTab(0));
        tabButton[1].onClick.AddListener(() => OpenTab(1));
    }
}
