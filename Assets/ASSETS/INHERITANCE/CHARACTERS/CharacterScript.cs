using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterScript : MonoBehaviour
{
    public int health;
    public int armour;
    public float movementSpeed;


    public abstract void InitCharacteristics();
}
