
using Bolt;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ControllingTrees : MonoBehaviour
{
    [SerializeField] private KeyCode _controlKey;
    [SerializeField] private float _treeSearchRadius;
    [SerializeField] private PlayerMovement _characterController;

    private Controllable _controllableTree;
    private List<Controllable> _treesInRange;
    private bool _isControlling;
    private bool _isInTreeRange;

    private void Awake()
    {
        _isControlling = false;
        _isInTreeRange = false;
        _treesInRange = new List<Controllable>();
    }

    public void DisableMovement()
    {
        _characterController.SetEnabled(false);
    }

    public void EnableMovement()
    {
        _characterController.SetEnabled(true);
    }

    private void Update()
    {
        if(_isControlling)
        {
            if (Input.GetKeyDown(_controlKey))
            {
                UnControlTree();
            }
        }
        else
        {
            ScanRadiusForControllable();

            if (_controllableTree)
                GameInitialization.instance.ui.ShowControllableUi();
            else
                GameInitialization.instance.ui.HideControllableUi();

            if (Input.GetKeyDown(_controlKey) && _isInTreeRange)
            {                
                ControlTree();
                GameInitialization.instance.ui.HideControllableUi();
            }
        }        
    }

    private void ScanRadiusForControllable()
    {
        var formerControllableTree = _controllableTree;
        _treesInRange.Clear();
        _controllableTree = null;
        var scanResults = new List<Controllable>();
        var colliders = Physics.OverlapSphere(transform.position, _treeSearchRadius);

        foreach (Collider collider in colliders)
        {
            GameObject obj = collider.gameObject;
            if (obj.tag == Tags.TreeTag)
            {
                Controllable controllabe = obj.GetComponent<Controllable>();
                if (controllabe)
                {
                    //Debug.Log(controllabe.name);
                    scanResults.Add(controllabe);
                }                    
            }
        }

        if (scanResults.Count == 0)
        {
            _isInTreeRange = false;
            _treesInRange.Clear();
            return;
        }

        _isInTreeRange = true;

        var newTreesInRange = _treesInRange.Except(scanResults).ToList();
        _treesInRange = scanResults;

        if (scanResults.Contains(formerControllableTree))
            _controllableTree = formerControllableTree;
        else if (newTreesInRange.Count > 0)
            _controllableTree = newTreesInRange[0];
        else
            _controllableTree = _treesInRange[0];
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    GameObject obj = other.gameObject;
    //    bool wasInRange = _isInTreeRange;

    //    if (obj.tag == Tags.TreeTag)
    //    {
    //        Controllable controllable = obj.GetComponent<Controllable>();
    //        if (controllable)
    //        {
    //            _treesInRange.Add(controllable);
    //            _controllableTree = controllable;
    //            _isInTreeRange = true;
    //        }
    //    }

    //    if (!wasInRange && _isInTreeRange)
    //        GameInitialization.instance.ui.ShowControllableUi();
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    GameObject obj = other.gameObject;
    //    bool wasInRange = _isInTreeRange;

    //    Controllable controllabe = obj.GetComponent<Controllable>();
    //    if (controllabe && _treesInRange.Contains(controllabe))
    //        _treesInRange.Remove(controllabe);

    //    if (_treesInRange.Count == 0)
    //        _isInTreeRange = false;

    //    if (!_isInTreeRange && wasInRange)
    //        GameInitialization.instance.ui.HideControllableUi();
    //}

    private void ControlTree()
    {
        Debug.Log("Controlling Tree");
        _isControlling = true;
        DisableMovement();
        _controllableTree.ControlTree();
        GameInitialization.instance.playerCamera.SetOrbitTarget(_controllableTree.controlPivot.gameObject);
    }

    private void UnControlTree()
    {
        Debug.Log("Un-Controlling Tree");
        _controllableTree.UnControlTree();
        EnableMovement();
        _isControlling = false;
        GameInitialization.instance.playerCamera.SetOrbitTarget(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _treeSearchRadius);
    }
}
