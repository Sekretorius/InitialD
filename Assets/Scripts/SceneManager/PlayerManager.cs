using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    GameObject player;
    public GameObject playerPrefab; 
    public Transform startPosition;

    private void Awake()
    {
        if(SceneManager.GetActiveScene().buildIndex != 0)
        {
            CreatePlayer();
        }
    }
    public GameObject CreatePlayer()
    {
        startPosition = GameObject.FindGameObjectWithTag("Spawn").transform;
        if (player == null && startPosition != null)
        {
            GameObject playerInstance = Instantiate(playerPrefab, startPosition.position, Quaternion.identity);
            return playerInstance;
        }
        else if(player == null)
        {
            GameObject playerInstance = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
            return playerInstance;
        }
        return null;
    }
    public void LoadPlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            if (player.TryGetComponent(out Player controler))
            {
                controler.LoadPlayer();
            }
        }
    }
    public void SetPosition(Vector3 pos)
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        if (player != null)
        {
            player.transform.position = pos;
        }
    }
}
