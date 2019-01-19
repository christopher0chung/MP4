using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MP4_Task
{
    private TaskStates _state;
    public TaskStates state
    {
        get
        {
            return _state;
        }
        set
        {
            if (value != state)
            {
                _state = value;

                if (_state == TaskStates.Created) { }
                else if (_state == TaskStates.Ready)
                {
                    Initialize();
                }
                else if (_state == TaskStates.Running)
                {
                    OnEnter();
                }
                else if (_state == TaskStates.Success)
                {
                    OnSuccess();
                }
                else if (_state == TaskStates.Failure)
                {
                    OnFail();
                }
                else if (_state == TaskStates.Cleanup)
                {
                    Cleanup();
                }
            }
        }
    }

    public TaskType type;

    public MP4_Task queuedTask { get; private set; }

    public MP4_Task(TaskType type, MP4_Task queuedTask)
    {
        this.queuedTask = queuedTask;
        this.type = type;
        state = TaskStates.Created;
    }

    public abstract void Initialize();
    public abstract void OnEnter();
    public abstract void Running();
    public abstract void OnSuccess();
    public abstract void OnFail();
    public abstract void Cleanup();

    public abstract bool SuccessTest();
    public abstract bool FailureTest();
}

public class TestTask : MP4_Task
{
    private Vector3 _startPos;
    private Vector3 _endPos;
    private Transform _body;

    public TestTask(MP4_Task queuedTaskPassThrough) : base(TaskType.Unscheduled, queuedTaskPassThrough)
    {

    }

    public override void Initialize()
    {
        Vector2 loc = Random.insideUnitCircle;
        _startPos = new Vector3();
        _startPos.x = loc.x;
        _startPos.y = loc.y + 10;

        GameObject testGO = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/ArTank"), _startPos, Quaternion.identity, null);
        _body = testGO.transform;
    }

    public override void OnEnter()
    {
        _endPos = Random.insideUnitSphere * 5 + Vector3.right;
    }

    public override void Running()
    {
        _body.transform.position = Vector3.MoveTowards(_body.transform.position, _endPos, Time.deltaTime);
    }

    public override void OnSuccess()
    {
        Debug.Log("Success");
    }

    public override void OnFail()
    {
        Debug.Log("Failure");
    }

    public override void Cleanup()
    {
        GameObject.Destroy(_body.gameObject);
        Debug.Log("Clean up");
    }

    public override bool SuccessTest()
    {
        if (Vector3.Distance(_body.transform.position, _endPos) < .01f)
            return true;
        else return false;
    }

    private float timer;

    public override bool FailureTest()
    {
        timer += Time.deltaTime;

        if (timer >= 3)
            return true;
        else return false;
    }
}
