using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Equipment", menuName = "New Equipment")]
public class Equipment_Base : Thing {

    public ServiceLocator.EquipmentStates state;
    public ServiceLocator.ID holderOperator;
    public Equipment_Base stowingEqpt;

    public List<ServiceLocator.Interactives> stowable = new List<ServiceLocator.Interactives>();
    public List<Thing> stowed = new List<Thing>();
    public Command[] possibleCommands;

    public Equipment_Base()
    {
        cat = ServiceLocator.InteractivesCategory.Equipment;
        //Debug.Log("This " + type + " is a " + cat + ".");
    }
}
