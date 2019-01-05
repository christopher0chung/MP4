using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MP4_Application : MonoBehaviour {

    private List<MP4_ScheduledMono> _schedule = new List<MP4_ScheduledMono>();

    public void RegisterToSchedule(MP4_ScheduledMono registerer, int priority)
    {
        //Debug.Log("Registering at count: " + _schedule.Count + ".");
        if (_schedule.Count == 0)
        {
            _schedule.Add(registerer);
            //Debug.Log("CODE0: Schedule count is now: " + _schedule.Count + ".");
        }
        else if (_schedule.Count == 1)
        {
            if (priority >= _schedule[0].priority)
                _schedule.Add(registerer);
            else
                _schedule.Insert(0, registerer);

            //Debug.Log("CODE1: Schedule count is now: " + _schedule.Count + ".");
        }
        else
        {
            //Debug.Log("Attempt else case");
            for (int i = _schedule.Count - 1; i >= 0; i--)
            {
                if (priority >= _schedule[i].priority)
                {
                    _schedule.Insert(i + 1, registerer);
                    //Debug.Log("CODE2: Schedule count is now: " + _schedule.Count + ".");

                    //Debug.Log("Registered new mono. Mono's priority is " + priority + ". Mono is scheduled after mono of priority " + _schedule[i].priority + ".");
                    return;
                }
            }
            _schedule.Insert(0, registerer);
            //Debug.Log("CODE3: Schedule count is now: " + _schedule.Count + ".");
        }
    }

	void Update () {
		for (int i = 0; i < _schedule.Count; i++)
        {
            _schedule[i].S_PauseableUpdate();
            _schedule[i].S_Update();
            //Debug.Log(_schedule.Count);
        }
	}
}
