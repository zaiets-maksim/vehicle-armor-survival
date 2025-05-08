using Services.Factories;
using Services.StaticDataService;
using UnityEngine;
using Zenject;

public class EnemyFactory : Factory, IEnemyFactory
{
    private IStaticDataService _staticDataService;

    public EnemyFactory(IInstantiator instantiator, IStaticDataService staticDataService) : base(instantiator)
    {
        _staticDataService = staticDataService;
    }

    public Enemy CreateEnemy(EnemyTypeId enemyTypeId, Vector3 position, Vector3 eulerAngles, Transform parent = null)
    {
        var config = _staticDataService.ForEnemy(enemyTypeId);
        var enemy = InstantiateOnActiveScene<Enemy>(config.Prefab, position, eulerAngles, parent);
        enemy.Initialize(config);
        return enemy;
    }
}

public interface IEnemyFactory
{
    Enemy CreateEnemy(EnemyTypeId enemyTypeId, Vector3 position, Vector3 eulerAngles, Transform parent = null);
}
