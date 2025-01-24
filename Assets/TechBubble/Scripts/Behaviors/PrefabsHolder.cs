using UnityEngine;

namespace TechBubble.Behaviors
{
    [CreateAssetMenu(fileName = "PrefabsHolder", menuName = "TechBubble/Prefabs Holder")]
    public class PrefabsHolder : ScriptableObject, IPrefabsProvider
    {
        public GameObject InvestmentPrefab => investmentPrefab;

        [SerializeField] private GameObject investmentPrefab;
    }
}