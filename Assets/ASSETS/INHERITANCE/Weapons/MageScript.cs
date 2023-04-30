using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageScript : WeaponScript
{
    protected override void DefaultDamageInfo()
    {
        damage = 60;
        accuracy = 80f;
        weaponSpeed = 6;
        Debug.Log("D : " + damage + " A : " + accuracy + " S : " + weaponSpeed);
    }
}
