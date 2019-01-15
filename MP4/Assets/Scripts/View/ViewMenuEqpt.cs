using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewMenuEqpt : MP4_ScheduledMono {

    private ModelGame _gameModel;
    private ModelMenuEqpt _eqptMenuModel;

    private Transform _MenuParent_p0;
    private Transform _MenuParent_p1;

    private Transform _option1_p0;
    private Transform _option1_p1;

    private ServiceLocator.ControlStates _0;
    private ServiceLocator.ControlStates _p0CurrentState
    {
        get
        {
            return _0;
        }
        set
        {
            if (_0 != value)
            {
                _0 = value;
                if (_0 == ServiceLocator.ControlStates.Menu_Eqpt)
                {
                    _timer_p0 = 0;
                    _menuOptionsText_p0 = new List<string>();
                    _PopulateMenu(ServiceLocator.ID.p0);
                }
            }
        }
    }
    private float _timer_p0;

    private ServiceLocator.ControlStates _1;
    private ServiceLocator.ControlStates _p1CurrentState
    {
        get
        {
            return _1;
        }
        set
        {
            if (_1 != value)
            {
                _1 = value;
                if (_1 == ServiceLocator.ControlStates.Menu_Eqpt)
                {
                    _timer_p1 = 0;
                    _menuOptionsText_p1 = new List<string>();
                    _PopulateMenu(ServiceLocator.ID.p1);
                }
            }
        }
    }
    private float _timer_p1;

    private Vector3 _p0Option1Anchor = new Vector3(-810, 200, 0);
    private Vector3 _p1Option1Anchor = new Vector3(-100, 200, 0);

    private List<string> _menuOptionsText_p0;
    private List<string> _menuOptionsText_p1;

    public override void Awake()
    {
        priority = 3000;
        base.Awake();

        _gameModel = ServiceLocator.Instance.Model.GetComponent<ModelGame>();
        _eqptMenuModel = ServiceLocator.Instance.Model.GetComponent<ModelMenuEqpt>();

        _MenuParent_p0 = ServiceLocator.Instance.View.Find("Canvas").Find("P0 Menu Parent");
        _MenuParent_p1 = ServiceLocator.Instance.View.Find("Canvas").Find("P1 Menu Parent");

        _option1_p0 = _MenuParent_p0.Find("P0 Option 1");
        _option1_p1 = _MenuParent_p1.Find("P1 Option 1");
    }

    public override void S_Update()
    {
        _p0CurrentState = _gameModel.CtrlState_P0;
        _p1CurrentState = _gameModel.CtrlState_P1;
    }

    private void _PopulateMenu(ServiceLocator.ID id)
    {
        if (id == ServiceLocator.ID.p0)
        {
            foreach (Command c in _eqptMenuModel.p0EqptCommandsActive.Keys)
            {
                _menuOptionsText_p0.Add(c.action.ToString() + " " + c.target.ToString());
            }
        }
        else
        {
            foreach (Command c in _eqptMenuModel.p1EqptCommandsActive.Keys)
            {
                _menuOptionsText_p1.Add(c.action.ToString() + " " + c.target.ToString());
            }
        }
    }
}
