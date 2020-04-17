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
        CashAdd.GetComponent<Text>().text ="+ " + amount;
        Cash += amount;
        UpdateMoney();
    }

    private void UpdateMoney()
    {
        StartCoroutine(FadeTextToZeroAlpha(FadeSpeed, CashAdd));
        CashText.GetComponent<Text>().text = Cash + " $";
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

}
