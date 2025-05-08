using System;
using UnityEngine;

namespace StaticData.Levels
{
    [CreateAssetMenu(fileName = "LevelStaticData", menuName = "StaticData/Levels/Level", order = 0)]   
    public class LevelStaticData : ScriptableObject
    {
        [Header("Data")]
        public EnemyData[] EnemyData;
    }

    [Serializable]
    public class EnemyData
    {
        public EnemyTypeId TypeId;
        public Vector3 Position;
        public Vector3 Rotation;
    }
}