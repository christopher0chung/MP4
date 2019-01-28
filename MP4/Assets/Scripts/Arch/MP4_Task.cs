using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

public class Task_ObjIntViaMenu_Base : MP4_Task
{
    public Task_ObjIntViaMenu_Base(TaskType type, MP4_Task queuedTask) : base(type, queuedTask) { }

    protected ControllerObjectInteraction objIntCtrlr;
    protected ModelGame gameModel;
    protected ModelInput inputModel;

    protected Transform canvas;
    protected GameObject imageObject;
    protected Image imageComponent;
    protected Camera finalCam;

    #region Required
    public override void Initialize()
    {
        objIntCtrlr = ServiceLocator.Instance.Controller.GetComponent<ControllerObjectInteraction>();
        gameModel = ServiceLocator.Instance.Model.GetComponent<ModelGame>();
        inputModel = ServiceLocator.Instance.Model.GetComponent<ModelInput>();
    }

    public override void OnEnter()
    {
        throw new System.NotImplementedException();
    }

    public override void Running()
    {
        throw new System.NotImplementedException();
    }

    public override void OnSuccess()
    {
        throw new System.NotImplementedException();
    }

    public override void OnFail()
    {
        throw new System.NotImplementedException();
    }

    public override void Cleanup()
    {
        throw new System.NotImplementedException();
    }

    public override bool SuccessTest()
    {
        throw new System.NotImplementedException();
    }

    public override bool FailureTest()
    {
        throw new System.NotImplementedException();
    }
    #endregion

    #region Common Functions
    public void ConsumeDiscrete(Command cmd, CommandReferences cRef)
    {
        if (cmd.reagents.Length > 0)
        {
            foreach (Reagent r in cmd.reagents)
            {
                foreach (Item_Base i in cRef.reagentRefs)
                {
                    if (r.reagentType.type == i.type)
                    {
                        i.capacity += r.discreteActionCost;
                    }
                }
            }
        }
    }

    public void ConsumeContinuous(Command cmd, CommandReferences cRef)
    {
        if (cmd.reagents.Length > 0)
        {
            foreach (Reagent r in cmd.reagents)
            {
                foreach (Item_Base i in cRef.reagentRefs)
                {
                    if (r.reagentType.type == i.type)
                    {
                        i.capacity += r.continuousCostPerSecond * Time.deltaTime;
                    }
                }
            }
        }
    }

    public Vector3 GetCanvasPosForGO(Transform xfm, Camera cam)
    {
        return (cam.WorldToScreenPoint(xfm.position) - Vector3.up * Screen.height / 2 - Vector3.right * Screen.width / 2) / ServiceLocator.Instance.View.Find("Canvas").localScale.x;
    }

    public void MakeProgressBarAndSetRefereces()
    {
        finalCam = ServiceLocator.Instance.View.Find("Cameras").Find("Final Render Camera").GetComponent<Camera>();

        canvas = ServiceLocator.Instance.View.Find("Canvas");
        imageObject = new GameObject("Progress Bar");
        imageObject.transform.SetParent(canvas);

        imageComponent = imageObject.AddComponent<Image>();
        imageComponent.sprite = Resources.Load<Sprite>("Textures/ProgressRing");
        Debug.Assert(imageComponent.sprite != null, "No sprite loaded");
        imageComponent.type = Image.Type.Filled;
        imageComponent.fillMethod = Image.FillMethod.Radial360;
        imageComponent.color = Color.green;
    }

    public void UpdateProgressBar(Transform playerXfm, float timer)
    {
        imageObject.GetComponent<RectTransform>().localPosition = GetCanvasPosForGO(playerXfm, finalCam);
        imageObject.GetComponent<RectTransform>().localScale = Vector3.one;

        imageComponent.fillAmount = timer;
    }

    public void DestroyProgressBar()
    {
        GameObject.Destroy(imageObject);
    }
    #endregion
}

