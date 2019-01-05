using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerInput_Temp : MP4_ScheduledMono {

    private ModelInput _inputModel;
    private ModelGame _gameModel;

    public override void Awake()
    {
        priority = 1000;
        base.Awake();

        _inputModel = ServiceLocator.Instance.Model.GetComponent<ModelInput>();
        _gameModel = ServiceLocator.Instance.Model.GetComponent<ModelGame>();
    }

    public override void S_Update()
    {

        _inputModel.SetInputStates(ServiceLocator.ID.p0, ServiceLocator.Inputs.Up, ServiceLocator.InputStates.Down, Input.GetKey(KeyCode.W));
        _inputModel.SetInputStates(ServiceLocator.ID.p0, ServiceLocator.Inputs.Down, ServiceLocator.InputStates.Down, Input.GetKey(KeyCode.S));
        _inputModel.SetInputStates(ServiceLocator.ID.p0, ServiceLocator.Inputs.Left, ServiceLocator.InputStates.Down, Input.GetKey(KeyCode.A));
        _inputModel.SetInputStates(ServiceLocator.ID.p0, ServiceLocator.Inputs.Right, ServiceLocator.InputStates.Down, Input.GetKey(KeyCode.D));

        _inputModel.SetInputStates(ServiceLocator.ID.p0, ServiceLocator.Inputs.Use, ServiceLocator.InputStates.Down, Input.GetKey(KeyCode.Q));
        _inputModel.SetInputStates(ServiceLocator.ID.p0, ServiceLocator.Inputs.Grab, ServiceLocator.InputStates.Down, Input.GetKey(KeyCode.E));


        //--------------------------------------

        _inputModel.SetInputStates(ServiceLocator.ID.p0, ServiceLocator.Inputs.Up, ServiceLocator.InputStates.OnDown, Input.GetKeyDown(KeyCode.W));
        _inputModel.SetInputStates(ServiceLocator.ID.p0, ServiceLocator.Inputs.Down, ServiceLocator.InputStates.OnDown, Input.GetKeyDown(KeyCode.S));
        _inputModel.SetInputStates(ServiceLocator.ID.p0, ServiceLocator.Inputs.Left, ServiceLocator.InputStates.OnDown, Input.GetKeyDown(KeyCode.A));
        _inputModel.SetInputStates(ServiceLocator.ID.p0, ServiceLocator.Inputs.Right, ServiceLocator.InputStates.OnDown, Input.GetKeyDown(KeyCode.D));

        _inputModel.SetInputStates(ServiceLocator.ID.p0, ServiceLocator.Inputs.Use, ServiceLocator.InputStates.OnDown, Input.GetKeyDown(KeyCode.Q));
        _inputModel.SetInputStates(ServiceLocator.ID.p0, ServiceLocator.Inputs.Grab, ServiceLocator.InputStates.OnDown, Input.GetKeyDown(KeyCode.E));


        //--------------------------------------

        _inputModel.SetInputStates(ServiceLocator.ID.p0, ServiceLocator.Inputs.Up, ServiceLocator.InputStates.OnUp, Input.GetKeyUp(KeyCode.W));
        _inputModel.SetInputStates(ServiceLocator.ID.p0, ServiceLocator.Inputs.Down, ServiceLocator.InputStates.OnUp, Input.GetKeyUp(KeyCode.S));
        _inputModel.SetInputStates(ServiceLocator.ID.p0, ServiceLocator.Inputs.Left, ServiceLocator.InputStates.OnUp, Input.GetKeyUp(KeyCode.A));
        _inputModel.SetInputStates(ServiceLocator.ID.p0, ServiceLocator.Inputs.Right, ServiceLocator.InputStates.OnUp, Input.GetKeyUp(KeyCode.D));

        _inputModel.SetInputStates(ServiceLocator.ID.p0, ServiceLocator.Inputs.Use, ServiceLocator.InputStates.OnUp, Input.GetKeyUp(KeyCode.Q));
        _inputModel.SetInputStates(ServiceLocator.ID.p0, ServiceLocator.Inputs.Grab, ServiceLocator.InputStates.OnUp, Input.GetKeyUp(KeyCode.E));



        if (Input.GetKeyDown(KeyCode.J))
            _gameModel.SetGameState(ServiceLocator.GameStates.Play);
        if (Input.GetKeyDown(KeyCode.K))
            _gameModel.SetGameState(ServiceLocator.GameStates.Cutscene);
        if (Input.GetKeyDown(KeyCode.L))
            _gameModel.SetGameState(ServiceLocator.GameStates.Menu);


        if (Input.GetKeyDown(KeyCode.Y))
            _gameModel.SetControlState(ServiceLocator.ID.p0, ServiceLocator.ControlStates.Disabled);
        if (Input.GetKeyDown(KeyCode.U))
            _gameModel.SetControlState(ServiceLocator.ID.p0, ServiceLocator.ControlStates.Free);
        if (Input.GetKeyDown(KeyCode.I))
            _gameModel.SetControlState(ServiceLocator.ID.p0, ServiceLocator.ControlStates.Menu_Eqpt);
        if (Input.GetKeyDown(KeyCode.O))
            _gameModel.SetControlState(ServiceLocator.ID.p0, ServiceLocator.ControlStates.Occupied);
        if (Input.GetKeyDown(KeyCode.P))
            _gameModel.SetControlState(ServiceLocator.ID.p0, ServiceLocator.ControlStates.Station);
    }
}
