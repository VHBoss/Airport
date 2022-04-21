using UnityEngine;

public class SettingsReader : MonoBehaviour
{
    public static SettingsReader I;

    public GameConfig gameConfig;

    private void Awake()
    {
        I = this;
    }

    void Start()
    {
        gameConfig.UpdateSettings();
    }
}
