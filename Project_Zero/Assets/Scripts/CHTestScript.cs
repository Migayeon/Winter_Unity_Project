using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CHTestScript : MonoBehaviour
{
    public Button button;
    private void Start()
    {
        button.onClick.AddListener(CH);
    }
    public void CH()
    {
        SceneManager.LoadScene("Title");
    }
}
