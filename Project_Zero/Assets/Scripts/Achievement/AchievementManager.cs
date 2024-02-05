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
    public static readonly string ACHIEVE_SAVE_PATH = Path.Combine(Application.dataPath, "AchievementsJson/achievementsSave.json");

    public static AchievementManagementSave achievementManagementInfo;
    public static List<AchievementInfo> achievementInfos = new List<AchievementInfo>();
    public static List<Sprite> illustSprites = new List<Sprite>();

    private static AchievementManagementSave loadedInfo;

    public static int achievementCount = 0;
    public static List<bool> isAchievementOpened = new List<bool>();

    public static KeyValuePair<AchievementManagementSave, List<AchievementInfo>> InitAchievementInfo()
    {
        achievementInfos = new List<AchievementInfo>();
        isAchievementOpened = new List<bool>();
        string loadJson = File.ReadAllText(ACHIEVE_SAVE_PATH);
        achievementManagementInfo = JsonUtility.FromJson<AchievementManagementSave>(loadJson);
        int i = 0;
        while (true)
        {
            isAchievementOpened.Add(false);
            string path = Path.Combine(Application.dataPath, "AchievementsJson/" + i.ToString() + ".json");
            if (!File.Exists(path))
            {
                achievementCount = i;
                break;
            }
            loadJson = File.ReadAllText(path);
            achievementInfos.Add(JsonUtility.FromJson<AchievementInfo>(loadJson));
            i++;
        }
        foreach (int achievementId in achievementManagementInfo.clearedAchievement)
            isAchievementOpened[achievementId] = true;
        return new KeyValuePair<AchievementManagementSave, List<AchievementInfo>>(achievementManagementInfo, achievementInfos);
    }

    public static void InitIllust(List<Sprite> initializedSprites)
    {
        foreach (Sprite sprite in initializedSprites)
            illustSprites.Add(sprite);
    }

    public class AchievementManagementSave
    {
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
        string loadJson = File.ReadAllText(ACHIEVE_SAVE_PATH);
        loadedInfo = JsonUtility.FromJson<AchievementManagementSave>(loadJson);
    }

    public static void Achieve(int achieveId)
    {
        if (isAchievementOpened[achieveId]) return;
        LoadJson();
        loadedInfo.clearedAchievement.Add(achieveId);
        string json = JsonUtility.ToJson(loadedInfo, true);
        isAchievementOpened[achieveId] = true;
        AchievementAlertManager.alertAchieve(achieveId);
        File.WriteAllText(ACHIEVE_SAVE_PATH, json);
    }

    public static void AddStat(int achieveId, int value)
    {
        LoadJson();
        AdjustStatList(achieveId);
        loadedInfo.stats[achieveId] += value;
        string json = JsonUtility.ToJson(loadedInfo, true);
        File.WriteAllText(ACHIEVE_SAVE_PATH, json);
    }

    public static void SetStat(int achieveId, int value)
    {
        LoadJson();
        AdjustStatList(achieveId);
        loadedInfo.stats[achieveId] = value;
        string json = JsonUtility.ToJson(loadedInfo, true);
        File.WriteAllText(ACHIEVE_SAVE_PATH, json);
    }

    public static int GetStat(int achieveId)
    {
        LoadJson();
        AdjustStatList(achieveId);
        return loadedInfo.stats[achieveId];
    }

    public static void AdjustStatList(int achieveId)
    {
        while (achieveId > loadedInfo.stats.Count - 1)
            loadedInfo.stats.Add(0);
    }

    public static bool isAchieveOpened(int achieveId)
    {
        return isAchievementOpened[achieveId];
    }

    public static void ResetAchievements()
    {
        LoadJson();
        loadedInfo.clearedAchievement.Clear();
        string json = JsonUtility.ToJson(loadedInfo, true);
        File.WriteAllText(ACHIEVE_SAVE_PATH, json);
    }
}