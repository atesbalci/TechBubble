using UnityEngine;

namespace TechBubble.Views
{
    public interface IViewData
    {
        float MoneySizeMultiplier { get; }
        float MinimumMoneySizeScale { get; }

        Sprite GetInvestorIcon(int id);
    }
}