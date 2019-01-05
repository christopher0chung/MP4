using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewMenuGame : MP4_ScheduledMono {

    private ModelGame _gameModel;

    private Transform _canvas;

    private Transform _menuParent;
    private Transform _bkgd;
    private Transform _title;
    private Transform _cursor;
    private Transform _option1;
    private Transform _option2;
    private Transform _option3;
    private Transform _option4;

    private bool _a;
    private bool _menu_Active
    {
        get
        {
            return _a;
        }
        set
        {
            if (value != _a)
            {
                if (value)
                    _MakeVisible();
                else
                    _MakeNotVisible();
                _a = value;
            }
        }
    }

    public override void Awake()
    {
        priority = 3001;
        base.Awake();

        _gameModel = ServiceLocator.Instance.Model.GetComponent<ModelGame>();

        _canvas = ServiceLocator.Instance.View.Find("Canvas");

        _menuParent = _canvas.Find("Game Menu Parent");

        _bkgd = _menuParent.Find("Bkgd");
        _title = _menuParent.Find("Title");
        _cursor = _menuParent.Find("Cursor");
        _option1 = _menuParent.Find("Option 1");
        _option2 = _menuParent.Find("Option 2");
        _option3 = _menuParent.Find("Option 3");
        _option4 = _menuParent.Find("Option 4");
    }

    private void Start()
    {
        _menu_Active = true;
        _menu_Active = false;
    }

    public override void S_Update()
    {
        _MenuActiveCheck();
        _CursorUpdate();
    }

    private void _MenuActiveCheck()
    {
        if (_gameModel.GameState == ServiceLocator.GameStates.Menu)
            _menu_Active = true;
        else
            _menu_Active = false;
    }

    private void _CursorUpdate()
    {
        if (_menu_Active)
        {
            if (_gameModel.GameMenuSelect == 0)
            {
                Vector3 cursorPos = _cursor.localPosition;
                cursorPos.y = _option1.localPosition.y;
                _cursor.localPosition = cursorPos;
            }
            else if (_gameModel.GameMenuSelect == 1)
            {
                Vector3 cursorPos = _cursor.localPosition;
                cursorPos.y = _option2.localPosition.y;
                _cursor.localPosition = cursorPos;
            }
            else if (_gameModel.GameMenuSelect == 2)
            {
                Vector3 cursorPos = _cursor.localPosition;
                cursorPos.y = _option3.localPosition.y;
                _cursor.localPosition = cursorPos;
            }
            else if (_gameModel.GameMenuSelect == 3)
            {
                Vector3 cursorPos = _cursor.localPosition;
                cursorPos.y = _option4.localPosition.y;
                _cursor.localPosition = cursorPos;
            }
            else
            {
                Vector3 cursorPos = _cursor.localPosition;
                cursorPos.y = 10000;
                _cursor.localPosition = cursorPos;
            }
        }
    }

    private void _MakeVisible()
    {
        _menuParent.localPosition = Vector3.zero;
    }

    private void _MakeNotVisible()
    {
        _menuParent.localPosition = Vector3.up * 10000;
    }
}
