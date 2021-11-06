using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestButtons : MonoBehaviour
{

    public void DecrementPlayerHealth()
    {
        PlayerStatus.Singleton.HandleHealthChange(-1);
    }

    public void IncrementPlayerHealth()
    {
        PlayerStatus.Singleton.HandleHealthChange(1);
    }

    public void RespawnPlayer()
    {
        SpawnHandler.Singleton.DespawnPlayer();
        SpawnHandler.Singleton.SpawnPlayer(LevelGenerator.Singleton.SpawnPoint);
    }

    public void RegenerateWorld()
    {
        SpawnHandler.Singleton.DespawnPlayer();
        LevelGenerator.Singleton.DestroyLevel();
        LevelGenerator.Singleton.GenerateLevel();
        SpawnHandler.Singleton.SpawnPlayer(LevelGenerator.Singleton.SpawnPoint);
    }

    public void ZoomOut()
    {
        PlayerCamera.Singleton.Camera.orthographicSize += 5;
    }
}
