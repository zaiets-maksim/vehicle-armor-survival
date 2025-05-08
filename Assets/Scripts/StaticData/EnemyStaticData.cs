using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "StaticData/Enemies", fileName = "EnemiesStaticData", order = 0)]
public class EnemyStaticData : ScriptableObject
{
    public List<EnemyConfig> Configs = new();

}