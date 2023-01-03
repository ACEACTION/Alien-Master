using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

[CreateAssetMenu(menuName = "Data/UI/Weapon Item Data", fileName = "Weapon_Item_Data")]
public class Weapon_Item_ScObj : ScriptableObject
{
    public float scaleFactor;
    public float scaleDuration;
    public float scaleDelay;
    public TextMeshProUGUI cantBuyTxt;
    public Ease canBuyTextEaseType;
}
