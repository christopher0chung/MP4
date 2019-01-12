using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MP4_EventManager
{
    //---------------------
    // // Creates singleton for ease of access
    // Access relocated to ServiceLocator;
    //---------------------

    //static private MP4_EventManager _instance;
    //static public MP4_EventManager instance
    //{
    //    get
    //    {
    //        if (_instance == null)
    //            return _instance = new MP4_EventManager();
    //        else
    //            return _instance;
    //    }
    //}

    //---------------------
    // Storage of Events
    //---------------------

    private Dictionary<Type, MP4_Event.Handler> registeredHandlers = new Dictionary<Type, MP4_Event.Handler>();

    //---------------------
    // Register and Unregister
    //---------------------

    public void Register<T>(MP4_Event.Handler handler) where T : MP4_Event
    {
        Type type = typeof(T);
        if (registeredHandlers.ContainsKey(type))
        {
            registeredHandlers[type] += handler;
        }
        else
        {
            registeredHandlers[type] = handler;
        }
    }

    public void Unregister<T>(MP4_Event.Handler handler) where T : MP4_Event
    {
        Type type = typeof(T);
        MP4_Event.Handler handlers;
        if (registeredHandlers.TryGetValue(type, out handlers))
        {
            handlers -= handler;
            if (handlers == null)
            {
                registeredHandlers.Remove(type);
            }
            else
            {
                registeredHandlers[type] = handlers;
            }
        }
    }

    //---------------------
    // Call event
    //---------------------

    public void Fire(MP4_Event e)
    {
        Type type = e.GetType();
        MP4_Event.Handler handlers;
        if (registeredHandlers.TryGetValue(type, out handlers))
        {
            handlers(e);
        }
    }

    public void ClearEventManagerRegistrations()
    {
        registeredHandlers.Clear();
    }
}