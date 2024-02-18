using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class InfoButton : MonoBehaviour
{
    private Collider2D infoButton;
    private Transform pannel;
    private void Awake()
    {
        infoButton = transform.GetChild(0).GetComponent<Collider2D>();
        pannel = transform.GetChild(1).GetComponent<Transform>();
    }

    private void Update()
    {
        Vector2 mp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Ray2D ray2 = new Ray2D(mp, Vector2.zero);
        RaycastHit2D hit = Physics2D.Raycast(ray2.origin, ray2.direction);
        pannel.gameObject.SetActive(hit.collider == infoButton);
    }
}
