using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelGame : MonoBehaviour {

    public ServiceLocator.GameStates GameState { get; private set;}

    public ServiceLocator.ControlStates CtrlState_P0 { get; private set; }
    public ServiceLocator.ControlStates CtrlState_P1 { get; private set; }

    public int EqptMenuSelect_P0;
    public string EqptMenu_P0_Title { get; private set; }
    public string EqptMenu_P0_Option1 { get; private set; }
    public string EqptMenu_P0_Option2 { get; private set; }
    public string EqptMenu_P0_Option3 { get; private set; }
    public string EqptMenu_P0_Option4 { get; private set; }

    public int EqptMenuSelect_P1;
    public string EqptMenu_P1_Title { get; private set; }
    public string EqptMenu_P1_Option1 { get; private set; }
    public string EqptMenu_P1_Option2 { get; private set; }
    public string EqptMenu_P1_Option3 { get; private set; }
    public string EqptMenu_P1_Option4 { get; private set; }

    public int GameMenuSelect;

    #region Public Functions

    public void SetGameState(ServiceLocator.GameStates newGameState) { GameState = newGameState; }

    public void SetControlState(ServiceLocator.ID id, ServiceLocator.ControlStates newControlState)
    {
        if (id == ServiceLocator.ID.p0)
            CtrlState_P0 = newControlState;
        else
            CtrlState_P1 = newControlState;
    }

    public void SetEqptMenuText(ServiceLocator.ID id, string title, string o1, string o2, string o3, string o4)
    {
        if (id == ServiceLocator.ID.p0)
        {
            EqptMenuSelect_P0 = 0;
            EqptMenu_P0_Title = title;
            EqptMenu_P0_Option1 = o1;
            EqptMenu_P0_Option2 = o2;
            EqptMenu_P0_Option3 = o3;
            EqptMenu_P0_Option4 = o4;
        }
        else
        {
            EqptMenuSelect_P1 = 0;
            EqptMenu_P1_Title = title;
            EqptMenu_P1_Option1 = o1;
            EqptMenu_P1_Option2 = o2;
            EqptMenu_P1_Option3 = o3;
            EqptMenu_P1_Option4 = o4;
        }
    }

    #endregion
}
