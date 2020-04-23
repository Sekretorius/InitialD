using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartVisual : MonoBehaviour
{
    [SerializeField] private Sprite heartSpriteFull;
    [SerializeField] private Sprite heartSprite75;
    [SerializeField] private Sprite heartSprite50;
    [SerializeField] private Sprite heartSprite25;
    [SerializeField] private Sprite heartSpriteEmpty;

    private List<HeartImage> heartImageList;
    private HealthSystem healthSystem;

    private void Awake()
    {
        heartImageList = new List<HeartImage>();
        healthSystem = GameObject.Find("PlayerStats").GetComponent<HealthSystem>();
    }

    private void Start()
    {
        //HealthSystem healthSystem = new HealthSystem(4);
      //  SetHealthSystem(healthSystem);

        List<HealthSystem.Heart> heartList = healthSystem.GetHeartList();
        Vector2 heartAnchoredPosition = new Vector2(0, 0);
        for (int i = 0; i < heartList.Count; i++)
        {
            HealthSystem.Heart heart = heartList[i];
            CreateHeartImage(heartAnchoredPosition).SetHeartFragments(heart.GetFragmentAmount());
            heartAnchoredPosition += new Vector2(25, 0);
        }

        healthSystem.OnChanged += healthSystem_OnChanged;
    }

    private void healthSystem_OnChanged(object sender, EventArgs e)
    {
        for (int i = 0; i < heartImageList.Count; i++)
        {
            HeartImage image = heartImageList[i];
            HealthSystem.Heart heart = healthSystem.GetHeartList()[i];
            image.SetHeartFragments(heart.GetFragmentAmount());
        }
    }

    public void SetHealthSystem(HealthSystem healthSystem)
    {
        this.healthSystem = healthSystem;
    }

    private HeartImage CreateHeartImage(Vector2 anchoredPosition)
    {
        GameObject heart = new GameObject("Heart", typeof(Image));

        heart.transform.SetParent(transform);
        heart.transform.localPosition = Vector3.zero;

        heart.GetComponent<RectTransform>().anchoredPosition = anchoredPosition;
        heart.GetComponent<RectTransform>().sizeDelta = new Vector2(50,50);

        Image heartImageUI = heart.GetComponent<Image>();
        heartImageUI.sprite = heartSpriteFull;

        HeartImage heartImage = new HeartImage(this,heartImageUI);
        heartImageList.Add(heartImage);

        return heartImage;
    }

    public class HeartImage
    {
        private Image heartImage;
        private HeartVisual heartVisual;

        public HeartImage(HeartVisual heartVisual, Image heartImage)
        {
            this.heartVisual = heartVisual;
            this.heartImage = heartImage;
        }

        public void SetHeartFragments(int fragment)
        {
            switch (fragment)
            {
                case 0:
                    heartImage.sprite = heartVisual.heartSpriteEmpty;
                    break;
                case 1:
                    heartImage.sprite = heartVisual.heartSprite25;
                    break;
                case 2:
                    heartImage.sprite = heartVisual.heartSprite50;
                    break;
                case 3:
                    heartImage.sprite = heartVisual.heartSprite75;
                    break;
                case 4:
                    heartImage.sprite = heartVisual.heartSpriteFull;
                    break;
            }
        }

    }
}
