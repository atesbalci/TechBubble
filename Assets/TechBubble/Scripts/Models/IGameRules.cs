using System;

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
        float DeadlineLongDist { get; }
        float DeadlineLongDistPossibility { get; }
        long StartMoney { get; }
        InvestmentData[] InvestorPossibilities { get; }
    }

    [Serializable]
    public struct InvestmentData
    {
        public long Money;
        public float Deadline;
        public int Id;
    }
}