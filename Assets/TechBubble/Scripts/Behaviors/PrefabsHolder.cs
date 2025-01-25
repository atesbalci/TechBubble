using UnityEngine;

namespace TechBubble.Behaviors
{
    [CreateAssetMenu(fileName = "PrefabsHolder", menuName = "TechBubble/Prefabs Holder")]
    public class PrefabsHolder : ScriptableObject
    {
        public GameObject InvestmentPrefab => investmentPrefab;
        public GameObject DeadlinePrefab => deadlinePrefab;

        [SerializeField] private GameObject investmentPrefab;
        [SerializeField] private GameObject deadlinePrefab;
    }
}