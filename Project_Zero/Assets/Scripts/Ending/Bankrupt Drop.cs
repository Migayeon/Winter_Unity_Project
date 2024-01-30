using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BankruptDrop : MonoBehaviour
{
    [SerializeField]
    private int height;
    [SerializeField]
    private float floor;
    [SerializeField]
    private int startTime;
    [SerializeField]
    private float bounceRate;
    [SerializeField]
    private float bounceLimit = 0.01f;
    private bool inFloor = false;
    private float speed = 0;
    private void FixedUpdate()
    {
        if (startTime > 0)
        { 
            startTime--; 
            return;
        }
        if (inFloor)
        {
            return;
        }
        transform.position += new Vector3(0f, speed, 0f);
        if (transform.position.y <= floor / 144)
        {
            speed = - speed * bounceRate;
        }
        if (transform.position.y <= floor / 144 && speed < bounceLimit / 144)
        {
            speed = 0;
            transform.position = new Vector3(transform.position.x, floor / 144, 0f);
            inFloor = true;
        }
        else
            speed -= 0.02f;
    }
}
