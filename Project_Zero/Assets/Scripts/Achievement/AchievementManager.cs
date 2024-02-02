using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AchievementManager : MonoBehaviour
{
    public static readonly string ACHIEVE_INFO_PATH = Path.Combine(Application.dataPath, "Resources/AchievementsJson/achievementsInfo.json");

    public static AchievementManagementInfo achievementManagementInfo;
    public static List<AchievementInfo> achievementInfos = new List<AchievementInfo>();
    public static List<Sprite> illustSprites = new List<Sprite>();

    private static AchievementManagementInfo loadedInfo;

    public static int achievementCount = 0;
    public static List<bool> isAchievementOpened = new List<bool>();

    public static KeyValuePair<AchievementManagementInfo, List<AchievementInfo>> InitAchievementInfo()
    {
        achievementInfos = new List<AchievementInfo>();
        isAchievementOpened = new List<bool>();
        string loadJson = File.ReadAllText(ACHIEVE_INFO_PATH);
        achievementManagementInfo = JsonUtility.FromJson<AchievementManagementInfo>(loadJson);
        achievementCount = achievementManagementInfo.count;
        for (int i = 0; i < achievementCount; i++)
        {
            isAchievementOpened.Add(false);
            string path = Path.Combine(Application.dataPath, "Resources/AchievementsJson/" + i.ToString() + ".json");
            loadJson = File.ReadAllText(path);
            achievementInfos.Add(JsonUtility.FromJson<AchievementInfo>(loadJson));
        }
        foreach (int achievementId in achievementManagementInfo.clearedAchievement)
            isAchievementOpened[achievementId] = true;
        return new KeyValuePair<AchievementManagementInfo, List<AchievementInfo>>(achievementManagementInfo, achievementInfos);
    }

    public static void InitIllust(List<Sprite> initializedSprites)
    {
        foreach(Sprite sprite in initializedSprites)
            illustSprites.Add(sprite);
    }

    public class AchievementManagementInfo
    {
        public int count;
        public List<int> clearedAchievement;
        public List<int> stats;
    }

    public class AchievementInfo
    {
        public int id;
        public string name;
        public string description;
        public bool hidden;
        public string code;
    }

    private static void LoadJson()
    {
        string loadJson = File.ReadAllText(ACHIEVE_INFO_PATH);
        loadedInfo = JsonUtility.FromJson<AchievementManagementInfo>(loadJson);
    }

    public static void Achieve(int achieveId)
    {
        if (isAchievementOpened[achieveId]) return;
        LoadJson();
        loadedInfo.clearedAchievement.Add(achieveId);
        string json = JsonUtility.ToJson(loadedInfo, true);
        isAchievementOpened[achieveId] = true;
        AchievementAlertManager.alertAchieve(achieveId);
        File.WriteAllText(ACHIEVE_INFO_PATH, json);
    }

    public static void AddStat(int achieveId, int value)
    {
        LoadJson();
        loadedInfo.stats[achieveId] += value;
        string json = JsonUtility.ToJson(loadedInfo, true);
        File.WriteAllText(ACHIEVE_INFO_PATH, json);
    }

    public static void SetStat(int achieveId, int value)
    {
        LoadJson();
        loadedInfo.stats[achieveId] = value;
        string json = JsonUtility.ToJson(loadedInfo, true);
        File.WriteAllText(ACHIEVE_INFO_PATH, json);
    }

    public static int GetStat(int achieveId)
    {
        LoadJson();
        return loadedInfo.stats[achieveId];
    }

    public static bool isAchieveOpened(int achieveId, bool recentlyUpdated = false)
    {
        if (recentlyUpdated)
            LoadJson();
        return isAchievementOpened[achieveId];
    }
}