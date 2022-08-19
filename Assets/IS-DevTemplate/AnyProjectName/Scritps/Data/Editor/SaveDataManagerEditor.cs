using ISDevTemplate.Data;
using UnityEditor;
using UnityEngine;

namespace ISDevTemplateEditor
{
    [CustomEditor(typeof(SaveDataManager))]
    internal class SaveDataManagerEditor : Editor
    {
        string _sceneName;
        int _sceneIndex;
        int _highScore;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var manager = target as SaveDataManager;

            EditorGUILayout.Space(5f);

            _sceneName = EditorGUILayout.TextField("保存したいシーン名", _sceneName);

            _sceneIndex = EditorGUILayout.IntField("保存したいシーンのIndex", _sceneIndex);

            _highScore = EditorGUILayout.IntField("最高得点", _highScore);

            if (GUILayout.Button("保存"))
            {
                _ = manager.SaveAsync(new SaveData(_sceneName, _sceneIndex, _highScore));
            }

            EditorGUILayout.Space(10f);

            if (GUILayout.Button("リセット"))
            {
                _ = manager.ResetSaveDataAsync();
            }
        }
    }
}
