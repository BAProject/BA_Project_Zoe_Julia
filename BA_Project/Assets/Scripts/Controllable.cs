
using UnityEngine;

public class Controllable : MonoBehaviour
{
    public void ControlTree()
    {
        GameInitialization.instance.ui.ShowTreeUi();
    }

    public void UnControlTree()
    {
        GameInitialization.instance.ui.HideTreeUi();
    }
}
