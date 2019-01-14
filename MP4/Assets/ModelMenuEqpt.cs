using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelMenuEqpt : MonoBehaviour {

    public Dictionary<Command, CommandReferences> p0EqptCommandsActive = new Dictionary<Command, CommandReferences>();
    public Dictionary<Command, CommandReferences> p1EqptCommandsActive = new Dictionary<Command, CommandReferences>();

    public void SetCommands (ServiceLocator.ID id, Dictionary<Command, CommandReferences> refsDict)
    {
        if (id == ServiceLocator.ID.p0)
            p0EqptCommandsActive = refsDict;
        else
            p1EqptCommandsActive = refsDict;
    }
}
