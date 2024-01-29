using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{
    public static string situation = null;
    public GameObject dialogueUI;
    public Button dialogueButton;
    public Text character;
    public Text message;

    struct content
    {
        public string[] message;
    }

    private content dialogue;
    private int dialogueLength;
    private int dialogueIndex;
    private string[] dialogueString;

    private bool isNowAnimation;

    IEnumerator DialogueAnimation(string str)
    {
        foreach (char letter in str)
        {
            message.text += letter;
            yield return new WaitForSeconds(0.1f);
        }
        isNowAnimation = false;
    }

    private void DialogueProcess()
    {
        if(isNowAnimation)
        {
            StopAllCoroutines();
            message.text = dialogueString[1];
            isNowAnimation = false;
        }
        else
        {
            if (dialogueIndex == dialogueLength)
            {
                CloseDialogue();
                return;
            }
            isNowAnimation = true;
            message.text = string.Empty;

            dialogueString = dialogue.message[dialogueIndex].Split(":");
            character.text = dialogueString[0];
            if (dialogueString[0] == "&E")
            {
                isNowAnimation = true;
                dialogueIndex++;
                return;
            }
            StartCoroutine(DialogueAnimation(dialogueString[1]));
            dialogueIndex++;
        }
    }

    private void CloseDialogue()
    {
        dialogueUI.SetActive(false);
        dialogueButton.onClick.RemoveAllListeners();
        situation = null;
    }

    private void Awake()
    {
        if (situation != null)
        {
            TextAsset json = Resources.Load<TextAsset>("Dialogue/" + situation);
            dialogue = JsonUtility.FromJson<content>(json.ToString());
            dialogueLength = dialogue.message.Length;
            dialogueIndex = 0;
            isNowAnimation = false;
            dialogueUI.SetActive(true);
            dialogueButton.onClick.AddListener(DialogueProcess);
            DialogueProcess();
        }
        else
        {
            dialogueUI.SetActive(false);
        }
    }
}
