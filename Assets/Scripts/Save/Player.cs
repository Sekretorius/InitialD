using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public Vector3 playerPosition;
    public float speed;
    public float jumpSpeed;
    public GameObject player;

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
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("Saving");
            SavePlayer();
            Level level = new Level();
            level.setLevel(SceneManager.GetActiveScene().name);
            SaveScript.SaveLevel(level);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            Debug.Log("Loding");
            LoadPlayer();
        }
    }
    public void addSettings(float speed = 0, float jumpSpeed = 0)
    {
        this.speed = speed;
        this.jumpSpeed = jumpSpeed;
    }
    public void SavePlayer()
    {
        SaveScript.SavePlayer(this);
    }
    public void LoadPlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        PlayerData playerData = SaveScript.LoadPlayer();
        playerPosition = new Vector3 (playerData.position[0], playerData.position[1], playerData.position[2]);
        speed = playerData.speed;
        jumpSpeed = playerData.jumpSpeed;
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
