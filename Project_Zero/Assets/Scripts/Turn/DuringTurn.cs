using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DuringTurn : MonoBehaviour
{
    [SerializeField]
    private Button endTurn;
    [SerializeField]
    private TMP_Text warningMessage;
    private void Start()
    {
        endTurn.onClick.RemoveAllListeners();
        warningMessage.outlineWidth = 0.2f;
        warningMessage.outlineColor = new Color32(200, 200, 0, 255);
        if (SubjectTree.ableToEndTurn)
        {
            endTurn.GetComponent<Image>().color = Color.white;
            endTurn.onClick.AddListener(EndTurn);
        }
        else
        {
            endTurn.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f);
            endTurn.onClick.AddListener(CannotEndTurnWarning);
        }
    }

    private void EndTurn()
    {
        LoadingSceneManager.LoadScene("AfterTurn");
    }

    private void CannotEndTurnWarning()
    {
        StartCoroutine(WarningMessage("교수가 배치된 과목 8개로 구성된 커리큘럼\n1개 이상을 보유하십시오.", 2.5f));
    }
    IEnumerator WarningMessage(string message, float time = 1.0f)
    {
        warningMessage.text = message;
        warningMessage.enabled = true;
        yield return new WaitForSeconds(time);
        warningMessage.enabled = false;
    }
}
