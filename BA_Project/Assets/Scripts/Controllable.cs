
using UnityEngine;

public class Controllable : MonoBehaviour
{
    public Transform controlPivot;
    public Plant controlledPlant;

    private void Awake()
    {
        if (!controlPivot)
            controlPivot = transform;
    }

    public void ControlTree()
    {
        controlledPlant.SetPlantControlled(true);
        GameInitialization.instance.ui.ShowTreeUi();
    }

    public void UnControlTree()
    {
        controlledPlant.SetPlantControlled(false);
        GameInitialization.instance.ui.HideTreeUi();
    }
}
