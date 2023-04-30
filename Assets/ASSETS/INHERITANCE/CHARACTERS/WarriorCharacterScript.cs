using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorCharacterScript : CharacterScript
{

    private void Start()
    {
        InitCharacteristics();
        PrintInfo();
    }

    public override void InitCharacteristics()
    {
        health = 250;
        armour = 150;
        movementSpeed = 5f;
        Debug.Log("H : " + health + " A : " + armour + " S : " + movementSpeed);
    }

    void PrintInfo()
    {
        Debug.Log("H : " + health + " A : " + armour + " S : " + movementSpeed);
    }
}
