using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InfoButton : MonoBehaviour
{
    [SerializeField]
    private int id;
    private Button infoButton;
    private string content;
    private string sceneName;
    private void Awake()
    {
        infoButton = GetComponent<Button>();
        sceneName = SceneManager.GetActiveScene().name;
        content = InfoButtonManager.getContent(sceneName, id);
        infoButton.onClick.RemoveAllListeners();
        infoButton.onClick.AddListener(
            delegate
            {
                activeInfo();
            }
        );
    }

    private void activeInfo()
    {
    }
}
