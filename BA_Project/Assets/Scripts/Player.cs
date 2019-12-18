using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllingTrees : MonoBehaviour
{
    public void DisablePlayer()
    {
        gameObject.SetActive(false);
    }

    public void EnablePlayer()
    {
        gameObject.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (GameInitialization.instance.level.IsPlayerInRange(transform.position, 4f))
            {
                //treeControl.ControllTree();
            }
        }
    }
}
