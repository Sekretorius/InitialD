using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public GameObject boxTalk;
    public GameObject boxQuestion;

    public GameObject boxPlayer;
    public GameObject boxNPC;

    public float letterSpeed;

    private Queue<string> sentences;
    private Queue<string> responses;
    private bool turn;


    // for positions
    private GameObject Player; 
    private GameObject NPC;

    private Text dialogueTextNPC;
    private Text dialogueTextPlayer;
    //public GameObject Camera;

    public bool Chat { get; private set; }
    public bool Interacted { get; private set; }
    public bool Mission { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        boxPlayer.SetActive(true);
        boxNPC.SetActive(true);

        sentences = new Queue<string>();
        responses = new Queue<string>();
        turn = false;
        Chat = false;
        Interacted = false;
        Mission = false;
        dialogueTextNPC = boxNPC.GetComponentInChildren<Text>();
        dialogueTextPlayer = boxPlayer.GetComponentInChildren<Text>();
    }

    void Update()
    {
        ShowInteractions();
        SetBoxPositions();
    }

    public void StartInteractions(GameObject player, GameObject npc, bool mission)
    {
        boxTalk.SetActive(true);
        boxQuestion.SetActive(true);
        Player = player;
        this.NPC = npc;
        Interacted = true;
        Mission = mission;
    }

    public void ShowInteractions()
    {
        if (Interacted && Mission == true)
        {
            //SetTalkBox(0.5f);
            SetQuestionBox(0.1f);
        }
        else if (Interacted)
        {
            SetTalkBox(0);
        }
    }

    public void DisableInteractions()
    {
        boxTalk.SetActive(false);
        boxQuestion.SetActive(false);
        Interacted = false;
    }

    public void SetTalkBox(float offset)
    {
        float top = 50f;
        top = boxTalk.GetComponent<SpriteRenderer>().bounds.size.y;
        Vector3 position = new Vector3(NPC.transform.position.x - offset, NPC.transform.position.y + top + 0.9f, 0);
        boxTalk.GetComponent<Transform>().position = position;
    }

    public void SetQuestionBox(float offset)
    {
        float top = 10f;
        top = boxQuestion.GetComponent<SpriteRenderer>().bounds.size.y;
        Vector3 position = new Vector3(NPC.transform.position.x - offset, NPC.transform.position.y + top + 1f, 0);
        boxQuestion.GetComponent<Transform>().position = position;
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
                position = new Vector3(Player.transform.position.x, Player.transform.position.y + top + 0.5f, 0);
                boxPlayer.GetComponent<Transform>().position = position;
                // Fade away
                float delta = 4f - Mathf.Abs(NPC.transform.position.x - Player.transform.position.x);
                boxPlayer.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, delta);
                Text temp = dialogueTextPlayer.GetComponent<Text>();
                temp.color = new Color(temp.color.r, temp.color.g, temp.color.b, delta);
            }
            else
            {
                //dialogueTextPlayer.GetComponent<RectTransform>().transform.position = position;

                // Chat box position
                top = boxNPC.GetComponent<SpriteRenderer>().bounds.size.y;
                position = new Vector3(NPC.transform.position.x, NPC.transform.position.y + top + 0.5f, 0);
                boxNPC.GetComponent<Transform>().position = position;
                // Fade away
                float delta = 4f - Mathf.Abs(NPC.transform.position.x - Player.transform.position.x);
                boxNPC.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, delta);
                Text temp = dialogueTextNPC.GetComponent<Text>();
                temp.color = new Color(temp.color.r, temp.color.g, temp.color.b, delta);

            }
          
        }
    }

    public void StopDialogue()
    {
       // Debug.Log("End of Conversation");
        Chat = false;
        boxPlayer.SetActive(false);
        boxNPC.SetActive(false);
    }
    

    public void StartDialogue(Dialogue dialogue, GameObject player, GameObject NPC)
    {
        DisableInteractions();
        Interacted = false;
        //Mission = false;

        Player = player;
        this.NPC = NPC;
        Chat = true;
       // Debug.Log("Conversation started");
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
            NPC.GetComponent<DialogueTrigger>().RemoveDialogue();
            NPC.GetComponent<Collider2D>().tag = "NPC_Ignored";
            if (Mission)
            {
                GameObject.Find("caseManager").GetComponent<StoryLineManager>().setCase(NPC.GetComponent<DialogueTrigger>().id);
                Mission = false;
            }
            return;
        }
        if (turn || sentences.Count == 0)
            DisplayPlayer();
        else if (!turn || responses.Count == 0)
            DisplayNPC();
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueTextPlayer.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueTextPlayer.text += letter;
            yield return new WaitForSeconds(letterSpeed);
        }
    }

    IEnumerator TypeSentence_npc(string sentence)
    {
        dialogueTextNPC.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueTextNPC.text += letter;
            yield return new WaitForSeconds(letterSpeed);
        }
    }

    public void DisplayPlayer()
    {
        boxPlayer.SetActive(true);
        boxNPC.SetActive(false);
        turn = false;
        string sentence = responses.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    public void DisplayNPC()
    {
        boxPlayer.SetActive(false);
        boxNPC.SetActive(true);
        turn = true;
        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence_npc(sentence));
    }
}
