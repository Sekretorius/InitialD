using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public event EventHandler OnChanged;

    public int hearts;

    private List<Heart> heartList;

    void Start()
    {
        SetHearts(hearts);
    }

    public void SetHearts(int heartAmount)
    {
        heartList = new List<Heart>();
        for (int i = 0; i < heartAmount; i++)
        {
            Heart heart = new Heart(4);
            heartList.Add(heart);
        }

        heartList[heartList.Count - 1].SetFragments(0);
    }

    public void AddHeart()
    {
        Heart heart = new Heart(0);
        heartList.Add(heart);
    }

    public void RemoveHeart()
    {
        heartList.RemoveAt(heartList.Count);
    }

    public void Damage(int damage)
    {
        for (int i = heartList.Count-1; i >= 0; i--)
        {
            if (damage <= 0)
                break;
            Heart heart = heartList[i];
            int amount = heart.GetFragmentAmount();
            heart.Damage(damage);
            damage -= amount;
        }

        if (OnChanged != null)
            OnChanged(this, EventArgs.Empty);
    }

    public void Heal(int amount)
    {
        for (int i = 0; i < heartList.Count; i++)
        {
            if (amount <= 0)
                break;
            if (heartList[i].GetFragmentAmount() < 4)
            {
                int current = heartList[i].GetFragmentAmount();
                heartList[i].Heal(amount);
                amount -= current;
            }
        }

        if (OnChanged != null)
            OnChanged(this, EventArgs.Empty);
    }

    public bool IsFullHealth()
    {
        bool isFull = true;
        for (int i = 0; i < heartList.Count; i++)
        {
            if (heartList[i].GetFragmentAmount() < 4)
            {
                isFull = false;
                break;
            }
        }
        return isFull;
    }

    public List<Heart> GetHeartList()
    {
        return heartList;
    }

    public class Heart
    {
        private int fragments;

        public Heart(int fragments)
        {
            this.fragments = fragments;
        }

        public int GetFragmentAmount()
        {
            return fragments;
        }

        public void SetFragments(int fragments)
        {
            this.fragments = fragments;
        }

        public void Damage(int damage)
        {
            fragments = damage >= fragments ? 0 : fragments - damage; 
        }

        public void Heal(int amount)
        {
            fragments = amount + fragments >= 4 ? 4 : amount + fragments;
        }
    }

}
