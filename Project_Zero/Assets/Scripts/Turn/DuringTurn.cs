using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DuringTurn : MonoBehaviour
{
    [SerializeField]
    private Button endTurn;
    private void Awake()
    {
        endTurn.onClick.AddListener(EndTurn);
    }

    private void Update()
    {
        endTurn.gameObject.SetActive(SubjectTree.professorManagingSubjectCnt >= 8);
    }

    private void EndTurn()
    {
        SceneManager.LoadScene("AfterTurn");
    }
}
