using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Equipment", menuName = "New Equipment")]
public class Equipment_Base : Thing {

    public ServiceLocator.EquipmentStates state;
    public ServiceLocator.ID holderOperator;
    public Equipment_Base stowingEqpt;

    public List<ServiceLocator.ThingType> stowable = new List<ServiceLocator.ThingType>();
    public List<Thing> stowed = new List<Thing>();
    public Command[] possibleCommands;

    public Equipment_Base()
    {
        cat = ServiceLocator.ThingCategory.Equipment;
        //Debug.Log("This " + type + " is a " + cat + ".");
    }
}
