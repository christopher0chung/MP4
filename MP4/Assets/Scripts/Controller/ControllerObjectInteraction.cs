using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerObjectInteraction : MP4_ScheduledMono {

    public Vector3 _tuning_PlayerTransformToHeadOffset;
    public float _tuning_InteractionRange;
    public float _tuning_OOCRange;

    [Header("Do NOT Modify")]
    public LayerMask selectInteractables;

    private ModelGame _gameModel;
    private ModelObjectInteraction _objIntModel;

    private Transform _p0;
    private Transform _p1;


    public override void Awake()
    {
        priority = 2400;
        base.Awake();
        _objIntModel = ServiceLocator.Instance.Model.GetComponent<ModelObjectInteraction>();
        _objIntModel.RegisteredInteractableObject = new Dictionary<Thing, GameObject>();
        ServiceLocator.Instance.EManager.Register<Event_NewInteractable>(NewInteractableHandler);
    }

    private void Start()
    {
        _gameModel = ServiceLocator.Instance.Model.GetComponent<ModelGame>();

        _p0 = ServiceLocator.Instance.Character0;
        _p1 = ServiceLocator.Instance.Character1;
    }

    public override void S_Update()
    {
        if (_gameModel.GameState == ServiceLocator.GameStates.Play)
        {
            if (_gameModel.CtrlState_P0 == ServiceLocator.ControlStates.Free)
            {
                if (_objIntModel.p0_InteractableGrabbed == null)
                {
                    _CheckPlayerInteractable(ServiceLocator.ID.p0);
                    _PassToModelInteractables(ServiceLocator.ID.p0);
                    _PassToModelOOCs(ServiceLocator.ID.p0);
                }
            }
            if (_gameModel.CtrlState_P1 == ServiceLocator.ControlStates.Free)
            {
                if (_objIntModel.p0_InteractableGrabbed == null)
                {
                    _CheckPlayerInteractable(ServiceLocator.ID.p1);
                    _PassToModelInteractables(ServiceLocator.ID.p1);
                    _PassToModelOOCs(ServiceLocator.ID.p1);
                }
            }
        }
    }

    Ray p0Look;
    Ray p1Look;
    RaycastHit p0Cast;
    RaycastHit p1Cast;

    private void _CheckPlayerInteractable(ServiceLocator.ID id)
    {
        p0Look.origin = _p0.position + _tuning_PlayerTransformToHeadOffset;
        p0Look.direction = _gameModel.P0_LookDir;

        Debug.DrawLine(p0Look.origin, p0Look.origin + (p0Look.direction * _tuning_InteractionRange));

        Physics.SphereCast(p0Look, .25f, out p0Cast, _tuning_InteractionRange, selectInteractables, QueryTriggerInteraction.Collide);
    }

    private void _PassToModelInteractables(ServiceLocator.ID id)
    {
        if (p0Cast.collider != null)
            _objIntModel.SetInteractableInterested(ServiceLocator.ID.p0, p0Cast.transform.GetComponent<ViewThing_Generic>().obvservedDataGeneric);
        else
            _objIntModel.SetInteractableInterested(ServiceLocator.ID.p0, null);

        if (p1Cast.collider != null)
            _objIntModel.SetInteractableInterested(ServiceLocator.ID.p1, p1Cast.transform.GetComponent<ViewThing_Generic>().obvservedDataGeneric);
        else
            _objIntModel.SetInteractableInterested(ServiceLocator.ID.p1, null);
    }

    Collider[] scratchColliderArray = new Collider[0];
    Thing[] scratchThingArray = new Thing[0];

    private void _PassToModelOOCs(ServiceLocator.ID id)
    {
        if (_objIntModel.p0_InteractableInterested != null)
        {
            scratchColliderArray = Physics.OverlapSphere(_objIntModel.GetWhereIsThing(_objIntModel.p0_InteractableInterested), _tuning_OOCRange, selectInteractables);
            if (scratchColliderArray.Length != 0)
            {
                scratchThingArray = new Thing[scratchColliderArray.Length];
                for (int i = 0; i < scratchColliderArray.Length; i++)
                {
                    scratchThingArray[i] = scratchColliderArray[i].gameObject.GetComponent<ViewThing_Generic>().obvservedDataGeneric;
                }
            }
            else
                scratchThingArray = new Thing[0];

        }
        else
            scratchThingArray = new Thing[0];
        _objIntModel.SetOOCs(ServiceLocator.ID.p0, scratchThingArray);

        //-------------- 
        // Mirror for p1
        //--------------

        if (_objIntModel.p1_InteractableInterested != null)
        {
            scratchColliderArray = Physics.OverlapSphere(_objIntModel.GetWhereIsThing(_objIntModel.p1_InteractableInterested), _tuning_OOCRange);
            if (scratchColliderArray.Length != 0)
            {
                scratchThingArray = new Thing[scratchColliderArray.Length];
                for (int i = 0; i < scratchColliderArray.Length; i++)
                {
                    scratchThingArray[i] = scratchColliderArray[i].gameObject.GetComponent<ViewThing_Generic>().obvservedDataGeneric;
                }
            }
            else
                scratchThingArray = new Thing[0];

        }
        else
            scratchThingArray = new Thing[0];
        _objIntModel.SetOOCs(ServiceLocator.ID.p1, scratchThingArray);
    }

    private void _Helper_AutoShiftBodyLayer(Thing t)
    {
        GameObject body = _objIntModel.GetBodyOfThing(t);
        if (body != null)
        {
            if (t.cat == ServiceLocator.ThingCategory.Items)
            {
                Item_Base i = t as Item_Base;
                Debug.Assert(i != null, "Item and category mismatch");

                if (i.state == ServiceLocator.ItemStates.Stowed)
                {
                    body.layer = 10;
                    //Debug.Log(body.layer);
                }
                else
                {
                    body.layer = 9;
                    //Debug.Log(body.layer);
                }
            }
            else if (t.cat == ServiceLocator.ThingCategory.Equipment)
            {
                Equipment_Base e = t as Equipment_Base;
                Debug.Assert(e != null, "Equipment and category mismatch");

                if (e.state == ServiceLocator.EquipmentStates.Stowed)
                {
                    body.layer = 10;
                    //Debug.Log(body.layer);
                }
                else
                {
                    body.layer = 9;
                    //Debug.Log(body.layer);
                }
            }
        }
    }

    private void _Helper_SetBodyLayerUsingState(GameObject body, Thing data)
    {
        int layerToSet;

        Item_Base i = data as Item_Base;
        Equipment_Base e = data as Equipment_Base;
        if (i != null)
        {
            if (i.state == ServiceLocator.ItemStates.Stowed)
                layerToSet = 10;
            else
                layerToSet = 9;
        }
        else if (e != null)
        {
            if (e.state == ServiceLocator.EquipmentStates.Stowed)
                layerToSet = 10;
            else
                layerToSet = 9;
        }
        else
            layerToSet = 9;

        Debug.Log(body.name + " " + layerToSet);

        body.layer = layerToSet;
    }

    #region Asynchronous External Functions
    public void AttemptToHold(ServiceLocator.ID id)
    {
        if (id == ServiceLocator.ID.p0)
        {       
            if (_objIntModel.p0_InteractableInterested.cat == ServiceLocator.ThingCategory.Items)
            {
                Item_Base i = _objIntModel.p0_InteractableInterested as Item_Base;
                if (i != null)
                {
                    i.state = ServiceLocator.ItemStates.Held;
                    _objIntModel.SetGrabbed(id, _objIntModel.p0_InteractableInterested);
                    _objIntModel.SetInteractableInterested(id, null);
                    Debug.Log("Grabbed is of type " + _objIntModel.p0_InteractableGrabbed + ". It's enum type is " + _objIntModel.p0_InteractableGrabbed.type.ToString() + ".");

                }
            }
            else if (_objIntModel.p0_InteractableInterested.cat == ServiceLocator.ThingCategory.Equipment)
            {
                Equipment_Base e = _objIntModel.p0_InteractableInterested as Equipment_Base;
                if (e != null)
                {
                    if (e.state == ServiceLocator.EquipmentStates.Loose)
                    {
                        e.state = ServiceLocator.EquipmentStates.Held;
                        _objIntModel.SetGrabbed(id, _objIntModel.p0_InteractableInterested);
                        _objIntModel.SetInteractableInterested(ServiceLocator.ID.p0, null);
                    }
                }
            }
        }
    }

    public void Unhold(ServiceLocator.ID id)
    {
        if (id == ServiceLocator.ID.p0)
        {
            if (_objIntModel.p0_InteractableGrabbed.cat == ServiceLocator.ThingCategory.Items)
            {
                Item_Base i = _objIntModel.p0_InteractableGrabbed as Item_Base;
                if (i != null)
                {
                    i.state = ServiceLocator.ItemStates.Loose;
                    _objIntModel.SetDrop(ServiceLocator.ID.p0);
                }
            }
            else if (_objIntModel.p0_InteractableGrabbed.cat == ServiceLocator.ThingCategory.Equipment)
            {
                Equipment_Base e = _objIntModel.p0_InteractableGrabbed as Equipment_Base;
                if (e != null)
                {
                    e.state = ServiceLocator.EquipmentStates.Loose;
                    _objIntModel.SetDrop(ServiceLocator.ID.p0);
                }
            }
        }
    }

    public void StowAndInstall(Equipment_Base eqptThatWillPossess, Thing thingToStore)
    {
        eqptThatWillPossess.stowed.Add(thingToStore);
        if (thingToStore.cat == ServiceLocator.ThingCategory.Items)
        {
            Item_Base i = thingToStore as Item_Base;
            Debug.Assert(i != null, "Item type and category mismatch");

            i.stowingEqpt = eqptThatWillPossess;
            i.state = ServiceLocator.ItemStates.Stowed;

            _Helper_AutoShiftBodyLayer(i);
        }
        else if(thingToStore.cat == ServiceLocator.ThingCategory.Equipment)
        {
            Equipment_Base e = thingToStore as Equipment_Base;
            Debug.Assert(e != null, "Equipment type and category mismatch");

            e.stowingEqpt = eqptThatWillPossess;
            e.state = ServiceLocator.EquipmentStates.Stowed;

            _Helper_AutoShiftBodyLayer(e);
        }
        else
        {
            // *** Still requires consumable implementation ***
            Debug.Log("Attempting to stow invalid InteractiveCategory thing");
        }
    }

    public void Unstow(Equipment_Base eqptUnstower, Thing thingUnstowee)
    {
        if (!eqptUnstower.stowed.Contains(thingUnstowee))
            Debug.Log("Attempting to unstow an item that is not stowed in this eqpt");

        eqptUnstower.stowed.Remove(thingUnstowee);
        if (thingUnstowee.cat == ServiceLocator.ThingCategory.Items)
        {
            Item_Base i = thingUnstowee as Item_Base;
            Debug.Assert(i != null, "Item type and category mismatch");

            i.stowingEqpt = null;
            i.state = ServiceLocator.ItemStates.Loose;

            _Helper_AutoShiftBodyLayer(i);
            _objIntModel.GetBodyOfThing(i).transform.position += Vector3.right;
        }
        else if (thingUnstowee.cat == ServiceLocator.ThingCategory.Equipment)
        {
            Equipment_Base e = thingUnstowee as Equipment_Base;
            Debug.Assert(e != null, "Equipment type and category mismatch");

            e.stowingEqpt = null;
            e.state = ServiceLocator.EquipmentStates.Loose;
            _objIntModel.GetBodyOfThing(e).transform.position += Vector3.right;

            _Helper_AutoShiftBodyLayer(e);
        }
        else
        {
            // *** Still requires consumable implementation ***
            Debug.Log("Attempting to stow invalid InteractiveCategory thing");
        }

        Vector3 eqptPos = _objIntModel.GetWhereIsThing(eqptUnstower);

        _objIntModel.GetBodyOfThing(thingUnstowee).transform.position = eqptPos + Vector3.right;

    }

    public void Eject(Equipment_Base eqptEjector, Thing thingEjectee)
    {
        if (!eqptEjector.stowed.Contains(thingEjectee))
            Debug.Log("Attempting to eject an item that is not stowed in this eqpt");

        eqptEjector.stowed.Remove(thingEjectee);
        if (thingEjectee.cat == ServiceLocator.ThingCategory.Items)
        {
            Item_Base i = thingEjectee as Item_Base;
            Debug.Assert(i != null, "Item type and category mismatch");

            i.stowingEqpt = null;
            i.state = ServiceLocator.ItemStates.Loose;

            _Helper_AutoShiftBodyLayer(i);
            _objIntModel.GetBodyOfThing(i).transform.position += Vector3.right;
        }
        else
        {
            Debug.Log("Attempting to eject invalid InteractiveCategory thing");
        }
    }

    #endregion

    #region Event Handler
    private void NewInteractableHandler(MP4_Event e)
    {
        Event_NewInteractable n = e as Event_NewInteractable;
        if (n != null)
        {
            Debug.Assert(!_objIntModel.RegisteredInteractableObject.ContainsKey(n.data), "Attempting to register an interactible that is already registered");

            GameObject body = new GameObject(n.data.type.ToString());

            Debug.Log(n.startStowedOrInstalled);

            _Helper_SetBodyLayerUsingState(body, n.data);

            body.transform.SetParent(ServiceLocator.Instance.View.Find("InteractiveAssets"));
            body.transform.position = n.pos;

            if (n.data.cat == ServiceLocator.ThingCategory.Items)
            {
                ViewItem itemView = body.AddComponent<ViewItem>();
                itemView.SetData(n.data as Item_Base);
            }

            else if (n.data.cat == ServiceLocator.ThingCategory.Equipment)
            {
                ViewEqpt eqptView = body.AddComponent<ViewEqpt>();
                eqptView.SetData(n.data as Equipment_Base);
            }

            _objIntModel.RegisteredInteractableObject.Add(n.data, body);
        }
    }
    #endregion
}
