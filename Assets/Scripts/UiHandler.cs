using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiHandler : MonoBehaviour
{
    public static UiHandler Singleton;
    public Text HealthText;
    
    void Awake()
    {
        Singleton = this;
    }
}
