
using UnityEngine;

public class ControllingTrees : MonoBehaviour
{
    [SerializeField] private float _treeSearchRadius;
    [SerializeField] private CharacterController _characterController;

    private Controllable _controllableTree;
    private bool _isControlling;
    private bool _isInRange;

    private void Awake()
    {
        _isControlling = false;
        _isInRange = false;
    }

    public void DisablePlayer()
    {
        _characterController.enabled = false;
    }

    public void EnablePlayer()
    {
        _characterController.enabled = true;
    }

    private void Update()
    {
        if (_isControlling)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                UnControllTree();
            }

            return;
        }

        ScanRadiusForControllable();

        if (!_isInRange)
        {
            GameInitialization.instance.ui.HideControllableUi();
        }
        else
        {
            GameInitialization.instance.ui.ShowControllableUi();

            if (Input.GetKeyDown(KeyCode.F))
            {
                GameInitialization.instance.ui.HideControllableUi();
                ControllTree();
            }
        }
    }

    private void ScanRadiusForControllable()
    {
        _controllableTree = null;
        Collider[] colliders = Physics.OverlapSphere(transform.position, _treeSearchRadius);

        foreach (Collider collider in colliders)
        {
            GameObject obj = collider.gameObject;
            if (obj.tag == "Tree")
                _controllableTree = obj.GetComponent<Controllable>();
        }

        _isInRange = _controllableTree == null ? false : true;
    }

    private void ControllTree()
    {
        Debug.Log("Controlling Tree");
        _isControlling = true;
        DisablePlayer();
        _controllableTree.ControllTree();
    }

    private void UnControllTree()
    {
        Debug.Log("Un-Controlling Tree");
        _controllableTree.UnControllTree();
        EnablePlayer();
        _isControlling = false;
    }
}
