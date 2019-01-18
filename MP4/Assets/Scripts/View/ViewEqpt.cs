using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewEqpt : ViewThing_Generic {

    private bool _set;
    private Equipment_Base _oEData;
    public Equipment_Base observedEqptData
    {
        get
        {
            return _oEData;
        }
        private set
        {
            _oEData = value;

            GameObject visPrefab = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/" + value.type.ToString()),
                transform.position,
                transform.rotation,
                transform);

            _col = visPrefab.GetComponent<Collider>();
            _mr = visPrefab.GetComponent<MeshRenderer>();

            Debug.Assert(_col != null, "Prefab missing collider");
            Debug.Assert(_mr != null, "Prefab missing mesh renderer");

            if (_oEData.type == ServiceLocator.ThingType.Welder)
            {
                _rb.mass = 20;
                _trig.radius = .8f;
            }
            else if (_oEData.type == ServiceLocator.ThingType.O2Charger)
            {
                _rb.mass = 20;
                _trig.radius = .8f;
            }
            else if (_oEData.type == ServiceLocator.ThingType.ArCharger)
            {
                _rb.mass = 20;
                _trig.radius = .8f;
            }
            else if (_oEData.type == ServiceLocator.ThingType.BatteryCharger)
            {
                _rb.mass = 20;
                _trig.radius = .8f;
            }
            else if (_oEData.type == ServiceLocator.ThingType.N2Charger)
            {
                _rb.mass = 20;
                _trig.radius = .8f;
            }
            else if (_oEData.type == ServiceLocator.ThingType.O2Charger)
            {
                _rb.mass = 20;
                _trig.radius = .8f;
            }
            else if (_oEData.type == ServiceLocator.ThingType.Locker)
            {
                _rb.mass = 20;
                _trig.radius = .8f;
            }
            else if (_oEData.type == ServiceLocator.ThingType.Wrench)
            {
                _rb.mass = 20;
                _trig.radius = .8f;
            }
            else if (_oEData.type == ServiceLocator.ThingType.PryBar)
            {
                _rb.mass = 20;
                _trig.radius = .8f;
            }

            _set = true;
            _StateBasedComponentConfiguration(_oEData.state);
        }
    }

    private Rigidbody _rb;
    private SphereCollider _trig;
    private Collider _col;
    private MeshRenderer _mr;

    private ServiceLocator.EquipmentStates _s;
    private ServiceLocator.EquipmentStates _currentState
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

    private void _StateBasedComponentConfiguration(ServiceLocator.EquipmentStates state)
    {
        if (state == ServiceLocator.EquipmentStates.Fixed)
        {
            if (_rb != null)
                Destroy(_rb);

            _trig.enabled = true;
            _mr.enabled = true;
            _col.enabled = false;

            transform.SetParent(_interactiveAssetsTransform);
        }
        else if (state == ServiceLocator.EquipmentStates.Held)
        {
            if (_rb != null)
                Destroy(_rb);

            _trig.enabled = false;
            _mr.enabled = true;
            _col.enabled = true;

            if (observedEqptData == _objIntModel.p0_InteractableGrabbed)
                transform.SetParent(ServiceLocator.Instance.Character0);
            else
                transform.SetParent(ServiceLocator.Instance.Character1);
        }
        else if (state == ServiceLocator.EquipmentStates.Loose)
        {
            if(_rb == null)
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
        else if (state == ServiceLocator.EquipmentStates.Operating)
        {
            if (_rb == null)
            {
                _rb = gameObject.AddComponent<Rigidbody>();
                serializedRigidBody.RestoreRigidbody(_rb);
            }

            _rb.isKinematic = true;
            _trig.enabled = true;
            _mr.enabled = true;
            _col.enabled = true;

            transform.SetParent(_interactiveAssetsTransform);
        }
        else if (state == ServiceLocator.EquipmentStates.Stowed)
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
    }

    private void _UpdateState()
    {
        _currentState = observedEqptData.state;
    }

    private void _ModifyPostion()
    {
        if (_currentState == ServiceLocator.EquipmentStates.Loose)
            return;
        else if (_currentState == ServiceLocator.EquipmentStates.Operating)
            return;
        else if (_currentState == ServiceLocator.EquipmentStates.Fixed)
            return;
        else if (_currentState == ServiceLocator.EquipmentStates.Held)
        {
            //Debug.Log(observedEqptData.type.ToString());
            //Debug.Log(_objIntModel.p0_InteractableGrabbed.ToString());
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
        else if (_currentState == ServiceLocator.EquipmentStates.Stowed)
        {
            Debug.Assert(observedEqptData.stowingEqpt != null, "No stowing eqpt set prior to state change");
            transform.position = _objIntModel.GetWhereIsThing(observedEqptData.stowingEqpt);
        }
    }

    public void SetData(Equipment_Base eqpt)
    {
        Debug.Assert(!_set, "Attempting to change item data reference for a set item.");

        _rb = gameObject.AddComponent<Rigidbody>();
        _rb.isKinematic = false;
        _rb.interpolation = RigidbodyInterpolation.Interpolate;
        _rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        _rb.constraints = RigidbodyConstraints.FreezePositionZ;
        _rb.drag = 1;
        _rb.angularDrag = 12;

        _trig = gameObject.AddComponent<SphereCollider>();
        _trig.isTrigger = true;

        obvservedDataGeneric = eqpt;
        observedEqptData = eqpt;

        // done at the end to capture item type based properties assigned in "item" property assignment
        serializedRigidBody = new SCG_RigidBodySerialized(_rb);
    }
}
