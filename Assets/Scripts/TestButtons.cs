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
        SpawnHandler.Singleton.SpawnPlayer(SceneEntry.Singleton.SpawnPoint);
    }
}
