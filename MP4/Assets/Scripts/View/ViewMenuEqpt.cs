using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewMenuEqpt : MP4_ScheduledMono {

    private ModelGame _gameModel;

    private Transform _canvas;

    private Transform _p0_MenuParent;
    private Transform _p0_Bkgd;
    private Transform _p0_Title;
    private Transform _p0_Cursor;
    private Transform _p0_Option1;
    private Transform _p0_Option2;
    private Transform _p0_Option3;
    private Transform _p0_Option4;

    private Transform _p1_MenuParent;
    private Transform _p1_Bkgd;
    private Transform _p1_Title;
    private Transform _p1_Cursor;
    private Transform _p1_Option1;
    private Transform _p1_Option2;
    private Transform _p1_Option3;
    private Transform _p1_Option4;

    private bool _p0;
    private bool _p0_EqptMenu_Active
    {
        get
        {
            return _p0;
        }
        set
        {
            if (value != _p0)
            {
                if (value)
                    _MakeVisible_P0();
                else
                    _MakeNotVisible_P0();
                _p0 = value;
            }
        }
    }
    private bool _p1;
    private bool _p1_EqptMenu_Active
    {
        get
        {
            return _p1;
        }
        set
        {
            if (value != _p1)
            {
                if (value)
                    _MakeVisible_P1();
                else
                    _MakeNotVisible_P1();
                _p1 = value;
            }
        }
    }

    public override void Awake()
    {
        priority = 3000;
        base.Awake();

        _gameModel = ServiceLocator.Instance.Model.GetComponent<ModelGame>();

        _canvas = ServiceLocator.Instance.View.Find("Canvas");

        _p0_MenuParent = _canvas.Find("P0 Menu Parent");
        _p1_MenuParent = _canvas.Find("P1 Menu Parent");

        _p0_Bkgd = _p0_MenuParent.Find("P0 Bkgd");
        _p0_Title = _p0_MenuParent.Find("P0 Title");
        _p0_Cursor = _p0_MenuParent.Find("P0 Cursor");
        _p0_Option1 = _p0_MenuParent.Find("P0 Option 1");
        _p0_Option2 = _p0_MenuParent.Find("P0 Option 2");
        _p0_Option3 = _p0_MenuParent.Find("P0 Option 3");
        _p0_Option4 = _p0_MenuParent.Find("P0 Option 4");

        _p1_Bkgd = _p1_MenuParent.Find("P1 Bkgd");
        _p1_Title = _p1_MenuParent.Find("P1 Title");
        _p1_Cursor = _p1_MenuParent.Find("P1 Cursor");
        _p1_Option1 = _p1_MenuParent.Find("P1 Option 1");
        _p1_Option2 = _p1_MenuParent.Find("P1 Option 2");
        _p1_Option3 = _p1_MenuParent.Find("P1 Option 3");
        _p1_Option4 = _p1_MenuParent.Find("P1 Option 4");


        Debug.Assert(_p0_MenuParent != null);
        Debug.Assert(_p1_MenuParent != null);

        Debug.Assert(_p0_Bkgd != null);
        Debug.Assert(_p0_Title != null);
        Debug.Assert(_p0_Cursor != null);
        Debug.Assert(_p0_Option1 != null);
        Debug.Assert(_p0_Option1 != null);
        Debug.Assert(_p0_Option1 != null);
        Debug.Assert(_p0_Option1 != null);

        Debug.Assert(_p1_Bkgd != null);
        Debug.Assert(_p0_Title != null);
        Debug.Assert(_p0_Cursor != null);
        Debug.Assert(_p1_Option1 != null);
        Debug.Assert(_p1_Option1 != null);
        Debug.Assert(_p1_Option1 != null);
        Debug.Assert(_p1_Option1 != null);
    }

    void Start () {
        _MakeNotVisible_P0();
        _MakeNotVisible_P1();
	}

	public override void S_Update () {
        _MenuActiveCheck();
        _CursorUpdate_P0();
        _CursorUpdate_P1();
    }

    private void _MenuActiveCheck()
    {
        if (_gameModel.GameState == ServiceLocator.GameStates.Play)
        {
            if (_gameModel.CtrlState_P0 == ServiceLocator.ControlStates.Menu_Eqpt)
                _p0_EqptMenu_Active = true;
            else
                _p0_EqptMenu_Active = false;

            if (_gameModel.CtrlState_P1 == ServiceLocator.ControlStates.Menu_Eqpt)
                _p1_EqptMenu_Active = true;
            else
                _p1_EqptMenu_Active = false;
        }
        else
        {
            _p0_EqptMenu_Active = false;
            _p1_EqptMenu_Active = false;
        }
    }

    private void _CursorUpdate_P0()
    {
        if (_p0_EqptMenu_Active)
        {
            if (_gameModel.EqptMenuSelect_P0 == 0)
            {
                Vector3 cursorPos = _p0_Cursor.localPosition;
                cursorPos.y = _p0_Option1.localPosition.y;
                _p0_Cursor.localPosition = cursorPos;
                //Debug.Log(_p0_Option1.localPosition + " " + _p0_Cursor.localPosition);

            }
            else if (_gameModel.EqptMenuSelect_P0 == 1)
            {
                Vector3 cursorPos = _p0_Cursor.localPosition;
                cursorPos.y = _p0_Option2.localPosition.y;
                _p0_Cursor.localPosition = cursorPos;
                //Debug.Log(_p0_Option2.localPosition + " " + _p0_Cursor.localPosition);

            }
            else if (_gameModel.EqptMenuSelect_P0 == 2)
            {
                Vector3 cursorPos = _p0_Cursor.localPosition;
                cursorPos.y = _p0_Option3.localPosition.y;
                _p0_Cursor.localPosition = cursorPos;
                //Debug.Log(_p0_Option3.localPosition + " " + _p0_Cursor.localPosition);

            }
            else if (_gameModel.EqptMenuSelect_P0 == 3)
            {
                Vector3 cursorPos = _p0_Cursor.localPosition;
                cursorPos.y = _p0_Option4.localPosition.y;
                _p0_Cursor.localPosition = cursorPos;
            }
            else
            {
                Vector3 cursorPos = _p0_Cursor.localPosition;
                cursorPos.y = 10000;
                _p0_Cursor.localPosition = cursorPos;
            }
        }
    }

    private void _CursorUpdate_P1()
    {
        if (_p1_EqptMenu_Active)
        {
            if (_gameModel.EqptMenuSelect_P1 == 0)
            {
                Vector3 cursorPos = _p1_Cursor.localPosition;
                cursorPos.y = _p1_Option1.localPosition.y;
                _p1_Cursor.localPosition = cursorPos;
            }
            else if (_gameModel.EqptMenuSelect_P1 == 1)
            {
                Vector3 cursorPos = _p1_Cursor.localPosition;
                cursorPos.y = _p1_Option2.localPosition.y;
                _p1_Cursor.localPosition = cursorPos;
            }
            else if (_gameModel.EqptMenuSelect_P1 == 2)
            {
                Vector3 cursorPos = _p1_Cursor.localPosition;
                cursorPos.y = _p1_Option3.localPosition.y;
                _p1_Cursor.localPosition = cursorPos;
            }
            else if (_gameModel.EqptMenuSelect_P1 == 3)
            {
                Vector3 cursorPos = _p1_Cursor.localPosition;
                cursorPos.y = _p1_Option4.localPosition.y;
                _p1_Cursor.localPosition = cursorPos;
            }
            else
            {
                Vector3 cursorPos = _p1_Cursor.localPosition;
                cursorPos.y = 10000;
                _p1_Cursor.localPosition = cursorPos;
            }
        }
    }

    private void _MakeVisible_P0()
    {
        _p0_MenuParent.localPosition = Vector3.zero;
    }

    private void _MakeVisible_P1()
    {
        _p1_MenuParent.localPosition = Vector3.zero;
    }

    private void _MakeNotVisible_P0()
    {
        _p0_MenuParent.localPosition = Vector3.up * 10000;
    }

    private void _MakeNotVisible_P1()
    {
        _p1_MenuParent.localPosition = Vector3.up * 10000;
    }
}
