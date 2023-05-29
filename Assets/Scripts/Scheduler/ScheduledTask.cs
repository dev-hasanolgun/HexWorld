using System;

public class ScheduledTask
{
    public Action Task;
    public float ElapsedTime;
    public float ExecutionTime;
    public int Loops;
    public float LoopInterval;
    
    public RunState State { get; private set; }
    public bool Completed => State == RunState.Complete;
    public bool Cancelled => State == RunState.Cancelled;

    public ScheduledTask(Action task, float executionTime)
    {
        Task = task;
        ExecutionTime = executionTime;
        State = RunState.Waiting;
    }

    public void Run()
    {
        if (Cancelled) throw new InvalidOperationException("Can not run a ScheduledTask that has been Cancelled.");
        if (Completed) throw new InvalidOperationException("Can not run a ScheduledTask that has been already Completed.");
        State = RunState.Running;
        Task.Invoke();
        State = Loops > 0 ? RunState.Waiting : RunState.Complete;
    }
    
    public void Cancel()
    {
        State = RunState.Cancelled;
    }

    public void SetLoops(int loops, float loopInterval)
    {
        Loops = loops;
        LoopInterval = loopInterval;
    }
    
    public override string ToString()
    {
        return "method " + Task?.Method + 
               " targeting " + Task?.Target +
               " executing in " + (ExecutionTime - ElapsedTime) + 
               " remaining loops " + Loops + " with loop interval " + LoopInterval;
    }

    public enum RunState
    {
        Waiting,
        Running,
        Complete,
        Cancelled
    }
}