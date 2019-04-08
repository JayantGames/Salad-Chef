using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectType : MonoBehaviour
{
    public enum TYPE
    {
        VEGETABLE,
        CUSTOMER_PLATE,
        CHOPPING_BOARD,
        EXTRA_PLATE,
        TRASHCAN
    }

    public TYPE objectType;
}
