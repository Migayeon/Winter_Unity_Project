using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class curriculumModManager : MonoBehaviour
{
    [SerializeField]
    private Transform subjectGetterTransform;
    [SerializeField]
    private List<Transform> canvasList;

    private static int nowMod = 0;

    void changeMod(int mod)
    {
        subjectGetterTransform.GetComponent<CurriculumSubjectGetter>().setMod(mod);
        for (int i = 0; i < canvasList.Count; i++)
        {
            canvasList[i].gameObject.SetActive(mod == i);
        }
    }

    private void Awake()
    {
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
}
