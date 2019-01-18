using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelMenuEqpt : MonoBehaviour {

    public Equipment_Base p0eqpt;
    public Equipment_Base p1eqpt;

    public Dictionary<Command, CommandReferences> p0EqptCommandsActive = new Dictionary<Command, CommandReferences>();
    public Dictionary<Command, CommandReferences> p1EqptCommandsActive = new Dictionary<Command, CommandReferences>();

    public Dictionary<int, Command> p0EqptCommandSeq = new Dictionary<int, Command>();
    public Dictionary<int, Command> p1EqptCommandSeq = new Dictionary<int, Command>();

    public void SetCommands (ServiceLocator.ID id, Equipment_Base eqpt, Dictionary<Command, CommandReferences> refsDict, Dictionary<int, Command> cmdSeq)
    {
        if (id == ServiceLocator.ID.p0)
        {
            p0EqptCommandsActive = refsDict;
            p0EqptCommandSeq = cmdSeq;
            p0eqpt = eqpt;
        }
        else
        {
            p1EqptCommandsActive = refsDict;
            p1EqptCommandSeq = cmdSeq;
            p1eqpt = eqpt;
        }
    }
}
