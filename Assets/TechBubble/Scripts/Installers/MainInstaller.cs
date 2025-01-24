using TechBubble.Behaviors;
using UnityEngine;
using Zenject;

namespace TechBubble.Installers
{
    public class MainInstaller : MonoInstaller
    {
        [SerializeField] private GameRulesHolder gameRulesHolder;
        [SerializeField] private PrefabsHolder prefabsHolder;
        [SerializeField] private PlayerBehaviour playerBehaviour;
        
        public override void InstallBindings()
        {
            
        }
    }
}