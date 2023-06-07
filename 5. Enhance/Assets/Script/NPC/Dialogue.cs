using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public class DialogueText
{
    [TextArea]
    public string dialogue;
}

public class Dialogue : MonoBehaviour
{
    [SerializeField]
    private DialogueText[] dialogueTexts;

    TextMeshProUGUI text;

    //int count = 0;

    public void ShowDialogue()
    {
        ShowOnOff(true);

        //count = 0;
    }

    public void ShowOnOff(bool flag)
    {
        gameObject.SetActive(flag);
        text.gameObject.SetActive(flag);
    }

    public void NextDialogue(int count)
    {
        text.text = dialogueTexts[count].dialogue;  
    }

    private void Awake()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        ShowOnOff(false);
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        if (count < dialogueTexts.Length)
    //        {
    //            NextDialogue(count);
    //            count++;
    //        }
    //        else
    //        {
    //            ShowOnOff(false);
    //        }
    //    }
    //}

}
