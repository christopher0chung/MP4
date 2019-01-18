using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thing : ScriptableObject {
    public ServiceLocator.ThingType type;
    public ServiceLocator.ThingCategory cat;
    public bool highlighted;
}
