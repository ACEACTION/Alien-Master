using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PowerDoor : MonoBehaviour
{
    [SerializeField] Vector3 openDoorMove;

    [SerializeField] Material activeMat;
    [SerializeField] MeshRenderer[] piplines;
    [SerializeField] Transform DoorMesh;

    void OpenDoor()
    {
        DoorMesh.DOMove(openDoorMove, 1f);
    }

    public void ActiveDoor()
    {
        foreach (MeshRenderer p in piplines)
        {
            p.material = activeMat;
        }


        OpenDoor();

    }



}
