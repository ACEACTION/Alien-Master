using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasBillboard : MonoBehaviour
{
    private Transform cam;

    private void Start()
    {
        cam = Camera.main.transform;
    }

    void LateUpdate()
    {
        // the health bar is not rotating
        // we want to always be able to clearly see our health bar
        // a billboard script created for this work
        transform.LookAt(transform.position + cam.transform.forward);
    }
}