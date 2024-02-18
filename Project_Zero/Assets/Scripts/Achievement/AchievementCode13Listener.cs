using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementCode13Listener : MonoBehaviour
{
    private const float interval = 1.0f;
    private float clickTime = -1.0f;
    private int step = 0;
    private readonly KeyCode[] arrows = {
        KeyCode.RightArrow,
        KeyCode.UpArrow,
        KeyCode.LeftArrow,
        KeyCode.DownArrow
    };
    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    void Update()
    {
        if (AchievementManager.isAchievementOpened[13]) return;
        if (Input.GetKey(arrows[step]))
        {
            if (Time.time - clickTime < interval)
            {
                if (++step == 4)
                {
                    AchievementManager.Achieve(13);
                    step = 0;
                }
                clickTime = Time.time;
            }
        }
        if (Time.time - clickTime >= interval)
        {
            step = 0;
            clickTime = Time.time;
        }
    }
}
