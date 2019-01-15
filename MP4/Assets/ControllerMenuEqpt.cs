using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerMenuEqpt : MP4_ScheduledMono {
    private ModelGame _gameModel;
    private ModelObjectInteraction _objIntModel;
    private ModelMenuEqpt _eqptMenuModel;

    private Transform _canvas;

    private Transform _p0_MenuParent;
    private Transform _p1_MenuParent;

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
                {
                    _ParseInteractionCommands(ServiceLocator.ID.p0);
                    _MakeVisible_P0();
                }
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
        _objIntModel = ServiceLocator.Instance.Model.GetComponent<ModelObjectInteraction>();
        _eqptMenuModel = ServiceLocator.Instance.Model.GetComponent<ModelMenuEqpt>();

        _canvas = ServiceLocator.Instance.View.Find("Canvas");

        _p0_MenuParent = _canvas.Find("P0 Menu Parent");
        _p1_MenuParent = _canvas.Find("P1 Menu Parent");

        Debug.Assert(_p0_MenuParent != null);
        Debug.Assert(_p1_MenuParent != null);
    }

    #region Sequence and Structure

    void Start()
    {
        _MakeNotVisible_P0();
        _MakeNotVisible_P1();
    }

    public override void S_Update()
    {
        _MenuActiveCheck();
        _CursorUpdate_P0();
        _CursorUpdate_P1();
    }

    #endregion

    #region Internal Process Functions

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

            }

            else
            {

            }
        }
    }

    private void _CursorUpdate_P1()
    {
        if (_p1_EqptMenu_Active)
        {
            if (_gameModel.EqptMenuSelect_P1 == 0)
            {

            }
            else
            {

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

    #endregion

    private void _ParseInteractionCommands(ServiceLocator.ID id)
    {
        if (id == ServiceLocator.ID.p0)
        {
            Debug.Assert(_objIntModel.p0_InteractableInterested.cat == ServiceLocator.InteractivesCategory.Equipment ||
                _objIntModel.p0_InteractableInterested.cat == ServiceLocator.InteractivesCategory.Stations,
                "Attempting to parse menu for command-less interactiveCategory");

            if (_objIntModel.p0_InteractableInterested.cat == ServiceLocator.InteractivesCategory.Equipment)
            {
                Equipment_Base e = _objIntModel.p0_InteractableInterested as Equipment_Base;
                if (e != null)
                {

                    CommandReferences cRef;
                    Dictionary<Command, CommandReferences> commandDict = new Dictionary<Command, CommandReferences>();

                    for (int i = 0; i < e.possibleCommands.Length; i++)
                    {
                        bool commandPermissible = false;

                        if (_ReqdReagentConditionsMet(id, e, e.possibleCommands[i]) && _ReqdConsumablesPresent(id, e.possibleCommands[i]) && _IntendedTargetPresent(id, e, e.possibleCommands[i]))
                        {
                            commandPermissible = true;
                        }

                        if (commandPermissible)
                        {
                            cRef = ScriptableObject.Instantiate(Resources.Load<CommandReferences>("CommandReference/CommandReference"));
                            _StoreConsumablesRefs(id, cRef, e.possibleCommands[i]);
                            _StoreReagentsRefs(id, e, cRef, e.possibleCommands[i]);
                            _StoreTargetRefs(id, cRef, e.possibleCommands[i]);

                            commandDict.Add(e.possibleCommands[i], cRef);
                        }
                    }
                    commandDict.Add(Resources.Load<Command>("Command/ExitMenu"), Resources.Load<CommandReferences>("CommandReference/CommandReference"));
                    _eqptMenuModel.SetCommands(id, commandDict);
                }
            }
            else if (_objIntModel.p0_InteractableInterested.cat == ServiceLocator.InteractivesCategory.Stations)
            {
                //Stn code
            }

        }
        else
        {
            //Mirror for p1
        }
    }

    #region Internal Logical Checks For Command Viability

    private bool _ReqdConsumablesPresent(ServiceLocator.ID id, Command cmd)
    {
        bool present = true;
        foreach (Thing t in cmd.consumables)
        {
            if (_objIntModel.CountOfTypeInOOC(id, t.type) == 0)
                present = false;
        }
        return present;
    }

    private bool _ReqdReagentConditionsMet(ServiceLocator.ID id, Equipment_Base e, Command cmd)
    {
        // For each cmd action type, a different logical check should be done to determine the specific conditional state for the action

        if (cmd.action == ServiceLocator.Actions.Weld || cmd.action == ServiceLocator.Actions.Power)
        {
            // Check stowage & capacity
            if (_Helper_CheckReagentStowage(e, cmd) && _Helper_CheckReagentCapacity(e, cmd))
                return true;
            else return false;
        }
        else if (cmd.action == ServiceLocator.Actions.Charge || cmd.action == ServiceLocator.Actions.Eject || cmd.action == ServiceLocator.Actions.Unstow)
        {
            // Check stowage
            if (_Helper_CheckReagentStowage(e, cmd))
                return true;
            else return false;
        }
        else
        {
            // Check presence in vicinity
            if (_Helper_CheckReagentPresence(id, cmd))
                return true;
            else return false;
        }
    }

    private bool _Helper_CheckReagentStowage(Equipment_Base e, Command cmd)
    {
        // If the cmd doesn't require any reagents, then stowage is gtg
        if (cmd.reagents.Length == 0)
            return true;

        // If the cmd does require reagents, and the eqpt has the required reagents, then stowage is gtg
        else
        {
            List<ServiceLocator.Interactives> typesStowed = new List<ServiceLocator.Interactives>();

            if (e.stowed.Count == 0)
                return false;

            foreach(Thing t in e.stowed)
            {
                typesStowed.Add(t.type);
            }

            foreach (Reagent r in cmd.reagents)
            {
                if (!typesStowed.Contains(r.reagentType.type))
                    return false;
            }
        }
        return true;
    }

    private bool _Helper_CheckReagentCapacity(Equipment_Base e, Command cmd)
    {
        // If there are no reqd reagents, then capacity check is gtg
        if (cmd.reagents.Length == 0)
            return true;
        // If there are reqd reagents, just check if each cap is not 0 b/c stowage check will already check that each required type is present
        else
        {
            foreach (Item_Base i in e.stowed)
            {
                if (i.capacity == 0)
                    return false;
            }
        }

        return true;
    }

    private bool _Helper_CheckReagentPresence(ServiceLocator.ID id, Command cmd)
    {
        if (cmd.reagents.Length == 0)
            return true;

        else
        {
            List<ServiceLocator.Interactives> typesPresent = new List<ServiceLocator.Interactives>();

            for (int i = 0; i < _objIntModel.p0_Interactable_ObjsOfConcern.Length; i++)
            {
                if (id == ServiceLocator.ID.p0)
                    typesPresent.Add(_objIntModel.p0_Interactable_ObjsOfConcern[i].type);
                else
                    typesPresent.Add(_objIntModel.p1_Interactable_ObjsOfConcern[i].type);
            }

            foreach (Reagent r in cmd.reagents)
            {
                if (!typesPresent.Contains(r.reagentType.type))
                    return false;
            }
        }

        return true;
    }

    private bool _IntendedTargetPresent(ServiceLocator.ID id, Equipment_Base e, Command cmd)
    {
        // Internal targets
        if (cmd.action == ServiceLocator.Actions.Charge || cmd.action == ServiceLocator.Actions.Eject || cmd.action == ServiceLocator.Actions.Unstow)
        {
            List<ServiceLocator.Interactives> _stowedTypes = new List<ServiceLocator.Interactives>();
            foreach (Thing t in e.stowed)
            {
                _stowedTypes.Add(t.type);
            }

            if (_stowedTypes.Contains(cmd.target))
                return true;
            else
                return false;
        }
        // External targets
        else
        {
            if (_objIntModel.CountOfTypeInOOC(id, cmd.target) != 0)
                return true;
            else
                return false;
        }
    }

    private void _StoreConsumablesRefs(ServiceLocator.ID id, CommandReferences cRef, Command cmd)
    {
        foreach (Thing t in cmd.consumables)
        {
            cRef.consumableRefs.Add(_objIntModel.ClosestThingOfTypeInOOC(id, t.type));
        }
    }

    private void _StoreReagentsRefs(ServiceLocator.ID id, Equipment_Base e, CommandReferences cRef, Command cmd)
    {
        if (cmd.reagents.Length == 0)
            return;
        else
        {
            // For some check the equipment's stowed list
            if (cmd.action == ServiceLocator.Actions.Weld || cmd.action == ServiceLocator.Actions.Power || cmd.action == ServiceLocator.Actions.Charge || cmd.action == ServiceLocator.Actions.Eject || cmd.action == ServiceLocator.Actions.Unstow)
            {
                // For actions that require that reagents be internally present:
                // -Check to see if a given stowed item meets the requirements of the cmd
                // -Then make sure that the ref for the cmd doesn't already contain a redundant one
                // -If that's good, then add the given stowed item to the refs
                for (int i = 0; i < e.stowed.Count; i++)
                {
                    for (int j = 0; j < cmd.reagents.Length; j++)
                    {
                        if (e.stowed[i].type == cmd.reagents[j].reagentType.type)
                        {
                            List<ServiceLocator.Interactives> storedReferenceTypes = new List<ServiceLocator.Interactives>();
                            foreach(Item_Base iB in cRef.reagentRefs)
                            {
                                storedReferenceTypes.Add(iB.type);
                            }

                            if (!storedReferenceTypes.Contains(cmd.reagents[j].reagentType.type))
                            {
                                cRef.reagentRefs.Add(e.stowed[i] as Item_Base);
                            }
                        }
                    }
                }
            }
            // For others, check the area
            else
            {
                foreach(Reagent r in cmd.reagents)
                {
                    cRef.reagentRefs.Add(_objIntModel.ClosestThingOfTypeInOOC(id, r.reagentType.type) as Item_Base);
                }
            }
        }
    }

    private void _StoreTargetRefs(ServiceLocator.ID id, CommandReferences cRef, Command cmd)
    {
        cRef.target = _objIntModel.ClosestThingOfTypeInOOC(id, cmd.target);
    }

    #endregion
}
