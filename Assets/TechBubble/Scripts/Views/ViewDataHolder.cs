using UnityEngine;

namespace TechBubble.Views
{
    [CreateAssetMenu(fileName = "ViewData", menuName = "TechBubble/View Data", order = 0)]
    public class ViewDataHolder : ScriptableObject, IViewData
    {
        public float MoneySizeMultiplier => moneySizeMultiplier;
        public float MinimumMoneySizeScale => minimumMoneySizeScale;
        
        [SerializeField] private float moneySizeMultiplier;
        [SerializeField] private float minimumMoneySizeScale;
    }
}