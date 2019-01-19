using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerMenuEqpt : MP4_ScheduledMono {

    #region Properties and References
    private ModelGame _gameModel;
    private ModelObjectInteraction _objIntModel;
    private ModelMenuEqpt _eqptMenuModel;
    private ControllerObjectInteraction _objIntCtrlr;

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
                    _gameModel.EqptMenuSelect_P0 = 0;
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

    #endregion

    #region Sequence and Structure

    public override void Awake()
    {
        priority = 2998;
        base.Awake();

        _gameModel = ServiceLocator.Instance.Model.GetComponent<ModelGame>();
        _objIntModel = ServiceLocator.Instance.Model.GetComponent<ModelObjectInteraction>();
        _eqptMenuModel = ServiceLocator.Instance.Model.GetComponent<ModelMenuEqpt>();
        _objIntCtrlr = ServiceLocator.Instance.Controller.GetComponent<ControllerObjectInteraction>();

        _canvas = ServiceLocator.Instance.View.Find("Canvas");

        _p0_MenuParent = _canvas.Find("P0 Menu Parent");
        _p1_MenuParent = _canvas.Find("P1 Menu Parent");

        Debug.Assert(_p0_MenuParent != null);
        Debug.Assert(_p1_MenuParent != null);
    }

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
            if (_gameModel.EqptMenuSelect_P0 < 0)
                _gameModel.EqptMenuSelect_P0 = _eqptMenuModel.p0EqptCommandsActive.Count - 1;
            if (_gameModel.EqptMenuSelect_P0 >= _eqptMenuModel.p0EqptCommandsActive.Count)
                _gameModel.EqptMenuSelect_P0 = 0;
        }
    }

    private void _CursorUpdate_P1()
    {
        if (_p1_EqptMenu_Active)
        {
            if (_gameModel.EqptMenuSelect_P1 < 0)
                _gameModel.EqptMenuSelect_P1 = _eqptMenuModel.p1EqptCommandsActive.Count - 1;
            if (_gameModel.EqptMenuSelect_P1 >= _eqptMenuModel.p1EqptCommandsActive.Count)
                _gameModel.EqptMenuSelect_P1 = 0;
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
            Debug.Assert(_objIntModel.p0_InteractableInterested.cat == ServiceLocator.ThingCategory.Equipment ||
                _objIntModel.p0_InteractableInterested.cat == ServiceLocator.ThingCategory.Stations,
                "Attempting to parse menu for command-less interactiveCategory");

            if (_objIntModel.p0_InteractableInterested.cat == ServiceLocator.ThingCategory.Equipment)
            {
                //Debug.Log("ControllerMenuEqpt in the equipment track");
                Equipment_Base e = _objIntModel.p0_InteractableInterested as Equipment_Base;
                if (e != null)
                {
                    //Debug.Log(this.GetType().ToString() + " identified as eqpt");
                    Dictionary<Command, CommandReferences> commandDict = new Dictionary<Command, CommandReferences>();
                    Dictionary<int, Command> seqDict = new Dictionary<int, Command>();

                    for (int i = 0; i < e.possibleCommands.Length; i++)
                    {
                        Debug.Log(this.GetType().ToString() + " in the for loop. possiblecommands length is " + e.possibleCommands.Length + ".");

                        bool commandPermissible = false;

                        if (_ReqdReagentConditionsMet(id, e, e.possibleCommands[i]) && 
                            _ReqdConsumablesPresent(id, e.possibleCommands[i]) && 
                            _IntendedTargetPresent(id, e, e.possibleCommands[i]) &&
                            _SpecialConditions(id, e, e.possibleCommands[i]))
                        {
                            commandPermissible = true;
                            Debug.Log("Command is permissible");
                        }

                        if (commandPermissible)
                        {
                            CommandReferences cRef = ScriptableObject.Instantiate<CommandReferences>(Resources.Load<CommandReferences>("CommandReference/CommandReference"));
                            _StoreConsumablesRefs(id, cRef, e.possibleCommands[i]);
                            _StoreReagentsRefs(id, e, cRef, e.possibleCommands[i]);
                            _StoreTargetRefs(id, e, cRef, e.possibleCommands[i]);
                            Debug.Log("CHecking if CREF EXISTS?!?!?!? " + cRef.target.ToString());

                            commandDict.Add(e.possibleCommands[i], cRef);
                            seqDict.Add(seqDict.Count, e.possibleCommands[i]);
                        }
                    }

                    //Debug.Log("SeqDict count is " + seqDict.Count);
                    Command cancel = Resources.Load<Command>("Command/ExitMenu");
                    //Debug.Log(cancel.name);

                    CommandReferences cancelCRef = ScriptableObject.Instantiate(Resources.Load<CommandReferences>("CommandReference/CommandReference"));
                    commandDict.Add(cancel, cancelCRef);
                    //Debug.Log("SeqDict count is " + seqDict.Count);

                    seqDict.Add(seqDict.Count, cancel);
                    //Debug.Log("SeqDict count is " + seqDict.Count);

                    //Debug.Log(commandDict.Count + " " + seqDict.Count);

                    //Debug.Log("Reagent Refs has :" + cRef.reagentRefs.Count);
                    _eqptMenuModel.SetCommands(id, e, commandDict, seqDict);
                }
            }
            else if (_objIntModel.p0_InteractableInterested.cat == ServiceLocator.ThingCategory.Stations)
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
            List<ServiceLocator.ThingType> typesStowed = new List<ServiceLocator.ThingType>();

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
            List<ServiceLocator.ThingType> typesPresent = new List<ServiceLocator.ThingType>();

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
            List<ServiceLocator.ThingType> _stowedTypes = new List<ServiceLocator.ThingType>();
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

    private bool _SpecialConditions(ServiceLocator.ID id, Equipment_Base e, Command cmd)
    {
        if (cmd.action == ServiceLocator.Actions.Install)
        {
            List<ServiceLocator.ThingType> installedTypes = new List<ServiceLocator.ThingType>();
            for (int i = 0; i < e.stowed.Count; i++)
                installedTypes.Add(e.stowed[i].type);
            if (installedTypes.Contains(cmd.target))
                return false;
            else return true;
        }
        else
        {
            return true;
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
                //    For actions that require that reagents be internally present:
                //    -Check to see if a given stowed item meets the requirements of the cmd
                //    - Then make sure that the ref for the cmd doesn't already contain a redundant one
                //     - If that's good, then add the given stowed item to the refs

                for (int i = 0; i < e.stowed.Count; i++)
                {
                    for (int j = 0; j < cmd.reagents.Length; j++)
                    {
                        if (e.stowed[i].type == cmd.reagents[j].reagentType.type)
                        {
                            List<ServiceLocator.ThingType> storedReferenceTypes = new List<ServiceLocator.ThingType>();
                            foreach (Item_Base iB in cRef.reagentRefs)
                            {
                                storedReferenceTypes.Add(iB.type);
                            }

                            if (!storedReferenceTypes.Contains(cmd.reagents[j].reagentType.type))
                            {
                                Debug.Log("Item of type of reagent specified type stored. Item is " + e.stowed[i].name.ToString());
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

    private void _StoreTargetRefs(ServiceLocator.ID id, Equipment_Base e, CommandReferences cRef, Command cmd)
    {
        if (cmd.action == ServiceLocator.Actions.Charge || cmd.action == ServiceLocator.Actions.Eject || cmd.action == ServiceLocator.Actions.Unstow)
        {
            foreach (Thing t in e.stowed)
            {
                Item_Base iB = t as Item_Base;
                Equipment_Base eB = t as Equipment_Base;

                if (iB != null)
                {
                    if (iB.type == cmd.target)
                    {
                        Debug.Log("CREF!!! --- " + cmd.target.ToString() + " should match with " + iB.type.ToString());

                        cRef.target = iB;

                        Debug.Log("CREF!!! --- " + cRef.target.ToString());
                        break;
                    }
                }
                else if (eB != null)
                {
                    if (eB.type == cmd.target)
                    {
                        Debug.Log("CREF!!! --- " + cmd.target.ToString() + " should match with " + eB.type.ToString());

                        cRef.target = eB;

                        Debug.Log("CREF!!! --- " + cRef.target.ToString());
                        break;
                    }
                }
            }
        }
        else
            cRef.target = _objIntModel.ClosestThingOfTypeInOOC(id, cmd.target);
    }

    #endregion

    #region External Asynchronous Functions

    public void ExecuteCommand(ServiceLocator.ID id)
    {
        //------------------------------
        //Item resource consumption should be relocated to objInteractionController
        //------------------------------
        if (id == ServiceLocator.ID.p0)
        {
            Command cmd;
            _eqptMenuModel.p0EqptCommandSeq.TryGetValue(_gameModel.EqptMenuSelect_P0, out cmd);

            //foreach (int sd in _eqptMenuModel.p0EqptCommandSeq.Keys)
            //{
            //    Debug.Log(sd);
            //    Command asdasd;
            //    _eqptMenuModel.p0EqptCommandSeq.TryGetValue(sd, out asdasd);
            //    Debug.Log(asdasd.target.ToString());

            //    CommandReferences ddd;
            //    _eqptMenuModel.p0EqptCommandsActive.TryGetValue(asdasd, out ddd);
            //    Debug.Log(ddd.name);
            //    if (ddd.target != null)
            //        Debug.Log("YAAAAAAAAAAAAAAAAAAAAAAAAY NOT NULL");
            //}

            CommandReferences cRef;
            _eqptMenuModel.p0EqptCommandsActive.TryGetValue(cmd, out cRef);

            if (cmd.action == ServiceLocator.Actions.Install)
            {
                _objIntCtrlr.StowAndInstall(_eqptMenuModel.p0eqpt, cRef.target);
            }
            else if (cmd.action == ServiceLocator.Actions.Stow)
            {
                _objIntCtrlr.StowAndInstall(_eqptMenuModel.p0eqpt, cRef.target);
            }
            else if (cmd.action == ServiceLocator.Actions.Eject)
            {
                //Debug.Log(cRef.target.name);
                _objIntCtrlr.Eject(_eqptMenuModel.p0eqpt, cRef.target);

                if (cmd.reagents.Length > 0)
                {
                    foreach (Reagent r in cmd.reagents)
                    {
                        foreach (Item_Base i in cRef.reagentRefs)
                        {
                            if (r.reagentType.type == i.type)
                            {
                                i.capacity += r.discreteActionCost;
                                i.capacity += r.continuousCostPerSecond * Time.deltaTime;
                            }
                        }
                    }
                }
            }
            else if (cmd.action == ServiceLocator.Actions.Unstow)
            {
                _objIntCtrlr.Unstow(_eqptMenuModel.p0eqpt, cRef.target);

                if (cmd.reagents.Length > 0)
                {
                    foreach (Reagent r in cmd.reagents)
                    {
                        foreach (Item_Base i in cRef.reagentRefs)
                        {
                            if (r.reagentType.type == i.type)
                            {
                                i.capacity += r.discreteActionCost;
                                i.capacity += r.continuousCostPerSecond * Time.deltaTime;
                            }
                        }
                    }
                }
            }
            else if (cmd.action == ServiceLocator.Actions.Exit)
            {
                _gameModel.SetControlState(ServiceLocator.ID.p0, ServiceLocator.ControlStates.Free);
            }
        }

        _gameModel.SetControlState(id, ServiceLocator.ControlStates.Free);
    }

    #endregion
}
