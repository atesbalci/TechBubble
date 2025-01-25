using TechBubble.Models;
using UnityEngine;

namespace TechBubble.Behaviors
{
    [CreateAssetMenu(fileName = "GameRulesHolder", menuName = "TechBubble/Game Rules Holder")]
    public class GameRulesHolder : ScriptableObject, IGameRules
    {
        public float SpawnRadius => spawnRadius;
        public float DespawnRadius => despawnRadius;
        public int MaxSpawnedInvestmentCount => maxSpawnedInvestmentCount;
        public float MoneyLossPerDistance => moneyLossPerDistance;
        public float DeadlineMaxDist => deadlineMaxDist;
        public float DeadlineMinDist => deadlineMinDist;
        public long StartMoney => startMoney;
        public InvestmentData[] InvestorPossibilities => investorPossibilities;

        [SerializeField] private float spawnRadius;
        [SerializeField] private float despawnRadius;
        [SerializeField] private int maxSpawnedInvestmentCount;
        [SerializeField] private float moneyLossPerDistance;
        [SerializeField] private float deadlineMaxDist;
        [SerializeField] private float deadlineMinDist;
        [SerializeField] private long startMoney;
        [SerializeField] private InvestmentData[] investorPossibilities;
    }
}