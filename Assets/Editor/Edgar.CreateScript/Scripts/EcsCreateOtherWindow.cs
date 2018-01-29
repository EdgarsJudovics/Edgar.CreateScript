using System.IO;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace Edgar.CreateScript
{
    public class EcsCreateOtherWindow : EditorWindow
    {
        private static EcsCreateOtherWindow _window;
        public static void Create()
        {
            if (_window != null)
            {
                _window.Close();
            }
            var window = CreateInstance<EcsCreateOtherWindow>();
            window.titleContent = new GUIContent("Create other...");
            _window = window;
            window.ShowUtility();
        }

        private void OnGUI()
        {
            GUILayout.Label("Select a template", EditorStyles.boldLabel);
            EditorGUILayout.BeginScrollView(new Vector2(0, 0));
            var files = Directory.GetFiles(Path.Combine(EcsCore.GetPluginRootFolderPath(), "Templates"))
                .Where(f => f.EndsWith(".asset")); // skip .meta files
            foreach (var file in files)
            {
                var filename = Path.GetFileNameWithoutExtension(file);
                GUILayout.BeginHorizontal();
                if (GUILayout.Button(filename))
                {
                    if (_window != null)
                    {
                        _window.Close();
                        _window = null;
                    }
                    EcsScriptTemplate.CreateScript(filename);
                }
                if (GUILayout.Button("X", GUILayout.Width(20)))
                {
                    if(EditorUtility.DisplayDialog("Delete warning", "Delete " + filename + " template?", "Yes", "No"))
                    {
                        AssetDatabase.DeleteAsset(file);
                    }
                }
                GUILayout.EndHorizontal();
            }
            EditorGUILayout.EndScrollView();
            GUILayout.FlexibleSpace();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("(+) New Template"))
            {
                EcsScriptTemplate.CreateNewDefaultTemplate();
            }
            GUILayout.EndHorizontal();
        }
    }
}