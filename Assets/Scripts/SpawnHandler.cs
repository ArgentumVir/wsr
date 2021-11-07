using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnHandler : MonoBehaviour
{
    // Start is called before the first frame update
    public static SpawnHandler Singleton;
    public GameObject PlayerPrefab, EmailPrefab;
    public GameObject Player;
    public List<GameObject> SpawnedEnemies;
    
    void Awake()
    {
        Singleton = this;
    }

    public void DespawnPlayer()
    {
        Destroy(Player);
    }

    public void SpawnPlayer(Vector3 spawnPoint)
    {
        Player = Instantiate(PlayerPrefab, spawnPoint,  Quaternion.identity);
        PlayerCamera.Singleton.SetPlayer(Player.transform);
    }

    public void DespawnEnemies()
    {
        foreach (var enemy in SpawnedEnemies)
        {
            Destroy(enemy);
        }
    }

    public void SpawnEnemies(List<Vector2> enemySpawns)
    {
        SpawnedEnemies = new List<GameObject>();

        foreach (var enemySpawn in enemySpawns)
        {
            var enemy = Instantiate(EmailPrefab, enemySpawn,  Quaternion.identity);
            SpawnedEnemies.Add(enemy);
        }
    }

}
