using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow_Arrow_Script : WeaponScript
{


    protected override void DefaultDamageInfo()
    {
        damage = 80;
        accuracy = 80f;
        weaponSpeed = 8;
        Debug.Log("D : " + damage + " A : " + accuracy + " S : " + weaponSpeed);
    }
}
