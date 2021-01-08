using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public bool debugEnabled;
    private IStates currentState;
    private IStates previousState;

    /////////////////////////////////////////////////////////////
    ///                     Machine Logic                     ///
    /////////////////////////////////////////////////////////////

    /// <summary>Change the current state of the state machine</summary>
    /// <param name="newState">The desired state you wish the FSM to change to.</param>
    public void ChangeState(IStates newState)
    {
        if (currentState != null)
        {
            
            currentState.Exit();
            if (debugEnabled) PrintLog(currentState.StateName, exitState);
            previousState = currentState;
        }
        currentState = newState;
        currentState.Enter();
        if (debugEnabled) PrintLog(currentState.StateName, enterState);
    }

    /// <summary>Contains all logic that verify if current state should be playing it's execute or imediately change to a new state.</summary>
    public void CheckIfStateChange()
    {
        if (currentState != null)
        {
            currentState.IfStateChange();
            if (debugEnabled) PrintLog(currentState.StateName, stateChange);
        }
    }

    /// <summary>Update loop for the states of the state machine.</summary>
    public void CurrentStateUpdate()
    {
        if (currentState != null)
        {
            currentState.StateUpdate();
            if (debugEnabled) PrintLog(currentState.StateName, updateState);
        }
    }

    /// <summary>Revert current state to previous state.</summary>
    public void SwitchToPreviousState()
    {
        currentState.Exit();
        if (debugEnabled) PrintLog(currentState.StateName, exitState);
        currentState = previousState;
        currentState.Enter();
        if (debugEnabled) PrintLog(currentState.StateName, enterState);
    }

    /////////////////////////////////////////////////////////////
    ///                      Debug Logic                      ///
    /////////////////////////////////////////////////////////////

    private readonly string enterState = "<color=yellow>State Enter</color>";
    private readonly string stateChange = "<color=purple>Switching State</color>";
    private readonly string updateState = "<color=blue>State update</color>";
    private readonly string exitState = "<color=yellow>State Exit</color>";

    private void PrintLog(string name, string state)
    {
        Debug.Log($"{name} => {state}");
    }
}
