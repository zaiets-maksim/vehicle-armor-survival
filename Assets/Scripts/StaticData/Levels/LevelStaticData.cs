using System;
using UnityEngine;

namespace StaticData.Levels
{
    [CreateAssetMenu(fileName = "LevelStaticData", menuName = "StaticData/Levels/Level", order = 0)]   
    public class LevelStaticData : ScriptableObject
    {

    }

    [Serializable]
    public class StorageData
    {
        // public CrateTypeId CrateTypeId;
        public Vector2 Position;
        public Vector3 Rotation;
    }

    [Serializable]
    public class ItemData<TTypeId>
    {
        public TTypeId TypeId;
        public int PurchaseOrder;
        public Vector3 Position;
        public Vector3 Rotation;
        public Transform Parent;
    }
}