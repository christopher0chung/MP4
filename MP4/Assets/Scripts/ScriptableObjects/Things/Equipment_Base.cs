using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Equipment", menuName = "New Equipment")]
public class Equipment_Base : Thing {

    public ServiceLocator.Interactives type;
    public ServiceLocator.EquipmentStates state;
    public ServiceLocator.ID holderOperator;
    public Equipment_Base stowingEqpt;
    public bool highlighted;

    public ServiceLocator.Interactives[] stowable;
    public List<Thing> stowed = new List<Thing>();
    public Command[] possibleCommands;
}
