using System;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    PlayerSpawnPoint playerSpawnPoint;
    public void Start()
    {
        playerSpawnPoint = GameObject.FindAnyObjectByType<PlayerSpawnPoint>();
        transform.position = playerSpawnPoint.transform.position;
    }
}
