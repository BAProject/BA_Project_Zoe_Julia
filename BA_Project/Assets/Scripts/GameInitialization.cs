
using UnityEngine;

public class GameInitialization : MonoBehaviour
{
    public static GameInitialization instance;

    public Level level;
    public UI ui;

    private void Awake()
    {
        instance = this;
    }

}
