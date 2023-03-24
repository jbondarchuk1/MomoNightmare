using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using static LayerManager;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public Animator animator;
    public CinemachineVirtualCamera virtualCamera;
    public Transform lookAt;

    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("isTalking", false);
        if (lookAt == null) lookAt = transform;
    }

    public void TriggerDialogue()
    {
        DialogueManager.OnDialogueStart += StartDialogue;
        DialogueManager.OnDialogueEnd += EndDialogue;

        DialogueManager.Instance.StartDialogue(dialogue);
    }
    public void QuitDialogue()
    {
        DialogueManager.Instance.EndConversation();
        animator.SetBool("isTalking", false);
    }

    private void StartDialogue()
    {
        animator.SetBool("isTalking", true);
        virtualCamera.enabled = true;
        virtualCamera.LookAt = lookAt;
    }
    private void EndDialogue()
    {
        animator.SetBool("isTalking", false);
        DialogueManager.OnDialogueStart -= StartDialogue;
        DialogueManager.OnDialogueEnd -= EndDialogue;
        virtualCamera.enabled = false;

    }
}
