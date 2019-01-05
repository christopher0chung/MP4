using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "New Item")]
public class Item_Base : Thing {

    public ServiceLocator.Interactives type;
    public ServiceLocator.ItemStates state;
    public ServiceLocator.ID holder;
    public Equipment_Base stowingEqpt;
    public bool highlighted;
    public float capacity;

}