public class Task_EjectThing : Task_ObjIntViaMenu_Base
{
    private ServiceLocator.ID playerID;
    private Equipment_Base ejector;
    private Command cmd;
    private CommandReferences cRef;
    private Transform playerXfm;

    public Task_EjectThing(MP4_Task queuedTask, ServiceLocator.ID id, Equipment_Base ejector, Command cmd, CommandReferences cRef) : base(TaskType.Scheduled_Pauseable, queuedTask)
    {
        playerID = id;
        this.ejector = ejector;
        this.cmd = cmd;
        this.cRef = cRef;
    }

    public override void Initialize()
    {
        base.Initialize();

        if (playerID == ServiceLocator.ID.p0)
            playerXfm = ServiceLocator.Instance.Character0;
        else
            playerXfm = ServiceLocator.Instance.Character1;

        MakeProgressBarAndSetRefereces();
    }

    float timer;
    public override void OnEnter()
    {
        timer = 0;
        gameModel.SetControlState(playerID, ServiceLocator.ControlStates.Occupied);
    }

    public override void Running()
    {
        timer += Time.deltaTime / cmd.timeTotal;

        UpdateProgressBar(playerXfm, timer);

        ConsumeContinuous(cmd, cRef);
    }

    public override void OnSuccess()
    {
        ConsumeDiscrete(cmd, cRef);

        objIntCtrlr.Eject(ejector, cRef.target);
    }

    public override void OnFail()
    {
        
    }

    public override void Cleanup()
    {
        gameModel.SetControlState(playerID, ServiceLocator.ControlStates.Free);
        DestroyProgressBar();
    }

    public override bool SuccessTest()
    {
        if (timer >= 1)
            return true;
        else return false;
    }

    public override bool FailureTest()
    {
        if (playerID == ServiceLocator.ID.p0)
        {
            if (inputModel.P0_Grab_IsDown)
                return true;
            else
                return false;
        }
        else
        {
            if (inputModel.P1_Grab_IsDown)
                return true;
            else
                return false;
        }
    }
}

public class Task_InstallThing : Task_ObjIntViaMenu_Base
{
    private ServiceLocator.ID playerID;
    private Equipment_Base installTgt;
    private Command cmd;
    private CommandReferences cRef;
    private Transform playerXfm;

    public Task_InstallThing(MP4_Task queued, ServiceLocator.ID id, Equipment_Base eqpt, Command cmd, CommandReferences cRef) : base (TaskType.Scheduled_Pauseable, queued)
    {
        playerID = id;
        this.installTgt = eqpt;
        this.cmd = cmd;
        this.cRef = cRef;
    }

    public override void Initialize()
    {
        base.Initialize();

        if (playerID == ServiceLocator.ID.p0)
            playerXfm = ServiceLocator.Instance.Character0;
        else
            playerXfm = ServiceLocator.Instance.Character1;

        MakeProgressBarAndSetRefereces();
    }

    float timer;
    public override void OnEnter()
    {
        timer = 0;
        gameModel.SetControlState(playerID, ServiceLocator.ControlStates.Occupied);
    }

    public override void Running()
    {
        timer += Time.deltaTime / cmd.timeTotal;

        UpdateProgressBar(playerXfm, timer);

        ConsumeContinuous(cmd, cRef);
    }

    public override void OnSuccess()
    {
        ConsumeDiscrete(cmd, cRef);

        objIntCtrlr.StowAndInstall(installTgt, cRef.target);
    }

    public override void OnFail()
    {

    }

    public override void Cleanup()
    {
        gameModel.SetControlState(playerID, ServiceLocator.ControlStates.Free);
        DestroyProgressBar();
    }

    public override bool SuccessTest()
    {
        if (timer >= 1)
            return true;
        else return false;
    }

    public override bool FailureTest()
    {
        if (playerID == ServiceLocator.ID.p0)
        {
            if (inputModel.P0_Grab_IsDown)
                return true;
            else
                return false;
        }
        else
        {
            if (inputModel.P1_Grab_IsDown)
                return true;
            else
                return false;
        }
    }
}

