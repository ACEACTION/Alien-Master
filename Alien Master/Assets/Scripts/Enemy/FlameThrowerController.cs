using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class FlameThrowerController : MonoBehaviour
{
    [SerializeField] Transform destination;
    [SerializeField] float transmitionTime;

    private void Start()
    {
        transform.DOMove(destination.position, transmitionTime).SetLoops(-1,LoopType.Yoyo);
    }
}
