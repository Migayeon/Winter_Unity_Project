using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestBackButton : MonoBehaviour
{
    public void BackMainScene()
    {
        SceneManager.LoadScene("Main");
    }
}
