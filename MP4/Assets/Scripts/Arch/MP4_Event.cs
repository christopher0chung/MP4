using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MP4_Event
{
    public delegate void Handler(MP4_Event e);
}

public class Event_NewInteractable : MP4_Event
{
    public Thing data;
    public Vector3 pos;
    public Event_NewInteractable(Thing newData, Vector3 startingLocation)
    {
        data = newData;
        pos = startingLocation;
    }
}
