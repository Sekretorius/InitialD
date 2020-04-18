using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneySystem : MonoBehaviour
{
    public int Cash;
    public float FadeSpeed;
    public Text CashAdd;
    public Text CashText;
    

    void Start()
    {
        CashText.GetComponent<Text>().text = Cash + " $";
        CashAdd.color = new Color(CashAdd.color.r, CashAdd.color.g, CashAdd.color.b, 0);
    }

    public void Add(int amount)
    {
        UpdateMoney(amount);
        CashAdd.GetComponent<Text>().text ="+ " + amount;

    }

    private void UpdateMoney(int amount)
    {
        StartCoroutine(FadeTextToZeroAlpha(FadeSpeed, CashAdd));
        StartCoroutine(CoolCashAnimation(amount, CashText));       
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

    public IEnumerator CoolCashAnimation(int amount, Text i)
    {
        int temp = Cash;
        float time = 5;
       
        float x = 1;    
        for (int t = 2; t <= amount; t++)
            x += t;
        x = time / x;

        int count = 0;
        while (temp <= Cash + amount)
        {
            CashText.GetComponent<Text>().text = temp + " $";
            temp += 1;
            yield return new WaitForSeconds(count++ * x);
        }
        Cash += amount;
    }

}
