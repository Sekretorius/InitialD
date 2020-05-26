using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryLineManager : MonoBehaviour
{
    public Case @case;

    public GameObject Player { get; private set; }
    public DialogueManager dialogueManager { get; private set; }
    public Inventory Inv { get; private set; }
    public MoneySystem Cash { get; private set; }

    public GameObject caseManager;

    public Text caseName;
    public GameObject caseObject;

    public GameObject prefab;

    public bool onMission;
    public bool active;

    private Vector3 position;
    private Vector3 tempPosition;
    private List<GameObject> list = new List<GameObject>();

    private void Start()
    {
        Player = GameObject.FindWithTag("Player");
        dialogueManager = FindObjectOfType<DialogueManager>();
        Inv = FindObjectOfType<Inventory>();
        Cash = FindObjectOfType<MoneySystem>();

        position = prefab.transform.position;
        prefab.SetActive(false);
        caseObject.SetActive(false);
        active = false;
        ShowOnScreen();
        onMission = false;
    }

    void Update()
    {
        OnEvent();
    }

    public void SetCase(Case c)
    {
        //foreach (Case c in caseManager.GetComponents<Case>())
        //{
        //    if (c.id == id)
        //    {
        //        c.Begin = true;
        //        @case = c;
        //        break;
        //    }
        //}
        if (c != null)
        {
            c.Begin = true;
            @case = c;
            onMission = true;
        }
        ShowOnScreen();
    }

    private void OnEvent()
    {
        if (Input.GetKeyDown(KeyCode.C))
            if (active)
            {
                caseObject.SetActive(false);
                active = false;
            }
            else
            {
                caseObject.SetActive(true);
                active = true;
            }
    }

    public void GoalUpdate()
    {
        int count = 0;
        foreach (GameObject goal in list)
            goal.GetComponent<Text>().text = @case.Goals[count++].getDescription();
    }

    public void ShowOnScreen()
    {
        tempPosition = position;
        foreach (GameObject obj in list)
        {
            Destroy(obj);
            list = new List<GameObject>();
        }

        if (@case != null)
        {
            caseName.text = @case.caseName;

            foreach (Goal goal in @case.Goals)
            {
                GameObject temp = Instantiate(prefab);
                temp.SetActive(true);
                temp.GetComponent<Text>().text = goal.getDescription();
                temp.transform.position = tempPosition;
                tempPosition = new Vector3(tempPosition.x, tempPosition.y - 20, tempPosition.z);
                temp.transform.SetParent(caseObject.transform);
                temp.transform.localScale = new Vector3(1, 1, 1);
                list.Add(temp);
            }
        }
        else
        {
            caseName.text = "NO CURRENT CASES";
            GameObject temp = Instantiate(prefab);
            temp.SetActive(true);
            //temp.GetComponent<Text>().text = "DO NOTHING 0/1 •";
            temp.GetComponent<Text>().text = "";
            temp.transform.position = tempPosition;
            tempPosition = new Vector3(tempPosition.x, tempPosition.y - 20, tempPosition.z);
            temp.transform.SetParent(caseObject.transform);
            temp.transform.localScale = new Vector3(1, 1, 1);
            list.Add(temp);
        }
    }

    
}
