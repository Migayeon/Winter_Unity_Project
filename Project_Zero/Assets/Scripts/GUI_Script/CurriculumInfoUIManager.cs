using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CurriculumInfoUIManager : MonoBehaviour
{
    void Start()
    {

    }
    void Update()
    {
        Vector2 mp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Ray2D ray2 = new Ray2D(mp, Vector2.zero);
        RaycastHit2D hit = Physics2D.Raycast(ray2.origin, ray2.direction);
        GameObject testObject;
        try
        {
            testObject = hit.collider.gameObject;
            Debug.Log(testObject.name);
        }
        catch { }
    }
}
