
using UnityEngine;

public class GameInitialization : MonoBehaviour
{
    public static GameInitialization instance;

    public Config config;

    public Level level;
    public UI ui;
    public PlayerCamera playerCamera;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        CheckReferences();
        InitializeGame();
    }

    private void CheckReferences()
    {
        if (!level)
            level = FindObjectOfType<Level>();

        if (!ui)
            ui = FindObjectOfType<UI>();

        if (!playerCamera)
            playerCamera = FindObjectOfType<PlayerCamera>();
    }

    private void InitializeGame()
    {
        InitializeUI();
    }

    private void InitializeUI()
    {
        ui.HideTreeUi();
        ui.HideControllableUi();
    }
}
