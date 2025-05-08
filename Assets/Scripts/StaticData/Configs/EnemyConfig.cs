using System;
using UnityEngine;

[Serializable]
public class EnemyConfig
{
    public EnemyTypeId TypeId;
    public GameObject Prefab;
    public int Heath;
    public int Damage;
}
