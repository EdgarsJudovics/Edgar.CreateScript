using UnityEditor;

namespace Edgar.CreateScript
{
    public class EcsMenuItems : Editor
    {
        [MenuItem("Assets/Create Script/MonoBehaviour", priority = 18)]
        private static void CreateMonoBehaviour()
        {
            EcsScriptTemplate.CreateScript("MonoBehaviour");
        }
        [MenuItem("Assets/Create Script/Class", priority = 18)]
        private static void CreateClass()
        {
            EcsScriptTemplate.CreateScript("Class");
        }
        [MenuItem("Assets/Create Script/Struct", priority = 18)]
        private static void CreateStruct()
        {
            EcsScriptTemplate.CreateScript("Struct");
        }
        [MenuItem("Assets/Create Script/Enum", priority = 18)]
        private static void CreateEnum()
        {
            EcsScriptTemplate.CreateScript("Enum");
        }
        // ---
        [MenuItem("Assets/Create Script/Other...", priority = 40)]
        private static void CreateOther()
        {
            EcsCreateOtherWindow.Create();
        }
        // ---
        [MenuItem("Assets/Create Script/New template...",  priority = 80)]
        private static void CreateNewTemplate()
        {
            EcsScriptTemplate.CreateNewDefaultTemplate();
        }
        [MenuItem("Assets/Create Script/Initialize", priority = 80)]
        private static void CreateConfig()
        {
            EcsScriptTemplate.CreateDefaultTemplates();
            EcsConfig.Create();
        }
    }
}
