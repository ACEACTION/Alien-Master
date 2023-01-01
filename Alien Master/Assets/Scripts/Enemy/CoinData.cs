using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("Data/Coin data"))]
public class CoinData : ScriptableObject
{
    public int cashAmount;
    public float magnetSpeed;
    public float jumpPower;
    public float magnetSpeedAmount;
    public float rotateSpeed;
    public float zAxisForce;
    public float xAxisForce;
    public bool staticCoin;
    public float timeToFollow;
    public float timeToFollowAmount;
}
