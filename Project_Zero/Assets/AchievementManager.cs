using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AchievementManager : MonoBehaviour
{
    [SerializeField]
    private Sprite[] achievementSprites;
    [SerializeField]
    private Transform achievementTransform;
    [SerializeField]
    private RectTransform scroll;

    private Transform scrollContents;
    public AchievementManagementInfo achievementManagementInfo;
    private List<AchievementInfo> achievementInfos = new List<AchievementInfo>();
    private List<bool> isAchievementOpened = new List<bool>();
    
    private readonly string INFO_PATH = Path.Combine(Application.dataPath, "Resources/AchievementsJson/achievementsInfo.json");
    private int achievementCount = 0;

    private void Start()
    {
        initAchievementInfo();
        scrollContents = scroll.GetChild(0).GetChild(0);
        for (int i = 0; i < achievementCount; i++)
            addAchievement(i);
    }

    private void addAchievement(int achievementId)
    {
        Transform newFrame = Instantiate(achievementTransform, new Vector2(-5f + achievementId * 4, 0.5f), Quaternion.identity, scrollContents);
        Transform newLight = newFrame.GetChild(0);
        Transform newPicture = newFrame.GetChild(1);
        if (isAchievementOpened[achievementId])
        {
            newLight.gameObject.SetActive(true);
            newPicture.GetComponent<SpriteRenderer>().sprite = achievementSprites[achievementId];
            newFrame.GetComponent<SpriteRenderer>().color = Color.white;
        }
        else
        {
            newLight.gameObject.SetActive(false);
            newPicture.GetComponent<SpriteRenderer>().sprite = null;
            newFrame.GetComponent<SpriteRenderer>().color = new Color32(80, 112, 173, 255);
        }
    }

    private void initAchievementInfo()
    {
        achievementInfos = new List<AchievementInfo>();
        isAchievementOpened = new List<bool>();
        string loadJson = File.ReadAllText(INFO_PATH);
        achievementManagementInfo = JsonUtility.FromJson<AchievementManagementInfo>(loadJson);
        achievementCount = achievementManagementInfo.count;
        for (int i = 0; i < achievementCount; i++)
        {
            isAchievementOpened.Add(false);
            loadJson = File.ReadAllText(Path.Combine(Application.dataPath, "Resources/AchievementsJson/" + i.ToString() + ".json"));
            achievementInfos.Add(JsonUtility.FromJson<AchievementInfo>(loadJson));
        }
        foreach (int achievementId in achievementManagementInfo.clearedAchievement)
            isAchievementOpened[achievementId] = true;
    }

    public void gotoMenu()
    {
        SceneManager.LoadScene("Title");
    }

    public class AchievementManagementInfo
    {
        public int count;
        public int[] clearedAchievement;
    }

    class AchievementInfo
    {
        int id;
        string name;
        string description;
        bool hidden;
        string code;
    }
}
