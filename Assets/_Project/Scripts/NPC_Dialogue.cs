using System;
using System.Collections;
using System.Collections.Generic;
using PixelCrushers.DialogueSystem;
using UnityEngine;

public class NPC_Dialogue : MonoBehaviour
{
    
    private WaitForSeconds _initialDelay = new(3.0f);

    [SerializeField] private DialogueSystemTrigger dialogueSystemTrigger;
    
    private IEnumerator Start()
    {
        yield return _initialDelay;

        dialogueSystemTrigger.enabled = true;

    }
}
