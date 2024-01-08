using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class curriculumModManager : MonoBehaviour
{
    [SerializeField]
    private Transform subjectGetterTransform;
    [SerializeField]
    private List<Transform> canvasList;

    void changeMod(int mod)
    {
        subjectGetterTransform.GetComponent<CurriculumSubjectGetter>().setMod(mod);
        for (int i = 0; i < canvasList.Count; i++)
            canvasList[mod].gameObject.SetActive(mod == i);
        
    }
}
