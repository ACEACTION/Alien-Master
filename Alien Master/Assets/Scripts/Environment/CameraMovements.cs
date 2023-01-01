using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovements : MonoBehaviour
{
    [SerializeField] GameObject _player;

    [Range(0.01f, 1f)]
    public float smoothFactor = 5.5f;

    public Vector3 offset;
    Vector3 velocity = Vector3.zero;
    Vector3 lastPlayerPosition;
    public bool lookAtPlayer = false;
    private void LateUpdate()
    {
        if (_player == null) return;

        Vector3 newPos = _player.transform.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, newPos, ref velocity, smoothFactor);
        if (lookAtPlayer)
            transform.LookAt(_player.transform);
    }
}
