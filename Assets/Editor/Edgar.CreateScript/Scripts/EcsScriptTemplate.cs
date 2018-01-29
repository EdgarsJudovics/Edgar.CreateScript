using System.IO;
using UnityEditor;
using UnityEngine;

namespace Edgar.CreateScript
{
    public class EcsScriptTemplate : ScriptableObject
    {
        #region DEFAULT TEMPLATES
        private const string NewTemplateCode =
            "namespace #NAMESPACE#\n" +
            "{\n" +
            "    public /type/ #SCRIPTNAME#\n" +
            "    {\n" +
            "        \n" +
            "    }\n" +
            "}\n";
        private const string MonoBehaviourTemplateCode =
            "using UnityEngine;\n" +
            "\n" +
            "namespace #NAMESPACE#\n" +
            "{\n" +
            "    public class #SCRIPTNAME# : MonoBehaviour\n" +
            "    {\n" +
            "        \n" +
            "    }\n" +
            "}\n";

        private const string ClassTemplateCode =
            "namespace #NAMESPACE#\n" +
            "{\n" +
            "    public class #SCRIPTNAME#\n" +
            "    {\n" +
            "        \n" +
            "    }\n" +
            "}\n";
        private const string StructTemplateCode =
            "namespace #NAMESPACE#\n" +
            "{\n" +
            "    public struct #SCRIPTNAME#\n" +
            "    {\n" +
            "        \n" +
            "    }\n" +
            "}\n";
        private const string EnumTemplateCode =
            "namespace #NAMESPACE#\n" +
            "{\n" +
            "    public enum #SCRIPTNAME#\n" +
            "    {\n" +
            "        \n" +
            "    }\n" +
            "}\n";
        #endregion
        
        /// <summary>
        /// Path to templates folder
        /// </summary>
        /// <returns></returns>
        public static string GetTemplateFolderPath()
        {
            return Path.Combine(EcsCore.GetPluginRootFolderPath(), "Templates");
        }

        public static void CreateNewDefaultTemplate()
        {
            CreateNewTemplate("Untitled Template", NewTemplateCode, false);
        }

        public static void CreateNewTemplate(string name, string code, bool autoName)
        {
            var path = Path.Combine(GetTemplateFolderPath(), name + ".asset");
            if (File.Exists(path))
            {
                Debug.LogWarning("Template " + name + " already exists");
                return;
            }
            var asset = CreateInstance<EcsScriptTemplate>();
            asset.Code = code;
            if (autoName)
            {
                AssetDatabase.CreateAsset(asset, path);
                AssetDatabase.Refresh();
            }
            else
            {
                ProjectWindowUtil.CreateAsset(asset, path);
            }
        }

        /// <summary>
        /// Gets a specific template instance
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static EcsScriptTemplate GetScriptTemplate(string name)
        {
            return AssetDatabase.LoadAssetAtPath(Path.Combine(EcsCore.GetPluginRootFolderPath(), "Templates/" + name + ".asset"),
                typeof(EcsScriptTemplate)) as EcsScriptTemplate;
        }

        /// <summary>
        /// Creates a new C# script
        /// </summary>
        /// <param name="templateName"></param>
        public static void CreateScript(string templateName)
        {
            var path = Path.Combine(EcsCore.GetCurrentFolderPath(), "Untitled.txt");

            var endAction = CreateInstance<CreateScriptEndAction>();
            var config = EcsConfig.GetActiveConfig();
            var template = GetScriptTemplate(templateName);
            endAction.LoadDependencies(config, template);

            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(
                0,
                endAction,
                path,
                EditorGUIUtility.IconContent("cs Script Icon").image as Texture2D,
                null);
        }

        public static void CreateDefaultTemplates()
        {
            CreateNewTemplate("MonoBehaviour", MonoBehaviourTemplateCode, true);
            CreateNewTemplate("Class", ClassTemplateCode, true);
            CreateNewTemplate("Struct", StructTemplateCode, true);
            CreateNewTemplate("Enum", EnumTemplateCode, true);
        }

        [TextArea(15,50)]
        public string Code;
    }
}