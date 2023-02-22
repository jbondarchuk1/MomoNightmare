using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DialogueTrigger))]
public class NPCInteractableObject : InteractableObject, IActivatable
{
    private DialogueTrigger dialogueTrigger;
    private bool isActive = false;

    private void Update()
    {
        StartCoroutine(SetActive(DialogueManager.Instance.dialogueBox.activeInHierarchy));
        if (isActivated())
            Deselect();
    }
    private new void Start()
    {
        base.Start();
        dialogueTrigger = GetComponent<DialogueTrigger>();
    }
    public void Activate()
    {
        if (isActivated()) return;

        dialogueTrigger.TriggerDialogue();
    }
    public void Deactivate()
    {
        if (!isActivated() || DialogueManager.Instance.isScrolling) return;

        dialogueTrigger.QuitDialogue();
    }
    private IEnumerator SetActive(bool state)
    {
        yield return new WaitForSeconds(0.2f);
        isActive = state;
    }
    public bool isActivated() => isActive;
}
