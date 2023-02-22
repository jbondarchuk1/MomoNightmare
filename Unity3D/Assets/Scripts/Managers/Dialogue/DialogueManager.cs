using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }
    private Queue<string> sentences = new Queue<string>();
    public StarterAssetsInputs _inputs;

    private string alphaCode = "<color=#00000000>";
    public GameObject dialogueBox;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text dialogueText;
    [Range(0,2)][SerializeField] private float speed = 1f;

    public bool isScrolling = false;

    private void Start()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);

        _inputs = StarterAssetsInputs.Instance;
    }

    private void Update()
    {
        if (!isScrolling && _inputs.mouseL && sentences.Count >= 0)
            DisplayNextSentence();
    }
    public void StartDialogue(Dialogue dialogue)
    {
        dialogueBox.SetActive(true);
        nameText.text = dialogue.Name;
        sentences.Clear();
        foreach(string s in dialogue.Sentences)
            sentences.Enqueue(s);

        DisplayNextSentence();
        PlayerManager.Instance.playerMovementManager.canMove = false;
    }

    public void DisplayNextSentence()
    {
        dialogueText.text = "";
        if (sentences.Count == 0)
        {
            EndConversation();
            return;
        }

        string sentence = sentences.Dequeue();
        StartCoroutine(ScrollText(sentence));
    }
    private IEnumerator ScrollText(string sentence)
    {
        int alphaIdx = 0;

        isScrolling = true;
        foreach (char c in sentence)
        {
            alphaIdx++;
            dialogueText.text = sentence.Insert(alphaIdx, alphaCode);
            yield return new WaitForSeconds(.1f * Time.deltaTime * speed);
        }
        isScrolling = false;
    }

    public void EndConversation()
    {
        sentences.Clear();
        nameText.text = "";
        dialogueText.text = "";
        StopAllCoroutines();
        dialogueBox.SetActive(false);
        PlayerManager.Instance.playerMovementManager.canMove = true;

    }
}
