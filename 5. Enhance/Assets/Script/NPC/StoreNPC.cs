using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreNPC : MonoBehaviour
{
    Dialogue dialogue;

    private void Awake()
    { 
        dialogue = FindObjectOfType<Dialogue>();
    }
        
    /// <summary>
    /// 애니메이션 이벤트 시스템(Dialogue 출력)
    /// </summary>
    void StartAnim()
    {
        dialogue.ShowOnOff(true);
        dialogue.NextDialogue(0);
    }

}
