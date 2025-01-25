using System.Threading;
using System.Threading.Tasks;
using TechBubble.Models;

namespace TechBubble.Views
{
    public interface IAssholeInvestorAnimator
    {
        Task ShowAnimation(InvestmentData investmentData, CancellationToken cancellationToken);
    }
}