﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Command", menuName = "New Command")]
public class Command : ScriptableObject {

    public Reagent[] reagents = new Reagent[0];
    public Thing[] consumables = new Thing[0];
    public bool playerPresenceRequired;
    public ServiceLocator.Actions action;
    //public ServiceLocator.Condition condition;
    public ServiceLocator.ThingType target;
    public ServiceLocator.ActionType actionType;
    public float timeTotal;
}
