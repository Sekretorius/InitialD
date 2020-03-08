using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{

    public Text dialogueTextNPC;
    public Text dialogueTextPlayer;

    public GameObject boxPlayer;
    public GameObject boxNPC;

    private Queue<string> sentences;
    private Queue<string> responses;
    private bool turn;


    // for positions
    private GameObject Player; 
    private GameObject NPC;
    //public GameObject Camera;

    public bool Chat { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        boxPlayer.SetActive(false);
        boxNPC.SetActive(false);
        sentences = new Queue<string>();
        responses = new Queue<string>();
        turn = false;
        Chat = false;
    }

    void Update()
    {
        SetBoxPositions();
    }

    public void SetBoxPositions()
    {
        if (Chat)
        {            
            float top = 0f;
            Vector3 position = new Vector3();
            if (!turn)
            {
                // Chat box position
                top = boxPlayer.GetComponent<SpriteRenderer>().bounds.size.y;
                position = new Vector3(Player.transform.position.x, Player.transform.position.y + top, 0);
                boxPlayer.GetComponent<Transform>().position = position;
                // Text position
                dialogueTextPlayer.transform.position = position;
                // Fade away
                float delta = 4f - Mathf.Abs(boxNPC.transform.position.x - Player.transform.position.x);
                boxPlayer.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, delta);
                Text temp = dialogueTextPlayer.GetComponent<Text>();
                temp.color = new Color(temp.color.r, temp.color.g, temp.color.b, delta);
            }
            else
            {
                //dialogueTextPlayer.GetComponent<RectTransform>().transform.position = position;

                // Chat box position
                top = boxNPC.GetComponent<SpriteRenderer>().bounds.size.y;
                position = new Vector3(NPC.transform.position.x, NPC.transform.position.y + top, 0);
                boxNPC.GetComponent<Transform>().position = position;
                // Text position
                dialogueTextNPC.GetComponent<RectTransform>().transform.position = position;
                // Fade away
                float delta = 4f - Mathf.Abs(boxNPC.transform.position.x - Player.transform.position.x);
                boxNPC.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, delta);
                Text temp = dialogueTextNPC.GetComponent<Text>();
                temp.color = new Color(temp.color.r, temp.color.g, temp.color.b, delta);

            }
          
        }
    }

    public void StopDialogue()
    {
        Debug.Log("End of Conversation");
        Chat = false;
        boxPlayer.SetActive(false);
        boxNPC.SetActive(false);
    }
    

    public void StartDialogue(Dialogue dialogue, GameObject player, GameObject NPC)
    {
        Player = player;
        this.NPC = NPC;
        Chat = true;
        Debug.Log("Conversation started");
        sentences.Clear();
        responses.Clear();

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
            StopDialogue();
            return;
        }
        if (turn || sentences.Count == 0)
            DisplayPlayer();
        else if (!turn || responses.Count == 0)
            DisplayNPC();
    }

    public void DisplayPlayer()
    {
        boxPlayer.SetActive(true);
        boxNPC.SetActive(false);
        turn = false;
        string sentence = responses.Dequeue();
        dialogueTextPlayer.text = sentence;
        //dialogueTextNPC.text = "";
    }

    public void DisplayNPC()
    {
        boxPlayer.SetActive(false);
        boxNPC.SetActive(true);
        turn = true;
        string sentence = sentences.Dequeue();
        dialogueTextNPC.text = sentence;
        //dialogueTextPlayer.text = "";
    }
}
