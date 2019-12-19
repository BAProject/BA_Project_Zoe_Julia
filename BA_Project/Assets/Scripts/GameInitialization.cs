
using UnityEngine;

public class GameInitialization : MonoBehaviour
{
    public static GameInitialization instance;

    public Level level;
    public UI ui;

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

        InitializeGame();
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
