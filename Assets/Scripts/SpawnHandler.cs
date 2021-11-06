using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnHandler : MonoBehaviour
{
    // Start is called before the first frame update
    public static SpawnHandler Singleton;
    public GameObject PlayerPrefab;
    public GameObject Player;
    
    void Awake()
    {
        Singleton = this;
    }

    public void DespawnPlayer()
    {
        Destroy(Player);
    }

    public void SpawnPlayer(Transform spawnPoint)
    {
        Player = Instantiate(PlayerPrefab, spawnPoint.position,  Quaternion.identity);
        PlayerCamera.Singleton.SetPlayer(Player.transform);
    }

}