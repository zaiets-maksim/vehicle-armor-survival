using System;
using UnityEngine;

namespace StaticData
{
    [CreateAssetMenu(menuName = "StaticData/Balance", fileName = "Balance", order = 0)]
    public class BalanceStaticData : ScriptableObject
    {
        public Enemies Enemies = new();
    }

    [Serializable]
    public class Enemies
    {
        public EnemyTypeId EnemyTypes;
        [Header("Spawn area")] 
        public Vector2 Long;
        public Vector2 Width;
        public Vector2Int Quantity;
    }
}