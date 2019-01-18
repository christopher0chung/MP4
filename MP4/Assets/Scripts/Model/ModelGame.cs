using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelGame : MonoBehaviour {

    public ServiceLocator.GameStates GameState { get; private set;}

    public ServiceLocator.ControlStates CtrlState_P0 { get; private set; }
    public ServiceLocator.ControlStates CtrlState_P1 { get; private set; }

    public float playerBaseMass;

    public int EqptMenuSelect_P0;

    public int EqptMenuSelect_P1;

    public int GameMenuSelect;

    public Vector3 P0_LookDir { get; private set; }
    public Vector3 P1_LookDir { get; private set; }

    public Transform P0_Hands { get; private set; }
    public Transform P1_Hands { get; private set; }

    #region Public Functions

    public void Init(float playerBaseMass)
    {
        this.playerBaseMass = playerBaseMass;
    }

    public void SetHands(ServiceLocator.ID id, Transform hands)
    {
        if (id == ServiceLocator.ID.p0)
            P0_Hands = hands;
        else
            P1_Hands = hands;
    }

    public void SetGameState(ServiceLocator.GameStates newGameState) { GameState = newGameState; }

    public void SetControlState(ServiceLocator.ID id, ServiceLocator.ControlStates newControlState)
    {
        if (id == ServiceLocator.ID.p0)
            CtrlState_P0 = newControlState;
        else
            CtrlState_P1 = newControlState;
    }

    public void SetLookDir(ServiceLocator.ID id, Vector3 lookDir)
    {
        if (id == ServiceLocator.ID.p0)
            P0_LookDir = lookDir;
        else
            P1_LookDir = lookDir;
    }

    #endregion
}
