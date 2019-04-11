using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectType : MonoBehaviour
{
    // State machine to check for the type of object to interact with.
    public enum TYPE
    {
        VEGETABLE,
        CUSTOMER_PLATE,
        CHOPPING_BOARD,
        EXTRA_PLATE,
        TRASHCAN,
        PICKUP
    }

    // State machine to check the pickup type.
    public enum PICKUP_TYPE
    {
        SPEED,
        TIME,
        SCORE
    }

    // Public states.
    public TYPE objectType;
    public PICKUP_TYPE objectPickupType;

    // Public reference variables.
    public ChefDetails chef;
    public string extraVegetable;
}
