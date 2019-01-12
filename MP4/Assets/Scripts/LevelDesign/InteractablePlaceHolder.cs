using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractablePlaceHolder : MonoBehaviour {

    public ServiceLocator.Interactives type;

    private Thing _myData;

    [Header("Item Override")]
    public float startingCapacity = 100;

    [Header("Equipment Override")]
    public List<ServiceLocator.Interactives> stowedOrInstalled;

    private void Start()
    {
        _MakeData();
        if (_myData.cat == ServiceLocator.InteractivesCategory.Items && startingCapacity != 100)
            _ItemCapacityOverride();
        if (_myData.cat == ServiceLocator.InteractivesCategory.Equipment && stowedOrInstalled.Count > 0)
            _EqptCheckStowOverride();

        ServiceLocator.Instance.EManager.Fire(new Event_NewInteractable(_myData, transform.position));

        Destroy(this.gameObject);
    }

    private void _MakeData()
    {
        if (type == ServiceLocator.Interactives.ArTank ||
            type == ServiceLocator.Interactives.Battery ||
            type == ServiceLocator.Interactives.N2Tank ||
            type == ServiceLocator.Interactives.O2Tank)
        {
            _myData = ScriptableObject.Instantiate(Resources.Load<Item_Base>("SO_Item/" + type.ToString()));
        }
        else if (type == ServiceLocator.Interactives.ArCharger ||
            type == ServiceLocator.Interactives.BatteryCharger ||
            type == ServiceLocator.Interactives.N2Charger ||
            type == ServiceLocator.Interactives.O2Charger ||
            type == ServiceLocator.Interactives.Welder)
        {
            _myData = ScriptableObject.Instantiate(Resources.Load<Equipment_Base>("SO_Equipment/" + type.ToString()));
        }
    }

    private void _ItemCapacityOverride()
    {
        Item_Base tempItem = _myData as Item_Base;
        Debug.Assert(tempItem != null, "Not item");

        tempItem.capacity = startingCapacity;
    }

    private void _EqptCheckStowOverride()
    {            
        // Attempt to populate tempData's stowed list with override
        // Basic equipment can only potentially have items
        // Check each item in override to see if it's stowable
        // Then check if there already is one
        // Then make a new item to stow
        foreach (ServiceLocator.Interactives i in stowedOrInstalled)
        {
            Equipment_Base tempEqpt = _myData as Equipment_Base;
            Debug.Assert(tempEqpt != null, "Not eqpt");

            if (tempEqpt.stowable.Contains(i))
            {
                if (!ServiceLocator.Instance.DoesListOfThingsContainThingOfTypeItem(tempEqpt.stowed, i))
                {
                    //Create new thing
                    //Get thing's data
                    //Pass it 
                }
            }
        }
    }

}
