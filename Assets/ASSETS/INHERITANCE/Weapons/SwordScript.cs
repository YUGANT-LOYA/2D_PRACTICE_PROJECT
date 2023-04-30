using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordScript : WeaponScript
{

    protected override void DefaultDamageInfo()
    {
        damage = 75;
        accuracy = 60f;
        weaponSpeed = 4;
        Debug.Log("D : " + damage + " A : " + accuracy + " S : " + weaponSpeed);
    }
}
