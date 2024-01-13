using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class InfoButtonManager
{
    public static Dictionary<string, List<string>> buttonInfo;
    private static readonly string INFO_JSON_PATH = Path.Combine(Application.dataPath, "Assets/Resources/InfoContents/infoText.json");
    public class InfoButtonInfo
    {
        public List<string> keys = new List<string>();
        public List<string> values = new List<string>();
    }
    public static void loadInfoFromJson()
    {
        buttonInfo = new Dictionary<string, List<string>>();
        string loadJson = File.ReadAllText(INFO_JSON_PATH);
        InfoButtonInfo tmpLoad = JsonUtility.FromJson<InfoButtonInfo>(loadJson);
        for (int i = 0; i < tmpLoad.keys.Count; i++)
            buttonInfo[tmpLoad.keys[i]] = JsonUtility.FromJson<List<string>>(tmpLoad.values[i]);
    }
    public static string getContent(string sceneName, int index)
    {
        return buttonInfo[sceneName][index];
    }
}
