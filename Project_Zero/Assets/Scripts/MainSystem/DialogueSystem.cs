using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{
    public static string situation = null;
    public GameObject dialogueUI;
    public GameObject endMark;

    public Button skipButton;
    public Text character;
    public Text message;
    public Image imageUI;
    public MainSceneUIManager mainSceneUIManager;

    struct content
    {
        public string[] message;
    }

    private content dialogue;
    private int dialogueLength;
    private int dialogueIndex;
    private string[] dialogueString;
    private string prevObject;

    private bool isNowAnimation;

    IEnumerator DialogueAnimation(string str)
    {
        foreach (char letter in str)
        {
            message.text += letter;
            yield return new WaitForSeconds(0.05f);
        }
        isNowAnimation = false;
        endMark.SetActive(true);
    }

    private void DialogueProcess()
    {
        if(isNowAnimation)
        {
            StopAllCoroutines();
            message.text = dialogueString[1];
            isNowAnimation = false;
            endMark.SetActive(true);
        }
        else
        {
            endMark.SetActive(false);
            if (dialogueIndex == dialogueLength)
            {
                CloseDialogue();
                return;
            }
            message.text = string.Empty;

            dialogueString = dialogue.message[dialogueIndex].Split(":");

            if (dialogueString[0] == "&O")
            {
                if(prevObject != "none")
                {
                    GameObject.Find(prevObject).GetComponent<Image>().color = Color.white;
                }
                if (dialogueString[1] != "none")
                {
                    GameObject.Find(dialogueString[1]).GetComponent<Image>().color = Color.green;
                }
                prevObject = dialogueString[1];
                isNowAnimation = false;
                dialogueIndex++;
                DialogueProcess();
                return;
            }
            else if (dialogueString[0] == "&I")
            {
                if (dialogueString[1] == "Close")
                {
                    imageUI.sprite = null;
                    imageUI.gameObject.SetActive(false);
                }
                else
                {
                    imageUI.sprite = Resources.Load<Sprite>("Image/Tutorial/"+dialogueString[1]);
                    imageUI.gameObject.SetActive(true);
                }
                isNowAnimation = false;
                dialogueIndex++;
                DialogueProcess();
                return;
            }
            else if(dialogueString[0] == "&M")
            {
                mainSceneUIManager.NextTab();
                isNowAnimation = false;
                dialogueIndex++;
                DialogueProcess();
                return;
            }

            
            character.text = dialogueString[0];
            isNowAnimation = true;
            StartCoroutine(DialogueAnimation(dialogueString[1]));
            dialogueIndex++;
        }
    }

    private void CloseDialogue()
    {
        dialogueUI.SetActive(false);
        skipButton.gameObject.SetActive(false);
        dialogueUI.GetComponent<Button>().onClick.RemoveAllListeners();
        situation = null;
    }
    
    public void SkipDialogue()
    {
        if (prevObject != "none")
        {
            GameObject.Find(prevObject).GetComponent<Image>().color = Color.white;
        }
        imageUI.sprite = null;
        imageUI.gameObject.SetActive(false);
        isNowAnimation = false;
        CloseDialogue();
    }

    private void Awake()
    {
        if (situation != null)
        {
            StartDialogue();
            skipButton.gameObject.SetActive(true);
            skipButton.onClick.RemoveAllListeners();
            skipButton.onClick.AddListener(SkipDialogue);
        }
        else
        {
            dialogueUI.SetActive(false);
            skipButton.gameObject.SetActive(false);
        }
    }

    public void StartDialogue()
    {
        prevObject = "none";
        TextAsset json = Resources.Load<TextAsset>("Dialogue/" + situation);
        dialogue = JsonUtility.FromJson<content>(json.ToString());
        dialogueLength = dialogue.message.Length;
        dialogueIndex = 0;
        isNowAnimation = false;
        dialogueUI.SetActive(true);
        dialogueUI.GetComponent<Button>().onClick.RemoveAllListeners();
        dialogueUI.GetComponent<Button>().onClick.AddListener(DialogueProcess);
        DialogueProcess();
    }

    public void SimpleDialogue()
    {
        int maximum = 0;
        string sceneName = SceneManager.GetActiveScene().name;
        if(sceneName == "Main")
        {
            maximum = 3;
        }
        else if(sceneName == "BeforeTurn")
        {
            maximum = 1;
        }
        situation = sceneName + "/" + Random.Range(0, maximum).ToString();
        StartDialogue();
    }
}
