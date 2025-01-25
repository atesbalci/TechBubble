using System.Collections.Generic;

namespace TechBubble.Behaviors
{
    public interface IDeadlinesProvider
    {
        ICollection<DeadlineBehavior> Deadlines { get; }
    }
}