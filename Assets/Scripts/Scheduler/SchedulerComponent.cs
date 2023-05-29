using Sirenix.OdinInspector;
using UnityEngine;

public class SchedulerComponent : MonoBehaviour
{
    // TODO property drawer to view queued & scheduled tasks properly
    [DisplayAsString][ShowInInspector]
    public bool HasPendingTasks => Scheduler.HasPendingTasks;
    [DisplayAsString][ShowInInspector]
    public int TotalTasksRun => Scheduler.TotalTasksRun;
    [DisplayAsString][ShowInInspector]
    public int TotalPendingTasks => Scheduler.TotalPendingTasks;
        
    private void Update()
    {
        Scheduler.Update();
        Scheduler.QueueTasks();
    }
}