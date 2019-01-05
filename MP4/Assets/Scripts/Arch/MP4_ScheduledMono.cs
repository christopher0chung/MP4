using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MP4_ScheduledMono : MonoBehaviour {

    public virtual void Awake()
    {
        //Derived classes must set a priority first

        Debug.Assert(priority != 0, "Priority not set for " + this.GetType().ToString() + ". Set priority before override Awake.");

        ServiceLocator.Instance.Application.GetComponent<MP4_Application>().RegisterToSchedule(this, priority);
    }

    public int priority;
    //1000 - Inputs
    //2000 - Game Logic
    //3000 - View Update

	public virtual void S_Update () { }

    public virtual void S_PauseableUpdate () { }
}
