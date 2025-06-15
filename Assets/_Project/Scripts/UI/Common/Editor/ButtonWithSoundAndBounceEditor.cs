using UnityEditor;
using UnityEditor.UI;

namespace MergeBoard.UI.Common.Editor
{
    [CustomEditor(typeof(MergeBoard.UI.Common.ButtonWithSoundAndBounce))]
    public class ButtonWithSoundAndBounceEditor : ButtonEditor
    {
        private SerializedProperty soundKey;
        private SerializedProperty pressedScale;
        private SerializedProperty bounceScale;
        private SerializedProperty bounceDuration;

        protected override void OnEnable()
        {
            base.OnEnable();
            soundKey = serializedObject.FindProperty("soundKey");
            pressedScale = serializedObject.FindProperty("pressedScale");
            bounceScale = serializedObject.FindProperty("bounceScale");
            bounceDuration = serializedObject.FindProperty("bounceDuration");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();
            EditorGUILayout.PropertyField(soundKey);
            EditorGUILayout.PropertyField(pressedScale);
            EditorGUILayout.PropertyField(bounceScale);
            EditorGUILayout.PropertyField(bounceDuration);
            serializedObject.ApplyModifiedProperties();
        }
    }
}
