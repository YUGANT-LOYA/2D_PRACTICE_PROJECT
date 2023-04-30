using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangerCharacterScript : CharacterScript
{
    private void Start()
    {
        InitCharacteristics();
        PrintInfo();
    }

    public override void InitCharacteristics()
    {
        health = 300;
        armour = 150;
        movementSpeed = 8f;
        Debug.Log("H : " + health + " A : " + armour + " S : " + movementSpeed);
    }

    void PrintInfo()
    {
        Debug.Log("H : " + health + " A : " + armour + " S : " + movementSpeed);
    }
}

