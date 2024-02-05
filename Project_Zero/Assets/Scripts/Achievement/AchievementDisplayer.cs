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

    private void Start()
    {
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
            bool haveTohideInfo = AchievementManager.achievementInfos[achievementId].hidden && !AchievementManager.isAchieveOpened(achievementId);
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
        Transform newLight = newFrame.GetChild(0);
        Transform newPicture = newFrame.GetChild(1);
        if (AchievementManager.isAchievementOpened[achievementId])
        {
            newLight.gameObject.SetActive(true);
            newPicture.GetComponent<SpriteRenderer>().sprite = AchievementManager.illustSprites[achievementId];
            newFrame.GetComponent<SpriteRenderer>().color = Color.white;
        }
        else
        {
            newLight.gameObject.SetActive(false);
            newPicture.GetComponent<SpriteRenderer>().sprite = null;
            newFrame.GetComponent<SpriteRenderer>().color = new Color32(80, 112, 173, 255);
        }
        newFrame.name = achievementId.ToString();
    }

    public void gotoMenu()
    {
        SceneManager.LoadScene("Title");
    }
}