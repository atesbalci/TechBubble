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
        
        [SerializeField] private float spawnRadius;
        [SerializeField] private float despawnRadius;
        [SerializeField] private int maxSpawnedInvestmentCount;
    }
}