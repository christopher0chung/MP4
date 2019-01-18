using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewItem : ViewThing_Generic {

    private bool _set;
    private Item_Base _oIData;
    public Item_Base observedItemData
    {
        get
        {
            return _oIData;
        }
        private set
        {
            _oIData = value;

            GameObject visPrefab = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/" + value.type.ToString()),
                transform.position,
                transform.rotation,
                transform);

            _col = visPrefab.GetComponent<Collider>();
            _mr = visPrefab.GetComponent<MeshRenderer>();

            Debug.Assert(_col != null, "Prefab missing collider");
            Debug.Assert(_mr != null, "Prefab missing mesh renderer");

            if (_oIData.type == ServiceLocator.ThingType.ArTank)
            {
                //Init Ar Tank
                _rb.mass = 20;
                _trig.radius = .8f;
            }
            else if (_oIData.type == ServiceLocator.ThingType.Battery)
            {
                _rb.mass = 6;
                _trig.radius = .6f;
            }
            else if (_oIData.type == ServiceLocator.ThingType.N2Tank)
            {
                _rb.mass = 20;
                _trig.radius = .8f;
            }
            else if (_oIData.type == ServiceLocator.ThingType.O2Tank)
            {
                _rb.mass = 20;
                _trig.radius = .8f;
            }

            _currentState = _oIData.state;
            // Fallback in case the state is the default value
            _StateBasedComponentConfiguration(_oIData.state);
            _set = true;
        }
    }

    private Rigidbody _rb;
    private SphereCollider _trig;
    private Collider _col;
    private MeshRenderer _mr;

    private ServiceLocator.ItemStates _s;
    private ServiceLocator.ItemStates _currentState
    {
        get
        {
            return _s;
        }
        set
        {
            if (value != _s)
            {
                _s = value;

                _StateBasedComponentConfiguration(_s);
            }
        }
    }

    private ModelGame _gameModel;
    private ModelObjectInteraction _objIntModel;

    private Transform _interactiveAssetsTransform;

    public override void Awake()
    {
        priority = 3000;
        base.Awake();

        _gameModel = ServiceLocator.Instance.Model.GetComponent<ModelGame>();
        _objIntModel = ServiceLocator.Instance.Model.GetComponent<ModelObjectInteraction>();

        _interactiveAssetsTransform = ServiceLocator.Instance.View.Find("InteractiveAssets");
    }

    public override void S_PauseableUpdate()
    {
        if (!_set)
            return;

        _UpdateState();
        _ModifyPostion();
    }

    private void _StateBasedComponentConfiguration(ServiceLocator.ItemStates state)
    {
        Debug.Log("Shift in item state. " + gameObject.name + " is now " + state.ToString());
        if (state == ServiceLocator.ItemStates.Held)
        {
            Destroy(_rb);

            _trig.enabled = false;
            _mr.enabled = true;
            _col.enabled = true;

            if (observedItemData == _objIntModel.p0_InteractableGrabbed)
                transform.SetParent(ServiceLocator.Instance.Character0);
            else
                transform.SetParent(ServiceLocator.Instance.Character1);

        }
        else if (state == ServiceLocator.ItemStates.Loose)
        {
            if (_rb == null)
            {
                _rb = gameObject.AddComponent<Rigidbody>();
                serializedRigidBody.RestoreRigidbody(_rb);
            }

            _rb.isKinematic = false;
            _trig.enabled = true;
            _mr.enabled = true;
            _col.enabled = true;

            transform.SetParent(_interactiveAssetsTransform);
        }
        else if (state == ServiceLocator.ItemStates.Stowed)
        {
            if (_rb == null)
            {
                _rb = gameObject.AddComponent<Rigidbody>();
                serializedRigidBody.RestoreRigidbody(_rb);
            }

            _rb.isKinematic = true;
            _trig.enabled = false;
            _mr.enabled = false;
            _col.enabled = false;

            transform.SetParent(_interactiveAssetsTransform);
        }
        else if (state == ServiceLocator.ItemStates.Ejecting)
        {
            if (_rb == null)
            {
                _rb = gameObject.AddComponent<Rigidbody>();
                serializedRigidBody.RestoreRigidbody(_rb);
            }

            _rb.isKinematic = false;
            _trig.enabled = false;
            _mr.enabled = true;
            _col.enabled = false;

            transform.SetParent(_interactiveAssetsTransform);
        }
    }

    private void _UpdateState()
    {
        _currentState = observedItemData.state;
    }

    private void _ModifyPostion()
    {
        if (_currentState == ServiceLocator.ItemStates.Loose)
            return;
        else if (_currentState == ServiceLocator.ItemStates.Ejecting)
        {
            return;
        }
        else if (_currentState == ServiceLocator.ItemStates.Held)
        {
            Debug.Log(observedItemData.type.ToString());
            Debug.Log(_objIntModel.p0_InteractableGrabbed.ToString());
            if (obvservedDataGeneric == _objIntModel.p0_InteractableGrabbed)
            {
                transform.position = _gameModel.P0_Hands.position;
                transform.rotation = _gameModel.P0_Hands.rotation;
            }
            else
            {
                transform.position = _gameModel.P1_Hands.position;
                transform.rotation = _gameModel.P1_Hands.rotation;
            }
        }
        else if (_currentState == ServiceLocator.ItemStates.Stowed)
        {
            Debug.Assert(observedItemData.stowingEqpt != null, "No stowing eqpt set prior to state change");
            transform.position = _objIntModel.GetWhereIsThing(observedItemData.stowingEqpt);
        }
    }

    public void SetData(Item_Base item)
    {
        Debug.Assert(!_set, "Attempting to change item data reference for a set item.");

        _rb = gameObject.AddComponent<Rigidbody>();
        _rb.isKinematic = true;
        _rb.interpolation = RigidbodyInterpolation.Interpolate;
        _rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        _rb.constraints = RigidbodyConstraints.FreezePositionZ;
        _rb.drag = 1;
        _rb.angularDrag = 12;

        _trig = gameObject.AddComponent<SphereCollider>();
        _trig.isTrigger = true;

        obvservedDataGeneric = item;
        observedItemData = item;

        // done at the end to capture item type based properties assigned in "item" property assignment
        serializedRigidBody = new SCG_RigidBodySerialized(_rb);
    }
}
