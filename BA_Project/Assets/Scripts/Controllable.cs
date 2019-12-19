
using UnityEngine;

public class Controllable : MonoBehaviour
{
    public void ControllTree()
    {
        GameInitialization.instance.ui.ShowTreeUi();
    }

    public void UnControllTree()
    {
        GameInitialization.instance.ui.HideTreeUi();
    }
}
