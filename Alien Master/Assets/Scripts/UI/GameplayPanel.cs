using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayPanel : MonoBehaviour
{
    private void OnEnable()
    {
        PlayerMovement.Instance.canMove = true;
    }

    private void OnDisable()
    {
        PlayerMovement.Instance.canMove = false;
    }

}
