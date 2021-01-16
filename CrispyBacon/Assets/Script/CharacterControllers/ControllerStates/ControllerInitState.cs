public class ControllerInitState : IStates
{
    private RiderController user;
    private string stateName;
    public string StateName 
    { 
        get => stateName; 
        set => stateName = value; 
    }

    public ControllerInitState(RiderController _userController)
    {
        user = _userController;
    }

    public void Enter()
    {
        user.controllerStateMachine.ChangeState(user.defaultState);
    }

    public void IfStateChange() {}

    public void StateUpdate() {}

    public void Exit() {}
}
