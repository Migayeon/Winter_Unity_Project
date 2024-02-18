using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditTextUP : MonoBehaviour
{
    float speed = 0.01f;
    void FixedUpdate()
    {
        transform.position = transform.position + new Vector3(0f, speed, 0f);
        if (transform.localPosition.y >= 3805)
        {
            SceneManager.LoadScene("Title");
        }
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            speed = 0.3f;
        }
        if (Input.GetMouseButtonUp(0))
        {
            speed = 0.01f;
        }
    }
}
