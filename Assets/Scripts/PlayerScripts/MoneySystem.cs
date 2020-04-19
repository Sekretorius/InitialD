using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneySystem : MonoBehaviour
{
    public int Cash;
    public int MinLimit;
    public float FadeSpeed;
    public Text CashAdd;
    public Text CashText;

    private Color green; 
    private Color red;
    private bool active;
    

    void Start()
    {
        ColorUtility.TryParseHtmlString("#238C20", out green);
        ColorUtility.TryParseHtmlString("#9D292F", out red);

        if (Cash >= 0)
            CashText.color = green;
        else
            CashText.color = red;
        CashText.GetComponent<Text>().text = Cash + " $";
        CashAdd.color = new Color(CashAdd.color.r, CashAdd.color.g, CashAdd.color.b, 0);
        active = false;
    }

    public void Add(int amount)
    {
        if(active == true)
        {
            StopAllCoroutines();
            Debug.Log("Worked");
        }
        if(amount > 0)
            CashAdd.GetComponent<Text>().text ="+ " + amount;
        else
            CashAdd.GetComponent<Text>().text = amount.ToString();
        UpdateMoney(amount);
        active = true;
    }

    private void UpdateMoney(int amount)
    {
        if (amount > 0)
        {
            CashAdd.color = new Color(green.r, green.g, green.b, 0);
            StartCoroutine(CoolCashAnimationAddition(amount, CashText));
        }
        else
        {
            CashAdd.color = new Color(red.r, red.g, red.b, 0);
            StartCoroutine(CoolCashAnimationSubtraction(amount, CashText));
        }
        StartCoroutine(FadeTextToZeroAlpha(FadeSpeed, CashAdd));
        Cash += amount;
    }

    public IEnumerator FadeTextToZeroAlpha(float t, Text i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
        while (i.color.a > 0.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
            yield return null;
        }
    }

    public IEnumerator CoolCashAnimationAddition(int amount, Text i)
    {
        int temp = Cash;
        int CashTemp = Cash;
        float time = 5;
       
        float x = 1;    
        for (int t = 2; t <= amount; t++)
            x += t;
        x = time / x;

        int count = 0;
        while (temp <= CashTemp + amount)
        {
            i.color = temp >= 0 ? green : red;
            CashText.GetComponent<Text>().text = temp + " $";
            temp++;
            yield return new WaitForSeconds(count++ * x);
        }
        active = false;
    }

    public IEnumerator CoolCashAnimationSubtraction(int amount, Text i)
    {
        amount = -1 * amount;
        int temp = Cash;
        int CashTemp = Cash;
        float time = 5;

        float x = 1;
        for (int t = 2; t <= amount; t++)
            x += t;
        x = time / x;

        int count = 0;
        while (temp >= CashTemp - amount)
        {
            i.color = temp >= 0 ? green : red;
            CashText.GetComponent<Text>().text = temp + " $";
            temp--;
            yield return new WaitForSeconds(count++ * x);
        }
        active = false;
    }

}
