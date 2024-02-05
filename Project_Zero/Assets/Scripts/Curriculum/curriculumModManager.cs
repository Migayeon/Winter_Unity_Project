using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class curriculumModManager : MonoBehaviour
{
    [SerializeField]
    private Transform subjectGetterTransform;
    [SerializeField]
    private List<Transform> canvasList;
    [SerializeField]
    private GameObject ESC_Canvas;
    [SerializeField]
    private Button migayeonBtn;

    private static int nowMod = 0;

    void changeMod(int mod)
    {
        if (mod == 0)
            ESC_Canvas.SetActive(false);
        else
            ESC_Canvas.SetActive(true);
        for (int i = 0; i < canvasList.Count; i++)
            canvasList[i].gameObject.SetActive(mod == i);
    }

    private void Start()
    {
        migayeonBtn.onClick.RemoveAllListeners();
        migayeonBtn.onClick.AddListener(
            delegate
            {
                AchievementManager.Achieve(4);
            }
        );
        changeMod(nowMod);
    }

    public static void loadCurriculumSceneWithMod(int mod)
    {
        nowMod = mod;
        SceneManager.LoadScene("Curriculum");
    }

    public static void changeCurriculmMod(int mod)
    {
        nowMod = mod;
    }

    public static int getNowMod()
    {
        return nowMod;
    }
}
