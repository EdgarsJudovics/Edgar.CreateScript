using System.IO;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;

namespace Edgar.CreateScript
{
    public class EcsCore
    {
        public const string PluginName = "Edgar.CreateScript";
        public const int PluginVersion = 1;

        /// <summary>
        /// Path to current active folder. Determined by which file/folder is selected in inspector
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentFolderPath()
        {
            var path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (string.IsNullOrEmpty(path))
                path = "Assets/";
            else if (!string.IsNullOrEmpty(Path.GetExtension(path)))
            {
                var fileName = Path.GetFileName(path);
                path = path.Substring(0, path.Length - fileName.Length);
            }
            if (path[path.Length - 1] != '/')
                path += "/";
            return path;
        }

        /// <summary>
        /// Path to root folder of this plugin
        /// </summary>
        /// <returns></returns>
        public static string GetPluginRootFolderPath()
        {
            var guids = AssetDatabase.FindAssets(PluginName, null);
            if (guids.Length == 0)
            {
                var message = string.Format("Failed to find \"{0}\" folder. Make sure it is named correctly.", PluginName);
                throw new FileNotFoundException(message);
            }
            if (guids.Length > 1)
            {
                var message = string.Format("Found multiple \"{0}\" folders. Make sure you did not import it multiple times.", PluginName);
                throw new FileNotFoundException(message);
            }
            return AssetDatabase.GUIDToAssetPath(guids[0]);
        }
    }

    internal class CreateScriptEndAction : EndNameEditAction
    {
        private EcsConfig _config;
        private EcsScriptTemplate _template;

        public void LoadDependencies(EcsConfig config, EcsScriptTemplate template)
        {
            _config = config;
            _template = template;
        }

        public override void Action(int instanceId, string pathName, string resourceFile)
        {
            var p = new EcsPreProcessor(_config, pathName);
            var data = p.ProcessScript(_template.Code);

            using (var f = File.CreateText(pathName))
            {
                f.Write(data);
            }

            var newFilePath = Path.Combine(Path.GetDirectoryName(pathName) ?? "", Path.GetFileNameWithoutExtension(pathName) + ".cs");
            File.Move(pathName, newFilePath);
            AssetDatabase.Refresh();
        }
    }
}