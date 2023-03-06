using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("isTalking", false);
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
    }
    private void EndDialogue()
    {
        animator.SetBool("isTalking", false);
        DialogueManager.OnDialogueStart -= StartDialogue;
        DialogueManager.OnDialogueEnd -= EndDialogue;
    }
}
