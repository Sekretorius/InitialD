using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public GameObject UI;

    public Text nameNPC;
    public Text dialogueTextNPC;

    public Text namePlayer;
    public Text dialogueTextPlayer;

    private Queue<string> sentences;
    private Queue<string> responses;
    private bool turn;

    private 
    // Start is called before the first frame update
    void Start()
    {
        UI.SetActive(false);
        sentences = new Queue<string>();
        responses = new Queue<string>();
        turn = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartDialogue(Dialogue dialogue)
    {
        UI.SetActive(true);

        Debug.Log("Conversation with " + dialogue.name);
        sentences.Clear();
        responses.Clear();

        nameNPC.GetComponent<Text>().text = dialogue.name;
        namePlayer.GetComponent<Text>().text = "William";

        // Queueing NPC sentences
        foreach (string sentence in dialogue.sentences)
            sentences.Enqueue(sentence);
        // Queueing player responses
        foreach (string response in dialogue.responses)
            responses.Enqueue(response);

        turn = dialogue.isPlayerFirst;

        DisplayDialogue();
    }

    public void DisplayDialogue()
    {
        if(sentences.Count == 0 && responses.Count == 0)
        {
            Debug.Log("End of Conversation");
            UI.SetActive(false);
            return;
        }

        if (turn || sentences.Count == 0)
            DisplayPlayer();
        else if (!turn || responses.Count == 0)
            DisplayNPC();
    }

    public void DisplayPlayer()
    {
        dialogueTextPlayer.color = Color.black;
        namePlayer.color = Color.black;
        dialogueTextNPC.color = Color.grey;
        nameNPC.color = Color.grey;

        turn = false;
        string sentence = responses.Dequeue();
        dialogueTextPlayer.text = sentence;
        dialogueTextNPC.text = "";
    }

    public void DisplayNPC()
    {
        namePlayer.color = Color.grey;
        dialogueTextPlayer.color = Color.grey;

        dialogueTextNPC.color = Color.black;
        nameNPC.color = Color.black;

        turn = true;
        string sentence = sentences.Dequeue();
        dialogueTextNPC.text = sentence;
        dialogueTextPlayer.text = "";
    }
}
