using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

namespace MergeBoard.Readme
{
    [InitializeOnLoad]
    public class ReadmeEditorWindow : EditorWindow
    {
        private Readme _readme;

        static ReadmeEditorWindow()
        {
            EditorApplication.delayCall += AutoOpenReadmeWindow;
        }

        private static void AutoOpenReadmeWindow()
        {
            Open();
        }
        
        [MenuItem("Assets/Create Readme asset")]
        public static void CreateReadmeAsset()
        {
            var readme = CreateInstance<Readme>();
            AssetDatabase.CreateAsset(readme, "Assets/__Readme/Editor/ReadmeEditorWindow.asset");
            AssetDatabase.SaveAssets();
            EditorUtility.FocusProjectWindow();
        }

        [MenuItem("Help/Show MergeBoard Readme")]
        private static void Open()
        {
            var readme = AssetDatabase.LoadAssetAtPath<Readme>("Assets/__Readme/Editor/ReadmeEditorWindow.asset");
            var window = GetWindow<ReadmeEditorWindow>("Welcome to MergeBoard");
            window._readme = readme;
        }
        
        private void OnGUI()
        {
            EditorGUILayout.BeginVertical();
            {
                EditorGUILayout.Space(20);
                EditorGUILayout.LabelField(_readme.Title, EditorStyles.boldLabel);
                EditorGUILayout.Space(20);
                
                var text = _readme.Description;
                var lineCount = text.Split('\n').Length;
                var lineHeight = EditorGUIUtility.singleLineHeight;
                var height = lineCount * lineHeight + 4;
                EditorGUILayout.SelectableLabel(_readme.Description, GUILayout.Height(height));
                EditorGUILayout.Space(20);
                if (!EditorApplication.isPlaying)
                {
                    if (GUILayout.Button("게임 시작"))
                    {
                        EditorSceneManager.OpenScene("Assets/_Project/Scenes/StartupScene.unity");
                        EditorApplication.EnterPlaymode();
                    }
                }
            }
            EditorGUILayout.EndVertical();
        }
    }
}