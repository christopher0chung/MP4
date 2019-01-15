using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServiceLocator {

    private static ServiceLocator _i;
    public static ServiceLocator Instance
    {
        get
        {
            if (_i == null)
                _i = new ServiceLocator();

            return _i;
        }
    }

    private MP4_EventManager _e;
    public MP4_EventManager EManager
    {
        get
        {
            if (_e == null)
                _e = new MP4_EventManager();
            return _e;
        }
    }

    private Transform _a;
    public Transform Application
    {
        get
        {
            if (_a == null)
                _a = GameObject.Find("Application").transform;
            return _a;
        }
    }

    private Transform _m;
    public Transform Model
    {
        get
        {
            if (_m == null)
                _m = GameObject.Find("Model").transform;
            return _m;
        }
    }

    private Transform _v;
    public Transform View
    {
        get
        {
            if (_v == null)
                _v = GameObject.Find("View").transform;
            return _v;
        }
    }

    private Transform _c;
    public Transform Controller
    {
        get
        {
            if (_c == null)
                _c = GameObject.Find("Controller").transform;
            return _c;
        }
    }

    private Transform _0;
    public Transform Character0
    {
        get
        {
            if (_0 == null)
                _0 = View.Find("P0");
            return _0;
        }
    }

    private Transform _1;
    public Transform Character1
    {
        get
        {
            if (_1 == null)
                _1 = View.Find("P1");
            return _1;
        }
    }

    #region Functions

    public bool DoesListOfThingsContainThingOfTypeItem(List<Thing> list, Interactives type)
    {
        foreach(Thing t in list)
        {
            Item_Base itemCheck = t as Item_Base;
            if (itemCheck != null)
                if (itemCheck.type == type)
                    return true;
        }
        return false;
    }
    
    public bool DoesListOfThingsContainThingOfTypeEquipment(List<Thing> list, Interactives type)
    {
        foreach (Thing t in list)
        {
            Equipment_Base eqptCheck = t as Equipment_Base;
            if (eqptCheck != null)
                if (eqptCheck.type == type)
                    return true;
        }
        return false;
    }
        #endregion

    #region Enums
    public enum ID { p0, p1 }
    public enum ControlStates { Free, Menu_Eqpt, Station, Occupied, Disabled}
    public enum GameStates { Menu, Cutscene, Play }

    public enum Inputs { Up, Down, Left, Right, Grab, Use }
    public enum InputStates { OnDown, Down, OnUp }

    public enum Interactives {
        //Items
        O2Tank, ArTank, Battery, N2Tank,
        //Consumables
        SmallPatchPlate, LargePatchPlate, Fuses,
        //Equipment
        Welder, O2Charger, ArCharger, BatteryCharger, N2Charger, Locker, Wrench, PryBar,
        //Modifiables
        Rupture, Leak, PatchedRupture, PatchedLeak, RepairedRupture, RepairedLeak, DepressurizedAccumulator, PressurizedAccumulator,
        InoperablePump, OperatingPump, InoperableMotor, OperatingMotor, JammedGear, SpinningGear,
        FailedShutValve, FailedOpenValve, ShutValve, OpenValve, TrippedBreaker, ResetBreaker, Obstruction, ClearedDebris,
        //Stations
        Stn_Helm, Stn_LeeHelm, Stn_Crane,
        //None
        None
    }

    public enum InteractivesCategory { Items, Equipment, Stations, Consumables, Modifiables }

    public enum ItemStates { Loose, Held, Stowed, Ejecting }

    public enum EquipmentStates { Loose, Held, Stowed, Fixed, Operating }

    public enum Actions { Weld, Patch, Power, Charge, Energize, Shut, Repair, Install, Eject, Stow, Unstow, Cancel }
    public enum ActionType { Discrete, Continuous }
    //public enum Condition { Leaking, Ruptured, Underpowered, Unpowered, Tripped, MechanicallyFailed, CatastrophiclyFailed, Discharged, Required, Patched, This }
    #endregion
}
