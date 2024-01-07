using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TextUP : MonoBehaviour
{
    float speed = 0.01f;
    void FixedUpdate()
    {
        transform.position = transform.position + new Vector3(0f, speed, 0f);
        if (Input.GetMouseButtonDown(0))
        {
            speed = 0.3f;
        }
        if(Input.GetMouseButtonUp(0))
        {
            speed = 0.03f;
        }
        if(transform.localPosition.y >= 3170)
        {
            SceneManager.LoadScene("StartSetting");
        }
    }
}
