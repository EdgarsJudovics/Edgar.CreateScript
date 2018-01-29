using System.IO;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Edgar.CreateScript
{
    public class EcsConfig : ScriptableObject
    {
        public const string ConfigFileName = "Config.asset";

        public string DateFormat = "yyyy-MM-dd";
        public string TimeFormat = "HH:mm:ss";
        public string CompanyName = "";
        public string ProductName = "";
        public string Namespace = "";
        [Header("Namespace autonesting")]
        public bool IsNamespaceAutoNesting = false;
        [Header("Autonesting root folder")]
        public string NamespaceAutoNestingRoot = "Scripts";
        public List<ConfigTag> CustomTags = new List<ConfigTag>();

        /// <summary>
        /// Gets the config that is in use by the plugin. If one doesn't exist, a new config is created
        /// </summary>
        /// <returns>Current config instance</returns>
        public static EcsConfig GetActiveConfig()
        {
            var path = GetConfigPath();
            var c = File.Exists(path) 
                ? AssetDatabase.LoadAssetAtPath(path, typeof(EcsConfig)) as EcsConfig 
                : Create();
            return c;
        }

        /// <summary>
        /// Creates a new config in plugin root folder
        /// </summary>
        /// <returns>Created config instance</returns>
        public static EcsConfig Create()
        {
            var configPath = GetConfigPath();
            if (File.Exists(configPath))
            {
                Debug.LogWarning("Config file already exists");
                return GetActiveConfig();
            }
            var configAsset = CreateInstance<EcsConfig>();
            configAsset.CompanyName = Application.companyName;
            configAsset.ProductName = Application.productName;
            configAsset.Namespace = new Regex("[^a-zA-Z0-9 \\.]")
                .Replace(Application.productName, "");
            AssetDatabase.CreateAsset(configAsset, configPath);
            AssetDatabase.SaveAssets();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = configAsset;
            return AssetDatabase.LoadAssetAtPath(configPath, typeof(EcsConfig)) as EcsConfig;
        }

        /// <summary>
        /// Gets file path to Config.asset file. Path is returned even if file doesn't exist.
        /// </summary>
        /// <returns>Config.asset file path</returns>
        public static string GetConfigPath()
        {
            return Path.Combine(EcsCore.GetPluginRootFolderPath(), ConfigFileName);
        }
    }

    [System.Serializable]
    public class ConfigTag
    {
        public string Name;
        public string Value;
    }
}