public class Task_StowThing : Task_ObjIntViaMenu_Base
{
    private ServiceLocator.ID playerID;
    private Equipment_Base stowTgt;
    private Command cmd;
    private CommandReferences cRef;
    private Transform playerXfm;

    public Task_StowThing(MP4_Task queued, ServiceLocator.ID id, Equipment_Base eqpt, Command cmd, CommandReferences cRef) : base(TaskType.Scheduled_Pauseable, queued)
    {
        playerID = id;
        this.stowTgt = eqpt;
        this.cmd = cmd;
        this.cRef = cRef;
    }

    public override void Initialize()
    {
        base.Initialize();

        if (playerID == ServiceLocator.ID.p0)
            playerXfm = ServiceLocator.Instance.Character0;
        else
            playerXfm = ServiceLocator.Instance.Character1;

        MakeProgressBarAndSetRefereces();
    }

    float timer;
    public override void OnEnter()
    {
        timer = 0;
        gameModel.SetControlState(playerID, ServiceLocator.ControlStates.Occupied);
    }

    public override void Running()
    {
        timer += Time.deltaTime / cmd.timeTotal;

        UpdateProgressBar(playerXfm, timer);
    }

    public override void OnSuccess()
    {
        objIntCtrlr.StowAndInstall(stowTgt, cRef.target);
    }

    public override void OnFail()
    {

    }

    public override void Cleanup()
    {
        gameModel.SetControlState(playerID, ServiceLocator.ControlStates.Free);
        DestroyProgressBar();
    }

    public override bool SuccessTest()
    {
        if (timer >= 1)
            return true;
        else return false;
    }

    public override bool FailureTest()
    {
        if (playerID == ServiceLocator.ID.p0)
        {
            if (inputModel.P0_Grab_IsDown)
                return true;
            else
                return false;
        }
        else
        {
            if (inputModel.P1_Grab_IsDown)
                return true;
            else
                return false;
        }
    }
}

public class Task_UnstowThing : Task_ObjIntViaMenu_Base
{
    private ServiceLocator.ID playerID;
    private Equipment_Base installer;
    private Command cmd;
    private CommandReferences cRef;
    private Transform playerXfm;

    public Task_UnstowThing(MP4_Task queuedTask, ServiceLocator.ID id, Equipment_Base eqpt, Command cmd, CommandReferences cRef) : base(TaskType.Scheduled_Pauseable, queuedTask)
    {
        playerID = id;
        this.installer = eqpt;
        this.cmd = cmd;
        this.cRef = cRef;
    }

    public override void Initialize()
    {
        base.Initialize();

        if (playerID == ServiceLocator.ID.p0)
            playerXfm = ServiceLocator.Instance.Character0;
        else
            playerXfm = ServiceLocator.Instance.Character1;

        MakeProgressBarAndSetRefereces();
    }

    float timer;
    public override void OnEnter()
    {
        timer = 0;
        gameModel.SetControlState(playerID, ServiceLocator.ControlStates.Occupied);
    }

    public override void Running()
    {
        timer += Time.deltaTime / cmd.timeTotal;

        UpdateProgressBar(playerXfm, timer);

        ConsumeContinuous(cmd, cRef);
    }

    public override void OnSuccess()
    {
        ConsumeDiscrete(cmd, cRef);

        objIntCtrlr.Unstow(installer, cRef.target);
    }

    public override void OnFail()
    {

    }

    public override void Cleanup()
    {
        gameModel.SetControlState(playerID, ServiceLocator.ControlStates.Free);
        DestroyProgressBar();
    }

    public override bool SuccessTest()
    {
        if (timer >= 1)
            return true;
        else return false;
    }

    public override bool FailureTest()
    {
        if (playerID == ServiceLocator.ID.p0)
        {
            if (inputModel.P0_Grab_IsDown)
                return true;
            else
                return false;
        }
        else
        {
            if (inputModel.P1_Grab_IsDown)
                return true;
            else
                return false;
        }
    }
}

