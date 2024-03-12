namespace SpaceBattle.Lib;
using Hwdtech;


public class SoftStopCommand : ICommand
{
    private readonly ServerThread _t;
    private readonly Action _action;
    public SoftStopCommand(ServerThread t, Action action)
    {
        _t = t;
        _action = action;
    }

    public void Execute()
    {
        if (!_t.Equals(Thread.CurrentThread))
            throw new Exception();

        var oldBehaviour = _t.GetBehaviour();

        void newBehaviour()
        {
            if (!_t.QueueIsEmpty)
                oldBehaviour();
            else
                IoC.Resolve<ICommand>("Thread.HardStop", _t, _action).Execute();
        }

        _t.UpdateBehaviour(newBehaviour);
    }
}
