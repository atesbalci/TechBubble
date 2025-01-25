using TechBubble.Behaviors;
using TechBubble.Controllers;
using TechBubble.Models;
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
            Container.BindInstance<IGameRules>(gameRulesHolder).AsSingle();
            Container.BindMemoryPool<InvestmentBehavior, InvestmentPool>()
                .WithInitialSize(gameRulesHolder.MaxSpawnedInvestmentCount)
                .FromComponentInNewPrefab(prefabsHolder.InvestmentPrefab);
            Container.BindMemoryPool<DeadlineBehavior, DeadlinePool>()
                .WithInitialSize(10)
                .FromComponentInNewPrefab(prefabsHolder.DeadlinePrefab);
            Container.BindInstance(playerBehaviour).AsSingle();
            Container.Bind<IDeadlinesProvider>().To<GameController>().AsSingle().NonLazy();
        }
    }
}