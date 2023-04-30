using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardCharacterScript : CharacterScript
{

    private void Start()
    {
        InitCharacteristics();
        PrintInfo();
    }

    public override void InitCharacteristics()
    {
        health = 350;
        armour = 150;
        movementSpeed = 10f;
        Debug.Log("H : " + health + " A : " + armour + " S : " + movementSpeed);
    }

    void PrintInfo()
    {
        Debug.Log("H : " + health + " A : " + armour + " S : " + movementSpeed);
    }
}
