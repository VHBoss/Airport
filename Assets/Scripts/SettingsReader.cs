using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsReader : MonoBehaviour
{
    public GameConfig gameConfig;

    void Start()
    {
        gameConfig.UpdateSettings();
    }
}
