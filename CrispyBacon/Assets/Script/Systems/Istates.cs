public interface IStates
{
    public string StateName
    {
        get;
        set;
    }

    void Enter();
    void IfStateChange();
    void StateUpdate();
    void Exit();
}