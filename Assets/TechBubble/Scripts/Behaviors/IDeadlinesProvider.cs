using System.Collections.Generic;

namespace TechBubble.Behaviors
{
    public interface IDeadlinesProvider
    {
        public delegate void DeadlineEvent(DeadlineBehavior deadlineBehavior);
        
        event DeadlineEvent OnDeadlineCreated;
        event DeadlineEvent OnDeadlineReached;
        event DeadlineEvent OnDeadlineFailed;
    }
}