using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementAlertManager : MonoBehaviour
{
    [SerializeField]
    private Canvas alertCanvas;
    [SerializeField]
    private Transform alertPanel;
    private Vector2 startPos;
    public static Queue<int> achieveAlertQueue = new Queue<int>();
    private bool isDisplaying = false;
    private int nowAchieveId = -1;
    private int panelProgress = 0;
    private float targetY;


    public void Awake()
    {
        DontDestroyOnLoad(alertCanvas);
    }

    public static void alertAchieve(int id)
    {
        achieveAlertQueue.Enqueue(id);
    }

    public void Update()
    {
        if ((!isDisplaying) && (achieveAlertQueue.Count > 0))
        {
            isDisplaying = true;
            nowAchieveId = achieveAlertQueue.Dequeue();
            Image illust = alertPanel.GetChild(0).GetChild(0).GetComponent<Image>();
            TMP_Text text = alertPanel.GetChild(1).GetComponent<TMP_Text>();
            illust.sprite = AchievementManager.illustSprites[nowAchieveId];
            text.text = "업적 달성 : '" + AchievementManager.achievementInfos[nowAchieveId].name + "'";
            panelProgress = 0;
            startPos = alertPanel.position;
            targetY = startPos.y - 200;//alertPanel.localScale.y * 3 / 2;
            Debug.Log(startPos.y);
            Debug.Log(targetY);
        }
    }

    public void FixedUpdate()
    {
        if (isDisplaying)
        {
            if (panelProgress <= 30)
                alertPanel.position = new Vector3(startPos.x, startPos.y - 6.666f * panelProgress, 0);//alertPanel.localScale.y / 20 * panelProgress, 0);
            else if (panelProgress == 90)
            {
                alertPanel.position = startPos;
                isDisplaying = false;
                panelProgress = 0;
            }
            else if (panelProgress >= 60)
                alertPanel.position = new Vector3(startPos.x, targetY + 6.666f * (panelProgress - 60), 0);//alertPanel.localScale.y / 20 * (panelProgress - 60), 0);
            panelProgress++;
        }
    }
}
