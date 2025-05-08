using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnMarker : MonoBehaviour
{
    [SerializeField] private EnemyTypeId _typeId;
    public EnemyTypeId TypeId => _typeId;
}
