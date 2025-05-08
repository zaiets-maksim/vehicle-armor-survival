using System.Linq;
using StaticData.Levels;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(LevelStaticData))]
    public class LevelStaticDataEditor : UnityEditor.Editor
    {
        private LevelStaticData _levelStaticData;

        private void OnEnable()
        {
            _levelStaticData = (LevelStaticData)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            ShowLoadButtons();
        }

        private void ShowLoadButtons()
        {
            if (GUILayout.Button("Load Enemy Data"))
                LoadEnemyData();
        }

        private void LoadEnemyData()
        {
            _levelStaticData.EnemyData = FindEnemies();
            EditorUtility.SetDirty(_levelStaticData);
        }

        private EnemyData[] FindEnemies() =>
            FindObjectsOfType<EnemySpawnMarker>()
                .Select(EnemiesData).ToArray();


        private EnemyData EnemiesData(EnemySpawnMarker enemySpawnMarker) =>
            new()
            {
                TypeId = enemySpawnMarker.TypeId,
                Position = enemySpawnMarker.transform.position,
                Rotation = enemySpawnMarker.transform.eulerAngles,
            };
    }
}