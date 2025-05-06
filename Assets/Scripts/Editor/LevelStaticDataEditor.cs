using StaticData.Levels;
using UnityEditor;

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
        }
    }
}