using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fadein : MonoBehaviour
{
    private Color buttonColor;
    private Image buttonImg;
    [SerializeField]
    private int timing;
    private float fadeIn = 0f;
    private void Start()
    {
        buttonImg = GetComponent<Image>();
        buttonColor = buttonImg.color;
    }

    private void Update()
    {
        if (timing > 0)
        {
            timing--;
            return;
        }
        buttonColor.a = fadeIn;
        buttonImg.color = buttonColor;
        fadeIn += 0.01f;
    }
}
