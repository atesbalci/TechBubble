using System.Collections.Generic;

namespace TechBubble.Behaviors
{
    public delegate void DeadlineEvent(DeadlineBehavior deadlineBehavior);
    
    public interface IDeadlinesProvider
    {
        event DeadlineEvent OnDeadlineCreated;
        event DeadlineEvent OnDeadlineReached;
        event DeadlineEvent OnDeadlineFailed;
    }
}