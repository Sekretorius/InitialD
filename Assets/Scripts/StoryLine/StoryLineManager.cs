using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryLineManager : MonoBehaviour
{
    public Case @case;
    public GameObject caseManager;


    public Text caseName;
    public GameObject caseObject;

    public GameObject prefab;

    public bool active;

    private Vector3 position;
    private Vector3 tempPosition;
    private List<GameObject> list = new List<GameObject>();

    void Update()
    {
        OnEvent();
    }

    public void setCase(int id)
    {
        foreach (Case c in caseManager.GetComponents<Case>())
            if (c.id == id)
            {
                @case = c;
                break;
            }
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

    void Start()
    {
        setCase(0);
        position = prefab.transform.position;
        prefab.SetActive(false);
        caseObject.SetActive(false);
        active = false;
        ShowOnScreen();
    }

    public void ShowOnScreen()
    {
        tempPosition = position;
        foreach (GameObject obj in list)
            Destroy(obj);

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
            temp.GetComponent<Text>().text = "DO NOTHING 0/1 •";
            temp.transform.position = tempPosition;
            tempPosition = new Vector3(tempPosition.x, tempPosition.y - 20, tempPosition.z);
            temp.transform.SetParent(caseObject.transform);
            temp.transform.localScale = new Vector3(1, 1, 1);
            list.Add(temp);
        }

    }

    
}
