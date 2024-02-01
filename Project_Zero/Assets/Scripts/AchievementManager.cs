using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static AchievementManager;

public class AchievementManager : MonoBehaviour
{
    [SerializeField]
    private Sprite[] achievementSprites;
    [SerializeField]
    private Transform achievementTransform;
    [SerializeField]
    private RectTransform scroll;
    [SerializeField]
    private Button menuBtn;

    private Transform scrollContents;
    public AchievementManagementInfo achievementManagementInfo;
    private List<AchievementInfo> achievementInfos = new List<AchievementInfo>();
    private List<bool> isAchievementOpened = new List<bool>();
    
    private readonly string INFO_PATH = Path.Combine(Application.dataPath, "Resources/AchievementsJson/achievementsInfo.json");
    private int achievementCount = 0;
    private bool isAchievementDetailOpen = false;

    private void Start()
    {
        initAchievementInfo();
        scrollContents = scroll.GetChild(0).GetChild(0);
        for (int i = 0; i < achievementCount; i++)
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
            int nowTransformId = int.Parse(nowTransform.name);
        }
        else
        {
        }
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

    private KeyValuePair<AchievementManagementInfo, List<AchievementInfo>> initAchievementInfo()
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
        return new KeyValuePair<AchievementManagementInfo, List<AchievementInfo>>(achievementManagementInfo, achievementInfos);
    }

    public void gotoMenu()
    {
        SceneManager.LoadScene("Title");
    }

    public class AchievementManagementInfo
    {
        public int count;
        public int[] clearedAchievement;
        public int[] stats;
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

class AchieveController
{
    private static readonly string INFO_PATH = Path.Combine(Application.dataPath, "Resources/AchievementsJson/achievementsInfo.json");
    private static AchievementManagementInfo loadedInfo;

    private static void LoadJson()
    {
        string loadJson = File.ReadAllText(INFO_PATH);
        loadedInfo = JsonUtility.FromJson<AchievementManagementInfo>(loadJson);
    }

    public static void Achieve(int achieveId)
    {
        LoadJson();
        loadedInfo.clearedAchievement.Append(achieveId);
        string json = JsonUtility.ToJson(loadedInfo, true);
        File.WriteAllText(INFO_PATH, json);
    }

    public static void AddStat(int achieveId, int value)
    {
        LoadJson();
        loadedInfo.stats[achieveId] += value;
        string json = JsonUtility.ToJson(loadedInfo, true);
        File.WriteAllText(INFO_PATH, json);
    }

    public static void SetStat(int achieveId, int value)
    {
        LoadJson();
        loadedInfo.stats[achieveId] = value;
        string json = JsonUtility.ToJson(loadedInfo, true);
        File.WriteAllText(INFO_PATH, json);
    }

    public static int GetStat(int achieveId)
    {
        LoadJson();
        return loadedInfo.stats[achieveId];
    }
}