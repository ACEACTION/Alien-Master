using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using JetBrains.Annotations;

public class EndDoor : MonoBehaviour
{
    public static EndDoor instance;


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        
    }


    public void MoveDoorUp()
    {
        transform.DOMoveY(5f, 1f);
    }
}
