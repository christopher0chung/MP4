using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CommandReference", menuName = "New CommandReference")]
public class CommandReferences : ScriptableObject {

    public List<Item_Base> reagentRefs;
    public List<Thing> consumableRefs;
    public Thing target;

}
