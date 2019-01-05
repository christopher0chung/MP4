using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Reagent", menuName = "New Reagent")]

public class Reagent : ScriptableObject {
    public Item_Base reagentType;
    public float continuousCostPerSecond;
    public float discreteActionCost;
}
