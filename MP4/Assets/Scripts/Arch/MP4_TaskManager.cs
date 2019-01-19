using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MP4_TaskManager : MP4_ScheduledMono {

    private List<MP4_Task> _taskPool = new List<MP4_Task>();

    public override void Awake()
    {
        priority = 2999;
        base.Awake();
    }

    public void StartTask (MP4_Task task)
    {
        Debug.Assert(task.state == TaskStates.Created, "Attempting to register an old task");
        task.state = TaskStates.Ready;
        _taskPool.Add(task);
    }

    public void Update()
    {
        Helper_IterateAndUpdate(TaskType.Unscheduled);
        Helper_Cleanup();
    }

    public override void S_Update()
    {
        Helper_IterateAndUpdate(TaskType.Scheduled_Unpauseable);
        Helper_Cleanup();
    }

    public override void S_PauseableUpdate()
    {
        Helper_IterateAndUpdate(TaskType.Scheduled_Pauseable);
        Helper_Cleanup();
    }

    private void Helper_IterateAndUpdate(TaskType type)
    {
        if (_taskPool.Count > 0)
        {
            foreach (MP4_Task t in _taskPool)
            {
                if(t.type == type)
                {
                    if (t.state == TaskStates.Ready)
                        t.state = TaskStates.Running;
                    else if (t.state == TaskStates.Running)
                    {
                        t.Running();
                        if (t.SuccessTest())
                            t.state = TaskStates.Success;
                        else if (t.FailureTest())
                            t.state = TaskStates.Failure;
                    }
                    else if (t.state == TaskStates.Success || t.state == TaskStates.Failure)
                        t.state = TaskStates.Cleanup;
                }
            }
        }
    }

    private void Helper_Cleanup()
    {
        for (int i = _taskPool.Count - 1; i >= 0; i--)
        {
            if (_taskPool[i].state == TaskStates.Cleanup)
            {
                if (_taskPool[i].queuedTask != null)
                    StartTask(_taskPool[i].queuedTask);
                _taskPool.Remove(_taskPool[i]);
            }
        }
    }
}

public enum TaskStates { Created, Ready, Running, Success, Failure, Cleanup }
public enum TaskType { Unscheduled, Scheduled_Unpauseable, Scheduled_Pauseable}
// Created - Initial state;
// Ready - A created task is set to ready when registered. Initialization happens when ready.
// Running - A task that was in standby is escalated to running. After OnEnter(), the task's Running(), SuccessTest(), and FailureTest() will run continuously.
// Success - A task that meets the "success" criteria will call OnSuccess().
// Failure - A task that meets the "failure" criteria will call OnFailure().
// Cleanup - Whether a task is a success or failure, both will escalate to Cleanup.  Cleanup() will be called and will be marked for destruction.

