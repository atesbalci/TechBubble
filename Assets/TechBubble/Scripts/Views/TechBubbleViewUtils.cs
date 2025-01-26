namespace TechBubble.Views
{
    public static class TechBubbleViewUtils
    {
        public static string ToMoneyString(this long money)
        {
            return $"${money:N0}";
        }
    }
}