using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "New Item")]
public class Item_Base : Thing {

    public ServiceLocator.ItemStates state;
    //public ServiceLocator.ID holder;
    public Equipment_Base stowingEqpt;
    public float capacity;

    public Item_Base()
    {
        cat = ServiceLocator.InteractivesCategory.Items;
        //Debug.Log("This " + type + " is a " + cat + ".");
    }
}


