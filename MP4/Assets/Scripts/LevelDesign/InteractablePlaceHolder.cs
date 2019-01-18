using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractablePlaceHolder : MonoBehaviour {

    public ServiceLocator.ThingType type;

    private Thing _myData;

    [Header("Item Override")]
    public ServiceLocator.ItemStates startingStateForItems;
    public float startingCapacity = 100;

    [Header("Equipment Override")]
    public ServiceLocator.EquipmentStates startingStateForEqpt;
    public List<ServiceLocator.ThingType> stowedOrInstalled;

    private ControllerObjectInteraction _objIntController;

    private List<Thing> _tempStowedOverrideContents;

    private void Start()
    {
        _objIntController = ServiceLocator.Instance.Controller.GetComponent<ControllerObjectInteraction>();

        _MakeData();
        if (_myData.cat == ServiceLocator.ThingCategory.Items)
        {
            (_myData as Item_Base).state = startingStateForItems;
            if( startingCapacity != 100)
                _ItemCapacityOverride();
        }
        if (_myData.cat == ServiceLocator.ThingCategory.Equipment)
        {
            (_myData as Equipment_Base).state = startingStateForEqpt;
            if(stowedOrInstalled.Count > 0)
                _EqptCheckStowOverride();
        }

        ServiceLocator.Instance.EManager.Fire(new Event_NewInteractable(_myData, transform.position, false));

        _RegisterAndStowOverrideContents();

        Destroy(this.gameObject);
    }

    private void _MakeData()
    {
        if (type == ServiceLocator.ThingType.ArTank ||
            type == ServiceLocator.ThingType.Battery ||
            type == ServiceLocator.ThingType.N2Tank ||
            type == ServiceLocator.ThingType.O2Tank)
        {
            _myData = ScriptableObject.Instantiate(Resources.Load<Item_Base>("SO_Item/" + type.ToString()));
        }
        else if (type == ServiceLocator.ThingType.ArCharger ||
            type == ServiceLocator.ThingType.BatteryCharger ||
            type == ServiceLocator.ThingType.N2Charger ||
            type == ServiceLocator.ThingType.O2Charger ||
            type == ServiceLocator.ThingType.Welder ||
            type == ServiceLocator.ThingType.Locker ||
            type == ServiceLocator.ThingType.Wrench ||
            type == ServiceLocator.ThingType.PryBar)
        {
            _myData = ScriptableObject.Instantiate(Resources.Load<Equipment_Base>("SO_Equipment/" + type.ToString()));
        }

        Debug.Log(_myData.type.ToString());
    }

    private void _ItemCapacityOverride()
    {
        Item_Base tempItem = _myData as Item_Base;
        Debug.Assert(tempItem != null, "Not item");

        tempItem.capacity = startingCapacity;
    }

    private void _EqptCheckStowOverride()
    {
        if (_myData.cat == ServiceLocator.ThingCategory.Items)
            return;
        else if (_myData.cat == ServiceLocator.ThingCategory.Equipment)
        {
            if (stowedOrInstalled.Count == 0)
                return;
            else
            {
                // Attempt to populate tempData's stowed list with override
                // Basic equipment can only potentially have items
                // Check each item in override to see if it's stowable
                // Then check if there already is one
                // Then make a new item to stow

                // *** STILL REQUIRES CONSUMABLES IMPLEMENTATION ***

                // *** MOVE STOWING FUNCTIONALITY TO MANAGER ***

                _tempStowedOverrideContents = new List<Thing>();

                foreach (ServiceLocator.ThingType i in stowedOrInstalled)
                {
                    Equipment_Base tempEqpt = _myData as Equipment_Base;
                    Debug.Assert(tempEqpt != null, "Not eqpt");


                    if (tempEqpt.stowable.Contains(i))
                    {
                        Thing thingToStore;

                        if (i == ServiceLocator.ThingType.ArTank ||
                            i == ServiceLocator.ThingType.Battery ||
                            i == ServiceLocator.ThingType.N2Tank ||
                            i == ServiceLocator.ThingType.O2Tank)
                        {
                            thingToStore = ScriptableObject.Instantiate(Resources.Load<Item_Base>("SO_Item/" + i.ToString()));
                        }
                        else if (i == ServiceLocator.ThingType.ArCharger ||
                            i == ServiceLocator.ThingType.BatteryCharger ||
                            i == ServiceLocator.ThingType.N2Charger ||
                            i == ServiceLocator.ThingType.O2Charger ||
                            i == ServiceLocator.ThingType.Welder ||
                            i == ServiceLocator.ThingType.Locker ||
                            i == ServiceLocator.ThingType.Wrench ||
                            i == ServiceLocator.ThingType.PryBar)
                        {
                            thingToStore = ScriptableObject.Instantiate(Resources.Load<Equipment_Base>("SO_Equipment/" + i.ToString()));
                        }
                        else
                            thingToStore = null;

                        Debug.Assert(thingToStore != null, "Attempting to store a non-Item non-Eqpt");

                        _tempStowedOverrideContents.Add(thingToStore);
                        //_tempStowedOverrideContents.Add(thingToStore);
                        //tempEqpt.stowed.Add(thingToStore);
                    }
                }
            }
        }
    }

    private void _RegisterAndStowOverrideContents()
    {
        if (_myData as Equipment_Base != null)
        {
            if (_tempStowedOverrideContents != null)
            {
                if (_tempStowedOverrideContents.Count > 0)
                {
                    foreach (Thing t in _tempStowedOverrideContents)
                    {
                        Item_Base i = t as Item_Base;
                        Equipment_Base e = t as Equipment_Base;

                        if (i != null)
                            i.state = ServiceLocator.ItemStates.Stowed;
                        else if (e != null)
                            e.state = ServiceLocator.EquipmentStates.Stowed;

                        ServiceLocator.Instance.EManager.Fire(new Event_NewInteractable(t, transform.position, true));
                        _objIntController.StowAndInstall(_myData as Equipment_Base, t);
                    }
                }
            }
        }
    }
}
