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
        Welder, O2Charger, ArCharger, BatteryCharger, N2Charger, Locker,
        //Modifiable
        Hull, Container, Accumulator, Pump, Motor, Gear, Valve, Linkage, Pipe, Breaker, Obstruction
    }

    public enum ItemStates { Loose, Held, Stowed }

    public enum EquipmentStates { Loose, Held, Stowed, Fixed, Operating }

    public enum Actions { Weld, Patch, Power, Charge, Energize, Shut, Repair, Install, Eject, Stow, Unstow }
    public enum ActionType { Discrete, Continuous }
    //public enum Condition { Leaking, Ruptured, Underpowered, Unpowered, Tripped, MechanicallyFailed, CatastrophiclyFailed, Discharged, Required, Patched, This }
    #endregion
}
