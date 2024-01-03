using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextUP : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position + new Vector3(0f,0.02f,0f);
    }
}
