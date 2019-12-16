
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _treeSearchRadius;
    [SerializeField] private CharacterController _characterController;

    private Controllable _controllableTree;

    public void DisablePlayer()
    {
        _characterController.enabled = false;
        gameObject.SetActive(false);
    }

    public void EnablePlayer()
    {
        _characterController.enabled = true;
        gameObject.SetActive(true);
    }

    private void Update()
    {
        _controllableTree = null;
        Collider[] colliders = Physics.OverlapSphere(transform.position, _treeSearchRadius);

        foreach (Collider collider in colliders)
        {
            GameObject obj = collider.gameObject;

            if (obj.tag == "Tree")
            {
                Controllable controllable = obj.GetComponent<Controllable>();
                if (controllable != null)
                {
                    _controllableTree = controllable;
                }
            }
        }

        if (_controllableTree == null)
            return;

        // Show hint: control tree key

        if (Input.GetKeyDown(KeyCode.C))
        {
            ControllTree();
        }
    }

    private void ControllTree()
    {
        DisablePlayer();
        _controllableTree.ControllTree();
    }

    private void UnControllTree()
    {
        _controllableTree.UnControllTree();
        EnablePlayer();
    }
}
