using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponScript : MonoBehaviour
{
    public int damage;
    public float accuracy;
    [Range(0,10)]
    public int weaponSpeed;

    protected abstract void DefaultDamageInfo();

    protected virtual void GetDamage()
    {
        
    }

}
