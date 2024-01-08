using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class curriculumModManager : MonoBehaviour
{
    [SerializeField]
    private Transform subjectGetterTransform;
    [SerializeField]
    private List<Transform> canvasList;

    public static int c_mod = 0;

    void changeMod(int mod)
    {
        subjectGetterTransform.GetComponent<CurriculumSubjectGetter>().setMod(mod);
        for (int i = 0; i < canvasList.Count; i++)
        {
            canvasList[i].gameObject.SetActive(mod == i);
        }
        
    }

    private void Start()
    {
        changeMod(c_mod);
        Debug.Log("Test Done");
    }
}
