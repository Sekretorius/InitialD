using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public Vector3 playerPosition;
    public float speed;
    public float jumpSpeed;
    public int money;
    public int hearts;
    public GameObject player;
    private GameObject PlayerStats;
    private bool update = false;
    private int loadState = 0;

    public static Player control;
    void Awake()
    {
        if (control == null)
        {
            DontDestroyOnLoad(gameObject);
            control = this;
        }
        else if (control != null)
        {
            Destroy(gameObject);
        }
    }
    void FixedUpdate()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        if (player != null)
        {
            playerPosition = transform.position;
        }
        if (update)
        {
            loadData();
        }
    }
    private void loadData()
    {
        switch (loadState)
        {
            case 0:
                if (MoneySystem.moneySystem != null)
                {
                    MoneySystem.moneySystem.LoadMoney();
                    loadState++;
                }
                break;
            case 1:
                if (HealthSystem.healthSystem != null)
                {
                    HealthSystem.healthSystem.LoadHearts();
                    loadState++;
                }
                break;
            default:
                update = false;
                loadState = 0;
                break;
        }
    }
    public void addSettings(float speed = 0, float jumpSpeed = 0)
    {
        this.speed = speed;
        this.jumpSpeed = jumpSpeed;
    }
    public void UpdateCash(int money)
    {
        if (!update)
        {
            this.money = money;
        }
    }
    public void UpdateHealth(int hearts)
    {
        if (!update)
        {
            this.hearts = hearts;
        }
    }
    public void SavePlayer()
    {
        hearts = HealthSystem.healthSystem.GetFragmentCount();
        SaveScript.SavePlayer(this);
    }
    public void LoadPlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        PlayerData playerData = SaveScript.LoadPlayer();
        playerPosition = new Vector3 (playerData.position[0], playerData.position[1], playerData.position[2]);
        speed = playerData.speed;
        jumpSpeed = playerData.jumpSpeed;
        money = playerData.money;
        hearts = playerData.hearts;
        update = true;
        SetParameters();
    }
    private void SetParameters()
    {
        if(player != null)
        {
            if(player.TryGetComponent(out PlayerControler controler))
            {
                controler.AddSettings(this);
            }
        }
    }
}
