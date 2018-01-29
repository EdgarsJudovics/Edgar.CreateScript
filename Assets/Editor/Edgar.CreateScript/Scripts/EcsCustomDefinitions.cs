using UnityEditor;

namespace Edgar.CreateScript
{
    public class EcsCustomDefinitions : Editor
    {
        /* uncomment lines below to add custom items to context menu*/
        //[MenuItem("Assets/Create Script/{NAME}", priority = 19)]
        //private static void CreateCustomScript()
        //{
        //    EcsScriptTemplate.CreateScript("{TEMPLATENAME}");
        //}

        public static void ProcessPreProcessor(EcsPreProcessor p)
        {
            // add code here if you wish to adjust preprocessor
            // example
            //p.Tags.Add("custom", "put value here");
        }
    }
}

