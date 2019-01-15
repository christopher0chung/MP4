using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerGame : MP4_ScheduledMono {

    private ModelGame _gameModel;
    private ModelInput _inputModel;
    private ModelObjectInteraction _objIntModel;

    private ControllerObjectInteraction _objIntCtrlr;

    private SCG_FSM<ControllerGame> _GameStateFSM;
    private SCG_FSM<ControllerGame> _p0_PlayInputs;
    private SCG_FSM<ControllerGame> _p1_PlayInputs;

    private Transform _p0_Transfrom;
    private Transform _p1_Transform;

    private Rigidbody _p0_RigidBody;
    private Rigidbody _p1_RigidBody;

    public float tempTuning_LookRate;


    public override void Awake()
    {
        priority = 2500;
        base.Awake();

        _gameModel = ServiceLocator.Instance.Model.GetComponent<ModelGame>();
        _inputModel = ServiceLocator.Instance.Model.GetComponent<ModelInput>();
        _objIntModel = ServiceLocator.Instance.Model.GetComponent<ModelObjectInteraction>();

        _objIntCtrlr = ServiceLocator.Instance.Controller.GetComponent<ControllerObjectInteraction>();

        _p0_Transfrom = ServiceLocator.Instance.Character0;
        _p1_Transform = ServiceLocator.Instance.Character1;

        _p0_RigidBody = _p0_Transfrom.GetComponent<Rigidbody>();
        _p1_RigidBody = _p1_Transform.GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _gameModel.SetGameState(ServiceLocator.GameStates.Play);
        _gameModel.SetControlState(ServiceLocator.ID.p0, ServiceLocator.ControlStates.Free);
        _gameModel.SetControlState(ServiceLocator.ID.p1, ServiceLocator.ControlStates.Free);
        _gameModel.SetLookDir(ServiceLocator.ID.p0, Vector3.right);
        _gameModel.SetLookDir(ServiceLocator.ID.p1, Vector3.right);
        _gameModel.SetHands(ServiceLocator.ID.p0, GameObject.FindGameObjectWithTag("P0Hands").transform);
        _gameModel.SetHands(ServiceLocator.ID.p1, GameObject.FindGameObjectWithTag("P1Hands").transform);
        _gameModel.Init(50);

        _p0_PlayInputs = new SCG_FSM<ControllerGame>(this);
        _p1_PlayInputs = new SCG_FSM<ControllerGame>(this);

        _GameStateFSM = new SCG_FSM<ControllerGame>(this);
        _GameStateFSM.TransitionTo<GameState_Play>();
    }

    public override void S_Update()
    {
        _GameStateCheck();
        _GameStateFSM.Update();
    }

    private void _GameStateCheck()
    {
        if (_gameModel.GameState == ServiceLocator.GameStates.Play)
            _GameStateFSM.TransitionTo<GameState_Play>();
        else if (_gameModel.GameState == ServiceLocator.GameStates.Menu)
            _GameStateFSM.TransitionTo<GameState_Menu>();
        else
            _GameStateFSM.TransitionTo<GameState_CutScene>();
    }



    #region Game State FSM

    public class StateBase : SCG_FSM<ControllerGame>.State
    {
        public override void OnEnter()
        {
            base.OnEnter();
        }

        protected void _ControlStateChangeCheck()
        {
            if (Context._gameModel.CtrlState_P0 == ServiceLocator.ControlStates.Free)
                Context._p0_PlayInputs.TransitionTo<P0_ControlState_Free>();
            else if (Context._gameModel.CtrlState_P0 == ServiceLocator.ControlStates.Disabled)
                Context._p0_PlayInputs.TransitionTo<P01_ControlState_Disabled>();
            else if (Context._gameModel.CtrlState_P0 == ServiceLocator.ControlStates.Menu_Eqpt)
                Context._p0_PlayInputs.TransitionTo<P0_ControlState_MenuEqpt>();
            else if (Context._gameModel.CtrlState_P0 == ServiceLocator.ControlStates.Occupied)
                Context._p0_PlayInputs.TransitionTo<P0_ControlState_Occupied>();
            else if (Context._gameModel.CtrlState_P0 == ServiceLocator.ControlStates.Station)
                Context._p0_PlayInputs.TransitionTo<P0_ControlState_Station>();


            if (Context._gameModel.CtrlState_P1 == ServiceLocator.ControlStates.Free)
                Context._p1_PlayInputs.TransitionTo<P1_ControlState_Free>();
            else if (Context._gameModel.CtrlState_P1 == ServiceLocator.ControlStates.Disabled)
                Context._p1_PlayInputs.TransitionTo<P01_ControlState_Disabled>();
            else if (Context._gameModel.CtrlState_P1 == ServiceLocator.ControlStates.Menu_Eqpt)
                Context._p1_PlayInputs.TransitionTo<P1_ControlState_MenuEqpt>();
            else if (Context._gameModel.CtrlState_P1 == ServiceLocator.ControlStates.Occupied)
                Context._p1_PlayInputs.TransitionTo<P1_ControlState_Occupied>();
            else if (Context._gameModel.CtrlState_P1 == ServiceLocator.ControlStates.Station)
                Context._p1_PlayInputs.TransitionTo<P1_ControlState_Station>();
        }

        protected void _PlayerMassUpdate()
        {
            if (Context._objIntModel.p0_InteractableGrabbed != null)
            {
                float grabbedMass = Context._objIntModel.BodyOfThing(Context._objIntModel.p0_InteractableGrabbed).GetComponent<ViewThing_Generic>().serializedRigidBody.mass;
                Context._p0_RigidBody.mass = Context._gameModel.playerBaseMass + grabbedMass;
            }
            else
                Context._p0_RigidBody.mass = Context._gameModel.playerBaseMass;

            if (Context._objIntModel.p1_InteractableGrabbed != null)
            {
                float grabbedMass = Context._objIntModel.BodyOfThing(Context._objIntModel.p1_InteractableGrabbed).GetComponent<ViewThing_Generic>().serializedRigidBody.mass;
                Context._p1_RigidBody.mass = Context._gameModel.playerBaseMass + grabbedMass;
            }
            else
                Context._p1_RigidBody.mass = Context._gameModel.playerBaseMass;
        }
    }

    public class GameState_Play : StateBase
    {
        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override void Update()
        {
            base.Update();

            _ControlStateChangeCheck();

            _PlayerMassUpdate();

            Context._p0_PlayInputs.Update();
            Context._p1_PlayInputs.Update();
        }
    }

    public class GameState_Menu : StateBase
    {
        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override void Update()
        {
            base.Update();
            if (Context._inputModel.P0_Up_OnDown || Context._inputModel.P1_Up_OnDown)
            {
                int selection = Context._gameModel.GameMenuSelect;
                selection--;
                if (selection < 0)
                    selection = 3;
                Context._gameModel.GameMenuSelect = selection;
                Debug.Log("Game menu up");
            }
            if (Context._inputModel.P0_Down_OnDown || Context._inputModel.P1_Down_OnDown)
            {
                int selection = Context._gameModel.GameMenuSelect;
                selection++;
                if (selection > 3)
                    selection = 0;
                Context._gameModel.GameMenuSelect = selection;
                Debug.Log("Game menu down");
            }
            if (Context._inputModel.P0_Left_OnDown || Context._inputModel.P1_Left_OnDown)
                Debug.Log("Game menu left");
            if (Context._inputModel.P0_Right_OnDown || Context._inputModel.P1_Right_OnDown)
                Debug.Log("Game menu right");

            if (Context._inputModel.P0_Use_OnDown || Context._inputModel.P0_Use_OnDown)
                Debug.Log("Game menu accept");
            else if (Context._inputModel.P0_Grab_OnDown || Context._inputModel.P1_Grab_OnDown)
            {
                Debug.Log("Game menu cancel");
                Context._gameModel.SetGameState(ServiceLocator.GameStates.Play);
            }
        }
    }

    public class GameState_CutScene : StateBase
    {

    }

    #endregion

    #region Player Game Control States

    public class ControlState_Base : SCG_FSM<ControllerGame>.State { }

    public class P0_ControlState_Free : ControlState_Base
    {
        public override void Update()
        {
            if (Context._inputModel.P0_Up_IsDown)
            {
                Context._p0_RigidBody.AddForce(Vector3.up * 1000);
                Debug.Log("P0 is going up");
            }
            if (Context._inputModel.P0_Down_IsDown)
            {
                Context._p0_RigidBody.AddForce(Vector3.down * 1000);
                Debug.Log("P0 is going down");
            }
            if (Context._inputModel.P0_Left_IsDown)
            {
                Context._p0_RigidBody.AddForce(Vector3.left * 1000);
                Debug.Log("P0 is going left");
            }
            if (Context._inputModel.P0_Right_IsDown)
            {
                Context._p0_RigidBody.AddForce(Vector3.right * 1000);
                Debug.Log("P0 is going right");
            }

            if (Context._inputModel.P0_Grab_OnDown && !Context._inputModel.P0_Use_OnDown)
            {
                Debug.Log("P0 Attempt to grab");
                if (Context._objIntModel.p0_InteractableGrabbed == null && Context._objIntModel.p0_InteractableInterested != null)
                    Context._objIntCtrlr.AttemptToHold(ServiceLocator.ID.p0);
                else if (Context._objIntModel.p0_InteractableGrabbed != null)
                    Context._objIntCtrlr.Unhold(ServiceLocator.ID.p0);
            }
            else if (!Context._inputModel.P0_Grab_OnDown && Context._inputModel.P0_Use_OnDown)
            {
                Debug.Log("P0 Attempt to use");
                if (Context._objIntModel.p0_InteractableInterested != null)
                {
                    if (Context._objIntModel.p0_InteractableInterested.cat == ServiceLocator.InteractivesCategory.Equipment 
                        // should also permit stn//
                        )
                    {
                        TransitionTo<P0_ControlState_MenuEqpt>();
                    }
                }
            }

            //Temp input scheme until controllers integrated
            if (Input.GetKey(KeyCode.R))
                Context._gameModel.SetLookDir(ServiceLocator.ID.p0, Quaternion.Euler(0, 0, Context.tempTuning_LookRate * Time.deltaTime) * Context._gameModel.P0_LookDir);
            if (Input.GetKey(KeyCode.F))
                Context._gameModel.SetLookDir(ServiceLocator.ID.p0, Quaternion.Euler(0, 0, -Context.tempTuning_LookRate * Time.deltaTime) * Context._gameModel.P0_LookDir);
        }
    }

    public class P1_ControlState_Free : ControlState_Base
    {
        public override void Update()
        {
            if (Context._inputModel.P1_Up_IsDown)
                Debug.Log("P1 is going up");
            if (Context._inputModel.P1_Down_IsDown)
                Debug.Log("P1 is going down");
            if (Context._inputModel.P1_Left_IsDown)
                Debug.Log("P1 is going left");
            if (Context._inputModel.P1_Right_IsDown)
                Debug.Log("P1 is going right");

            if (Context._inputModel.P1_Grab_OnDown && !Context._inputModel.P1_Use_OnDown)
                Debug.Log("P1 Attempt to grab");
            else if (!Context._inputModel.P1_Grab_OnDown && Context._inputModel.P1_Use_OnDown)
                Debug.Log("P1 Attempt to use");
        }
    }

    public class P01_ControlState_Disabled : ControlState_Base { }

    public class P0_ControlState_MenuEqpt : ControlState_Base
    {
        public override void OnEnter()
        {
            base.OnEnter();
            Debug.Log("Prepare P0 Menu");
            Debug.Log("Make P0 Menu");
        }

        public override void Update()
        {
            base.Update();
            if (Context._inputModel.P0_Up_OnDown)
            {
                int selection = Context._gameModel.EqptMenuSelect_P0;
                selection--;
                if (selection < 0)
                    selection = 3;
                Context._gameModel.EqptMenuSelect_P0 = selection;
                Debug.Log("P0 menu up");
            }
            if (Context._inputModel.P0_Down_OnDown)
            {
                int selection = Context._gameModel.EqptMenuSelect_P0;
                selection++;
                if (selection > 3)
                    selection = 0;
                Context._gameModel.EqptMenuSelect_P0 = selection;
                Debug.Log("P0 menu down");
            }
            if (Context._inputModel.P0_Left_OnDown)
                Debug.Log("P0 menu left");
            if (Context._inputModel.P0_Right_OnDown)
                Debug.Log("P0 menu right");

            if (Context._inputModel.P0_Use_OnDown && !Context._inputModel.P0_Grab_OnDown)
                Debug.Log("P0 menu accept");
            else if (Context._inputModel.P0_Grab_OnDown && !Context._inputModel.P0_Use_OnDown)
            {
                Debug.Log("P0 menu cancel");
                Context._gameModel.SetControlState(ServiceLocator.ID.p0, ServiceLocator.ControlStates.Free);
            }
        }
    }

    public class P1_ControlState_MenuEqpt : ControlState_Base
    {
        public override void OnEnter()
        {
            base.OnEnter();
            Debug.Log("Prepare P1 Menu");
            Debug.Log("Make P1 Menu");
        }

        public override void Update()
        {
            base.Update();
            if (Context._inputModel.P1_Up_OnDown)
            {
                int selection = Context._gameModel.EqptMenuSelect_P1;
                selection--;
                if (selection < 0)
                    selection = 3;
                Context._gameModel.EqptMenuSelect_P1 = selection;
                Debug.Log("P1 menu up");
            }
            if (Context._inputModel.P1_Down_OnDown)
            {
                int selection = Context._gameModel.EqptMenuSelect_P1;
                selection++;
                if (selection > 3)
                    selection = 0;
                Context._gameModel.EqptMenuSelect_P1 = selection;
                Debug.Log("P1 menu down");
            }
            if (Context._inputModel.P1_Left_OnDown)
                Debug.Log("P1 menu left");
            if (Context._inputModel.P1_Right_OnDown)
                Debug.Log("P1 menu right");

            if (Context._inputModel.P1_Use_OnDown && !Context._inputModel.P1_Grab_OnDown)
                Debug.Log("P1 menu accept");
            else if (Context._inputModel.P1_Grab_OnDown && !Context._inputModel.P1_Use_OnDown)
            {
                Debug.Log("P1 menu cancel");
                Context._gameModel.SetControlState(ServiceLocator.ID.p0, ServiceLocator.ControlStates.Free);
            }
        }
    }

    public class P0_ControlState_Occupied : ControlState_Base
    {
        public override void Update()
        {
            base.Update();

            if (Context._inputModel.P0_Grab_OnDown)
            {
                Context._gameModel.SetControlState(ServiceLocator.ID.p0, ServiceLocator.ControlStates.Free);
                Debug.Log("P0 Cancel Occupied");
            }
        }
    }

    public class P1_ControlState_Occupied : ControlState_Base
    {
        public override void Update()
        {
            base.Update();

            if (Context._inputModel.P1_Grab_OnDown)
                Context._gameModel.SetControlState(ServiceLocator.ID.p1, ServiceLocator.ControlStates.Free);
        }
    }

    public class P0_ControlState_Station : ControlState_Base
    {
        public override void Update()
        {
            base.Update();
            if (Context._inputModel.P0_Up_IsDown)
                Debug.Log("P0's station input: up");
            if (Context._inputModel.P0_Down_IsDown)
                Debug.Log("P0's station input: down");
            if (Context._inputModel.P0_Left_IsDown)
                Debug.Log("P0's station input: left");
            if (Context._inputModel.P0_Right_IsDown)
                Debug.Log("P0's station input: right");

            if (Context._inputModel.P0_Use_IsDown)
                Debug.Log("P0's station input: A");
            if (Context._inputModel.P0_Grab_IsDown)
            {
                Context._gameModel.SetControlState(ServiceLocator.ID.p0, ServiceLocator.ControlStates.Free);
                Debug.Log("P0 cancel out of Station");
            }
        }
    }

    public class P1_ControlState_Station : ControlState_Base
    {
        public override void Update()
        {
            base.Update();
            if (Context._inputModel.P1_Up_IsDown)
                Debug.Log("P1's station input: up");
            if (Context._inputModel.P1_Down_IsDown)
                Debug.Log("P1's station input: down");
            if (Context._inputModel.P1_Left_IsDown)
                Debug.Log("P1's station input: left");
            if (Context._inputModel.P1_Right_IsDown)
                Debug.Log("P1's station input: right");

            if (Context._inputModel.P1_Use_IsDown)
                Debug.Log("P1's station input: A");
            if (Context._inputModel.P1_Grab_IsDown)
            {
                Context._gameModel.SetControlState(ServiceLocator.ID.p1, ServiceLocator.ControlStates.Free);
                Debug.Log("P1 cancel out of Station");
            }
        }
    }
    #endregion
}
