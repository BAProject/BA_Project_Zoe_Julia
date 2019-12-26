
using UnityEngine;

public class Controllable : MonoBehaviour
{
    public Transform controlPivot;

    private void Awake()
    {
        if (!controlPivot)
            controlPivot = transform;
    }

    public void ControlTree()
    {
        GameInitialization.instance.ui.ShowTreeUi();
    }

    public void UnControlTree()
    {
        GameInitialization.instance.ui.HideTreeUi();
    }
}
