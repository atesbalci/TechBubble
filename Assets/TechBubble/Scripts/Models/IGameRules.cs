namespace TechBubble.Models
{
    public interface IGameRules
    {
        float SpawnRadius { get; }
        float DespawnRadius { get; }
        int MaxSpawnedInvestmentCount { get; }
        float MoneyLossPerDistance { get; }
        float DeadlineMaxDist { get; }
        float DeadlineMinDist { get; }
        long StartMoney { get; }
        long[] InvestorPossibilities { get; }
    }
}