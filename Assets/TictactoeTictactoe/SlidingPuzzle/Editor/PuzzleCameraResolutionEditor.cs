using UnityEditor;
using TictactoeTictactoe.SlidingPuzzle.Runtime;

namespace TictactoeTictactoe.SlidingPuzzle.Editor
{
    [CustomEditor(typeof(PuzzleCameraResolution))]
    public class PuzzleCameraResolutionEditor : UnityEditor.Editor
    {
        private PuzzleCameraResolution _puzzleCameraResolution;

        void OnEnable() {
            _puzzleCameraResolution = target as PuzzleCameraResolution;
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Use Device Resolution ");
                _puzzleCameraResolution.isAutoSize = EditorGUILayout.Toggle(_puzzleCameraResolution.isAutoSize);
            EditorGUILayout.EndHorizontal();

            EditorGUI.BeginDisabledGroup(_puzzleCameraResolution.isAutoSize);
                _puzzleCameraResolution.width = EditorGUILayout.IntField("Width", _puzzleCameraResolution.width);
                _puzzleCameraResolution.height = EditorGUILayout.IntField("Height", _puzzleCameraResolution.height);
            EditorGUI.EndDisabledGroup();
        }
    }
}