using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementInitializer : MonoBehaviour
{
    [SerializeField]
    private List<Sprite> achieveIllust;
    void Awake()
    {
        AchievementManager.InitAchievementInfo();
        AchievementManager.InitIllust(achieveIllust);
    }
}
