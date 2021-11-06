using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerStatus : MonoBehaviour
{
    public static PlayerStatus Singleton;

    public int MaxHealth;
    public int CurrentHealth { get; private set; }
    public bool CanDie;
    public SpriteRenderer PlayerSprite;
    public bool CanMove;

    void Awake()
    {
        Singleton = this;
    }

    void Start()
    {
        ResetStatus();
    }

    public void ResetStatus()
    {
        MaxHealth = 3;
        SetHealth(MaxHealth);
        CanMove = true;
        CanDie = true;
        PlayerSprite.color = Color.white;
    }

    public void HandleHealthChange(int healthChange)
    {
        if (CurrentHealth == 0) { return; }

        CurrentHealth = CurrentHealth + healthChange;

        if (CurrentHealth > MaxHealth) { CurrentHealth = MaxHealth; }
        if (CurrentHealth < 0) { CurrentHealth = 0; }
        if (CurrentHealth == 0 && CanDie) { SetPlayerKilled(); }

        SetHealth(CurrentHealth);
    }

    public void SetHealth(int newHealth)
    {
        CurrentHealth = newHealth;
        UiHandler.Singleton.HealthText.text = CurrentHealth.ToString();
    }

    public void SetPlayerKilled()
    {
        PlayerSprite.color = Color.black;
        CanMove = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
