using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BankruptDrop : MonoBehaviour
{
    [SerializeField]
    private int height;
    [SerializeField]
    private int startTime;
    private float speed = 0;
    private void FixedUpdate()
    {
        if (startTime > 0)
        { 
            startTime--; 
            return;
        }
        transform.position += new Vector3(0f, speed, 0f);
        if (transform.position.y <= 0)
            speed = - speed * 0.85f;
        if (transform.position.y <= 0 && speed < 0.1f)
        {
            speed = 0f;
            transform.position = new Vector3(transform.position.x, speed, 0f);
        }
        else
            speed -= 0.02f;
    }
}
