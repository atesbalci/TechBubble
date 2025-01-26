using System.Threading;
using System.Threading.Tasks;

namespace TechBubble.Views
{
    public interface IDeathAnimator
    {
        Task AnimateDeath(CancellationToken cancellationToken);
    }
}