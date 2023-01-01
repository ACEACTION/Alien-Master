using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Weapon", fileName = "Item_Weapon_")]
public class WeaponItemScriptableObj : ScriptableObject
{
    public WeaponName weaponName;
    public Sprite weaponIcon;
    public int weaponPrice;
}
