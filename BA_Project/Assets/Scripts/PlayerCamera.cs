using Bolt;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    private const string OrbitTargetVariable = "OrbitTarget";

    public void SetOrbitTarget(GameObject target)
    {
        Variables.Object(gameObject).Set(OrbitTargetVariable, target);
    }
}
