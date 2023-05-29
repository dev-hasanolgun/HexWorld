using System;
using System.Collections.Generic;
using UnityEngine;

public static class Scheduler
{
    private static readonly Queue<ScheduledTask> s_queuedTasks = new Queue<ScheduledTask>();
    private static readonly List<ScheduledTask> s_scheduledTasks = new List<ScheduledTask>();

    public static bool HasPendingTasks => TotalPendingTasks > 0;
    public static int TotalTasksRun { get; private set; }
    public static int TotalPendingTasks => s_queuedTasks.Count + s_scheduledTasks.Count;
    
    public static void Update()
    {
        if (s_queuedTasks.Count > 0)
        {
            var task = s_queuedTasks.Dequeue();
            task.Loops--;
            task.Run();
            TotalTasksRun++;
            
            if (task.Loops > 0)
            {
                task.ElapsedTime = 0f;
                task.ExecutionTime = task.LoopInterval;
                Schedule(task);
            }
        }
    }
    
    public static void QueueTasks()
    {
        for (int i = 0; i < s_scheduledTasks.Count; i++)
        {
            var scheduledTask = s_scheduledTasks[i];
            if (scheduledTask.ElapsedTime >= scheduledTask.ExecutionTime)
            {
                Enqueue(scheduledTask);
                s_scheduledTasks.RemoveAt(i);
                i--;
            }
            scheduledTask.ElapsedTime += Time.deltaTime;
        }
    }

    public static void CancelAllScheduledTasks()
    {
        for (int i = 0; i < s_scheduledTasks.Count; i++)
        {
            var task = s_scheduledTasks[i];
            task.Cancel();
        }
    }

    public static ScheduledTask AddTask(Action task, float executionTime)
    {
        var scheduledTask = new ScheduledTask(task, executionTime);
        Schedule(scheduledTask);
        
        return scheduledTask;
    }
    
    private static void Schedule(ScheduledTask task)
    {
        if (task.Completed) throw new InvalidOperationException("Can not add a ScheduledTask that has been already Completed");
        if (task.Cancelled) throw new InvalidOperationException("Can not add a ScheduledTask that has been already Cancelled");
        s_scheduledTasks.Add(task);
        if (s_scheduledTasks.Count % 1000 != 0) return;

        Debug.Log("Scheduler has " + s_scheduledTasks.Count + " scheduled tasks pending");
        Debug.Log("-First task: " + s_scheduledTasks[0]);
        Debug.Log("-Last task: " + s_scheduledTasks[^0]);
    }

    private static void Enqueue(ScheduledTask task)
    {
        s_queuedTasks.Enqueue(task);
        
        if (s_queuedTasks.Count % 1000 != 0) return;

        Debug.Log("Scheduler has " + s_queuedTasks.Count + " queued tasks pending");
    }
    
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void OnRuntimeMethodLoad()
    {
        var schedulerComponent = new GameObject("[Scheduler]");
        schedulerComponent.AddComponent<SchedulerComponent>();
    }
}