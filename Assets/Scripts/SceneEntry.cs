using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneEntry : MonoBehaviour
{
    public static SceneEntry Singleton;
    public Transform SpawnPoint;
    void Awake()
    {
        Singleton = this;
    }
    void Start()
    {
        // SpawnHandler.Singleton.SpawnPlayer(SpawnPoint);
        LevelGenerator.Singleton.GenerateLevel();
        SpawnHandler.Singleton.SpawnEnemies(
            LevelGenerator.Singleton.EnemySpawns
        );
        SpawnHandler.Singleton.SpawnPlayer(
            LevelGenerator.Singleton.SpawnPoint
        );
    }
}
