using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public float speed { get; set; }
    public float jumpSpeed { get; set; }
    public float[] position { get; set; }

    public PlayerData(Player player)
    {
        speed = player.speed;
        jumpSpeed = player.jumpSpeed;
        position = new float[3];
        position[0] = player.playerPosition.x;
        position[1] = player.playerPosition.y;
        position[2] = player.playerPosition.z;
    }
}
