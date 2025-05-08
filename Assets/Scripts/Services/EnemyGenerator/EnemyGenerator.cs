using Services.StaticDataService;
using StaticData;
using UnityEngine;

namespace Services.EnemyGenerator
{
    public class EnemyGenerator : IEnemyGenerator
    {
        private IStaticDataService _staticDataService;
        private readonly BalanceStaticData _balance;
        private readonly IEnemyFactory _enemyFactory;

        public EnemyGenerator(IStaticDataService staticDataService, IEnemyFactory enemyFactory)
        {
            _enemyFactory = enemyFactory;
            _balance = staticDataService.Balance();
        }

        public void Generate()
        {
            int count = Random.Range(_balance.Enemies.Quantity.x, _balance.Enemies.Quantity.y);
            float areaX = _balance.Enemies.Width.y - _balance.Enemies.Width.x;
            float areaZ = _balance.Enemies.Long.y - _balance.Enemies.Long.x;
            
            int gridCols = Mathf.CeilToInt(Mathf.Sqrt(count * areaX / areaZ));
            int gridRows = Mathf.CeilToInt((float)count / gridCols);

            float stepX = areaX / gridCols;
            float stepZ = areaZ / gridRows;

            int spawned = 0;
            for (int row = 0; row < gridRows && spawned < count; row++)
            {
                for (int col = 0; col < gridCols && spawned < count; col++)
                {
                    float randomXOffset = Random.Range(-stepX * 0.5f, stepX * 0.5f);
                    float randomZOffset = Random.Range(-stepZ * 0.5f, stepZ * 0.5f);
                    float x = _balance.Enemies.Width.x + col * stepX + randomXOffset;
                    float z = _balance.Enemies.Long.x + row * stepZ + randomZOffset;
                    Vector3 pos = new Vector3(x, 0f, z);
                    
                    float randomYRotation = Random.Range(0f, 360f);
                    _enemyFactory.CreateEnemy(EnemyTypeId.Stickman, pos, Vector3.up * randomYRotation);
                    spawned++;
                }
            }
        }
    }

    public interface IEnemyGenerator
    {
        void Generate();
    }
}