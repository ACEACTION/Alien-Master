using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerDoorActivator : MonoBehaviour
{
    [SerializeField] PowerDoor powerDoor;
    private void Start()
    {
        
            transform.DOMoveY(transform.position.y + 1f, 0.5f).SetLoops(-1, LoopType.Yoyo);
        
    }
    private void Update()
    {
        
            transform.Rotate(0, 90f * Time.deltaTime, 90f * Time.deltaTime);
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            powerDoor.ActiveDoor();
        }
    }
}
