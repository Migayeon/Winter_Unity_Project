using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Unity.Burst.Intrinsics.X86.Avx;

public class AchievementManager : MonoBehaviour
{
    public static readonly string ACHIEVE_SAVE_PATH = Path.Combine(Application.dataPath, "AchievementsJson/achievementsSave.json");

    public static AchievementManagementSave achievementManagementInfo;
    public static List<AchievementInfo> achievementInfos = new List<AchievementInfo>();
    public static List<Sprite> illustSprites = new List<Sprite>();

    private static AchievementManagementSave loadedInfo;

    public static int achievementCount = 0;
    public static List<bool> isAchievementOpened = new List<bool>();

    public static Dictionary<int, int> localStat = new Dictionary<int, int>();

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
        public List<int> staticStats;
    }
    
    public class LocalStatSave
    {
        public List<int> Keys;
        public List<int> Values;
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
        AchievementDisplayer.updateAchievement(achieveId);
        File.WriteAllText(ACHIEVE_SAVE_PATH, json);
    }

    public static void AddStaticStat(int achieveId, int value)
    {
        LoadJson();
        AdjustStaticStatList(achieveId);
        loadedInfo.staticStats[achieveId] += value;
        string json = JsonUtility.ToJson(loadedInfo, true);
        File.WriteAllText(ACHIEVE_SAVE_PATH, json);
    }

    public static void SetStaticStat(int achieveId, int value)
    {
        LoadJson();
        AdjustStaticStatList(achieveId);
        loadedInfo.staticStats[achieveId] = value;
        string json = JsonUtility.ToJson(loadedInfo, true);
        File.WriteAllText(ACHIEVE_SAVE_PATH, json);
    }

    public static int GetStaticStat(int achieveId)
    {
        LoadJson();
        AdjustStaticStatList(achieveId);
        return loadedInfo.staticStats[achieveId];
    }

    public static void AdjustStaticStatList(int achieveId)
    {
        while (achieveId > loadedInfo.staticStats.Count - 1)
            loadedInfo.staticStats.Add(0);
    }

    public static string SaveLocalStat()
    {
        LocalStatSave tmp = new LocalStatSave();
        tmp.Keys = localStat.Keys.ToList();
        tmp.Values = localStat.Values.ToList();
        return JsonUtility.ToJson(tmp);
    }

    public static void LoadLocalStat(string json)
    {
        LocalStatSave tmp = JsonUtility.FromJson<LocalStatSave>(json);
        for (int i = 0; i < tmp.Keys.Count; i++)
            localStat[tmp.Keys[i]] = tmp.Values[i];
    }

    public static void CreateLocalStat(int achieveId, int value = 0)
    {
        if (!localStat.Keys.Contains(achieveId))
            localStat[achieveId] = 0;
    }

    public static bool IsAchievementOpened(int achieveId)
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