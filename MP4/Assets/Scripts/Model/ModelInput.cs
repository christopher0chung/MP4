using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelInput : MonoBehaviour {

    // -- P0 ---------------------------------------------------------------------

    public bool P0_Up_OnDown;
    public bool P0_Up_IsDown;
    public bool P0_Up_OnUp;

    public bool P0_Down_OnDown;
    public bool P0_Down_IsDown;
    public bool P0_Down_OnUp;

    public bool P0_Left_OnDown;
    public bool P0_Left_IsDown;
    public bool P0_Left_OnUp;

    public bool P0_Right_OnDown;
    public bool P0_Right_IsDown;
    public bool P0_Right_OnUp;

    public bool P0_Use_OnDown;
    public bool P0_Use_IsDown;
    public bool P0_Use_OnUp;

    public bool P0_Grab_OnDown;
    public bool P0_Grab_IsDown;
    public bool P0_Grab_OnUp;

    // -- P1 ---------------------------------------------------------------------

    public bool P1_Up_OnDown;
    public bool P1_Up_IsDown;
    public bool P1_Up_OnUp;

    public bool P1_Down_OnDown;
    public bool P1_Down_IsDown;
    public bool P1_Down_OnUp;

    public bool P1_Left_OnDown;
    public bool P1_Left_IsDown;
    public bool P1_Left_OnUp;

    public bool P1_Right_OnDown;
    public bool P1_Right_IsDown;
    public bool P1_Right_OnUp;

    public bool P1_Use_OnDown;
    public bool P1_Use_IsDown;
    public bool P1_Use_OnUp;

    public bool P1_Grab_OnDown;
    public bool P1_Grab_IsDown;
    public bool P1_Grab_OnUp;

    //// -- P0 ---------------------------------------------------------------------

    //public bool P0_Up_OnDown { get; private set; }
    //public bool P0_Up_IsDown { get; private set; }
    //public bool P0_Up_OnUp { get; private set; }

    //public bool P0_Down_OnDown { get; private set; }
    //public bool P0_Down_IsDown { get; private set; }
    //public bool P0_Down_OnUp { get; private set; }

    //public bool P0_Left_OnDown { get; private set; }
    //public bool P0_Left_IsDown { get; private set; }
    //public bool P0_Left_OnUp { get; private set; }

    //public bool P0_Right_OnDown { get; private set; }
    //public bool P0_Right_IsDown { get; private set; }
    //public bool P0_Right_OnUp { get; private set; }

    //public bool P0_Use_OnDown { get; private set; }
    //public bool P0_Use_IsDown { get; private set; }
    //public bool P0_Use_OnUp { get; private set; }

    //public bool P0_Grab_OnDown { get; private set; }
    //public bool P0_Grab_IsDown { get; private set; }
    //public bool P0_Grab_OnUp { get; private set; }

    //// -- P1 ---------------------------------------------------------------------

    //public bool P1_Up_OnDown { get; private set; }
    //public bool P1_Up_IsDown { get; private set; }
    //public bool P1_Up_OnUp { get; private set; }

    //public bool P1_Down_OnDown { get; private set; }
    //public bool P1_Down_IsDown { get; private set; }
    //public bool P1_Down_OnUp { get; private set; }

    //public bool P1_Left_OnDown { get; private set; }
    //public bool P1_Left_IsDown { get; private set; }
    //public bool P1_Left_OnUp { get; private set; }

    //public bool P1_Right_OnDown { get; private set; }
    //public bool P1_Right_IsDown { get; private set; }
    //public bool P1_Right_OnUp { get; private set; }

    //public bool P1_Use_OnDown { get; private set; }
    //public bool P1_Use_IsDown { get; private set; }
    //public bool P1_Use_OnUp { get; private set; }

    //public bool P1_Grab_OnDown { get; private set; }
    //public bool P1_Grab_IsDown { get; private set; }
    //public bool P1_Grab_OnUp { get; private set; }

    public void SetInputStates (ServiceLocator.ID id, ServiceLocator.Inputs input, ServiceLocator.InputStates state, bool newVal)
    {
        if (id == ServiceLocator.ID.p0)
        {
            if (input == ServiceLocator.Inputs.Up)
            {
                if (state == ServiceLocator.InputStates.OnDown)
                    P0_Up_OnDown = newVal;
                else if (state == ServiceLocator.InputStates.Down)
                    P0_Up_IsDown = newVal;
                else
                    P0_Up_OnUp = newVal;
            }
            else if (input == ServiceLocator.Inputs.Down)
            {
                if (state == ServiceLocator.InputStates.OnDown)
                    P0_Down_OnDown = newVal;
                else if (state == ServiceLocator.InputStates.Down)
                    P0_Down_IsDown = newVal;
                else
                    P0_Down_OnUp = newVal;
            }
            else if (input == ServiceLocator.Inputs.Left)
            {
                if (state == ServiceLocator.InputStates.OnDown)
                    P0_Left_OnDown = newVal;
                else if (state == ServiceLocator.InputStates.Down)
                    P0_Left_IsDown = newVal;
                else
                    P0_Left_OnUp = newVal;
            }
            else if (input == ServiceLocator.Inputs.Right)
            {
                if (state == ServiceLocator.InputStates.OnDown)
                    P0_Right_OnDown = newVal;
                else if (state == ServiceLocator.InputStates.Down)
                    P0_Right_IsDown = newVal;
                else
                    P0_Right_OnUp = newVal;
            }
            else if (input == ServiceLocator.Inputs.Use)
            {
                if (state == ServiceLocator.InputStates.OnDown)
                    P0_Use_OnDown = newVal;
                else if (state == ServiceLocator.InputStates.Down)
                    P0_Use_IsDown = newVal;
                else
                    P0_Use_OnUp = newVal;
            }
            else if (input == ServiceLocator.Inputs.Grab)
            {
                if (state == ServiceLocator.InputStates.OnDown)
                    P0_Grab_OnDown = newVal;
                else if (state == ServiceLocator.InputStates.Down)
                    P0_Grab_IsDown = newVal;
                else
                    P0_Grab_OnUp = newVal;
            }
        }
        else
        {
            if (input == ServiceLocator.Inputs.Up)
            {
                if (state == ServiceLocator.InputStates.OnDown)
                    P1_Up_OnDown = newVal;
                else if (state == ServiceLocator.InputStates.Down)
                    P1_Up_IsDown = newVal;
                else
                    P1_Up_OnUp = newVal;
            }
            else if (input == ServiceLocator.Inputs.Down)
            {
                if (state == ServiceLocator.InputStates.OnDown)
                    P1_Down_OnDown = newVal;
                else if (state == ServiceLocator.InputStates.Down)
                    P1_Down_IsDown = newVal;
                else
                    P1_Down_OnUp = newVal;
            }
            else if (input == ServiceLocator.Inputs.Left)
            {
                if (state == ServiceLocator.InputStates.OnDown)
                    P1_Left_OnDown = newVal;
                else if (state == ServiceLocator.InputStates.Down)
                    P1_Left_IsDown = newVal;
                else
                    P1_Left_OnUp = newVal;
            }
            else if (input == ServiceLocator.Inputs.Right)
            {
                if (state == ServiceLocator.InputStates.OnDown)
                    P1_Right_OnDown = newVal;
                else if (state == ServiceLocator.InputStates.Down)
                    P1_Right_IsDown = newVal;
                else
                    P1_Right_OnUp = newVal;
            }
            else if (input == ServiceLocator.Inputs.Use)
            {
                if (state == ServiceLocator.InputStates.OnDown)
                    P1_Use_OnDown = newVal;
                else if (state == ServiceLocator.InputStates.Down)
                    P1_Use_IsDown = newVal;
                else
                    P1_Use_OnUp = newVal;
            }
            else if (input == ServiceLocator.Inputs.Grab)
            {
                if (state == ServiceLocator.InputStates.OnDown)
                    P1_Grab_OnDown = newVal;
                else if (state == ServiceLocator.InputStates.Down)
                    P1_Grab_IsDown = newVal;
                else
                    P1_Grab_OnUp = newVal;
            }
        }
    }
}
