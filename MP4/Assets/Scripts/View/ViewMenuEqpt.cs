using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ViewMenuEqpt : MP4_ScheduledMono {

    private ModelGame _gameModel;
    private ModelMenuEqpt _eqptMenuModel;

    private Transform _menuParent_p0;
    private Transform _menuParent_p1;

    private Transform _title_p0;
    private Transform _title_p1;

    private Transform _option1_p0;
    private Transform _option1_p1;

    private Transform _cursor_p0;
    private Transform _cursor_p1;

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

                    if (_menuOptionsTransforms_p0 != null)
                        foreach (Transform t in _menuOptionsTransforms_p0)
                            Destroy(t.gameObject);
                    _menuOptionsTransforms_p0 = new List<Transform>();
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

                    if (_menuOptionsTransforms_p1 != null)
                        foreach (Transform t in _menuOptionsTransforms_p1)
                            Destroy(t.gameObject);
                    _menuOptionsTransforms_p1 = new List<Transform>();
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

    private List<Transform> _menuOptionsTransforms_p0;
    private List<Transform> _menuOptionsTransforms_p1;

    public override void Awake()
    {
        priority = 3000;
        base.Awake();

        _gameModel = ServiceLocator.Instance.Model.GetComponent<ModelGame>();
        _eqptMenuModel = ServiceLocator.Instance.Model.GetComponent<ModelMenuEqpt>();

        _menuParent_p0 = ServiceLocator.Instance.View.Find("Canvas").Find("P0 Menu Parent");
        _menuParent_p1 = ServiceLocator.Instance.View.Find("Canvas").Find("P1 Menu Parent");

        _title_p0 = _menuParent_p0.Find("P0 Title");
        _title_p1 = _menuParent_p1.Find("P1 Title");

        _option1_p0 = _menuParent_p0.Find("P0 Option 1");
        _option1_p1 = _menuParent_p1.Find("P1 Option 1");

        _cursor_p0 = _menuParent_p0.Find("P0 Cursor");
        _cursor_p1 = _menuParent_p1.Find("P1 Cursor");
    }

    public void Start()
    {
        _option1_p0.transform.localPosition = new Vector3(0, 1000, 0);
        _option1_p1.transform.localPosition = new Vector3(0, 1000, 0);
    }

    public override void S_Update()
    {
        _p0CurrentState = _gameModel.CtrlState_P0;
        _p1CurrentState = _gameModel.CtrlState_P1;

        _CursorUpdate();
    }

    private void _PopulateMenu(ServiceLocator.ID id)
    {
        if (id == ServiceLocator.ID.p0)
        {
            _title_p0.GetComponent<Text>().text = _eqptMenuModel.p0eqpt.type.ToString() + " Operation";

            for (int i = 0; i < _eqptMenuModel.p0EqptCommandsActive.Count; i++)
            {
                Command cmd;

                _eqptMenuModel.p0EqptCommandSeq.TryGetValue(i, out cmd);

                Debug.Log(cmd.name.ToString());

                _menuOptionsText_p0.Add(cmd.action.ToString() + " " + cmd.target.ToString());
            }

            for (int i = 0; i < _menuOptionsText_p0.Count; i++)
            {
                _menuOptionsTransforms_p0.Add(Instantiate(_option1_p0, _menuParent_p0));
                _menuOptionsTransforms_p0[i].transform.localPosition = _p0Option1Anchor + Vector3.down * 100 * i;
                _menuOptionsTransforms_p0[i].GetComponent<Text>().text = _menuOptionsText_p0[i];
            }
        }
        else
        {
            _title_p1.GetComponent<Text>().text = _eqptMenuModel.p1eqpt.type.ToString() + " Operation";

            for (int i = 0; i < _eqptMenuModel.p1EqptCommandsActive.Count; i++)
            {
                Command cmd;
                _eqptMenuModel.p1EqptCommandSeq.TryGetValue(i, out cmd);
                _menuOptionsText_p1.Add(cmd.action.ToString() + " " + cmd.target.ToString());
            }

            for (int i = 0; i < _menuOptionsText_p1.Count; i++)
            {
                _menuOptionsTransforms_p1.Add(Instantiate(_option1_p1, _menuParent_p1));
                _menuOptionsTransforms_p1[i].transform.localPosition = _p1Option1Anchor + Vector3.down * 100 * i;
                _menuOptionsTransforms_p1[i].GetComponent<Text>().text = _menuOptionsText_p1[i];
            }
        }
    }

    private void _CursorUpdate()
    {
        if (_gameModel.GameState == ServiceLocator.GameStates.Play)
        {
            if (_gameModel.CtrlState_P0 == ServiceLocator.ControlStates.Menu_Eqpt)
            {
                _cursor_p0.transform.localPosition = _gameModel.EqptMenuSelect_P0 * Vector3.down * 100 + _p0Option1Anchor;
            }

            if (_gameModel.CtrlState_P1 == ServiceLocator.ControlStates.Menu_Eqpt)
            {
                _cursor_p1.transform.localPosition = _gameModel.EqptMenuSelect_P1 * Vector3.down * 100 + _p1Option1Anchor;
            }
        }
    }
}
