namespace TechBubble.Models
{
    public interface IGameRules
    {
        public float SpawnRadius { get; }
        public float DespawnRadius { get; }
        public int MaxSpawnedInvestmentCount { get; }
    }
}