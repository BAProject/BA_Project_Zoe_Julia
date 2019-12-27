using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    private Transform cam;

    private void Awake()
    {
        cam = Camera.main.transform;
    }

    private void Update()
    {
        Vector3 targetPostition = new Vector3(cam.position.x, transform.position.y, cam.position.z);
        this.transform.LookAt(targetPostition);
    }
}
