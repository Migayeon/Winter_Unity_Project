using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class AchievementDisplayer : MonoBehaviour
{
    [SerializeField]
    private Transform achievementTransform;
    [SerializeField]
    private RectTransform scroll;
    [SerializeField]
    private Button menuBtn;
    [SerializeField]
    private RectTransform simpleDescription;

    private Transform scrollContents;
    private bool isAchievementDetailOpen = false;
    private static List<Transform> frames = new List<Transform>();

    private void Start()
    {
        frames.Clear();
        scrollContents = scroll.GetChild(0).GetChild(0);
        for (int i = 0; i < AchievementManager.achievementCount; i++)
            addAchievement(i);
        menuBtn.onClick.RemoveAllListeners();
        menuBtn.onClick.AddListener(gotoMenu);
    }

    private void Update()
    {
        if (isAchievementDetailOpen) return;
        Vector2 mp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Ray2D ray2 = new Ray2D(mp, Vector2.zero);
        RaycastHit2D hit = Physics2D.Raycast(ray2.origin, ray2.direction);
        if (hit.collider != null)
        {
            Transform nowTransform = hit.collider.transform;
            int achievementId = Convert.ToInt32(nowTransform.name);
            simpleDescription.gameObject.SetActive(true);
            TMP_Text achievementNameText = simpleDescription.GetChild(0).GetComponent<TMP_Text>();
            TMP_Text achievementContentsText = simpleDescription.GetChild(1).GetComponent<TMP_Text>();
            bool haveTohideInfo = AchievementManager.achievementInfos[achievementId].hidden && !AchievementManager.IsAchievementOpened(achievementId);
            achievementNameText.text = haveTohideInfo ? "???" : AchievementManager.achievementInfos[achievementId].name;
            achievementContentsText.text = haveTohideInfo ? "???" : AchievementManager.achievementInfos[achievementId].description;
            simpleDescription.position = new Vector3(hit.point.x,hit.point.y, -1);
        }
        else
        {
            simpleDescription.gameObject.SetActive(false);
        }
    }

    private void addAchievement(int achievementId)
    {
        Transform newFrame = Instantiate(achievementTransform, new Vector2(-5f + achievementId * 4, 0.5f), Quaternion.identity, scrollContents);
        frames.Add(newFrame);
        updateAchievement(achievementId);
        newFrame.name = achievementId.ToString();
    }

    public static void updateAchievement(int achievementId)
    {
        if (frames.Count <= achievementId) return;
        Transform nowFrame = frames[achievementId];
        Transform nowLight = nowFrame.GetChild(0);
        Transform nowPicture = nowFrame.GetChild(1);
        if (AchievementManager.isAchievementOpened[achievementId])
        {
            nowLight.gameObject.SetActive(true);
            nowPicture.GetComponent<SpriteRenderer>().sprite = AchievementManager.illustSprites[achievementId];
            nowFrame.GetComponent<SpriteRenderer>().color = Color.white;
        }
        else
        {
            nowLight.gameObject.SetActive(false);
            nowPicture.GetComponent<SpriteRenderer>().sprite = null;
            nowFrame.GetComponent<SpriteRenderer>().color = new Color32(80, 112, 173, 255);
        }
    }

    public void gotoMenu()
    {
        SceneManager.LoadScene("Title");
    }
}