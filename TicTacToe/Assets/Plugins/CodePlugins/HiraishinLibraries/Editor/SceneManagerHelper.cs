#if UNITY_EDITOR

#endif
using UnityEngine;
using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Text;
using System.IO;

namespace Hiraishin.EditorUtilities
{
    public partial class SceneManagerHelper
    {
        private const string SCENES_FOLDER_PATH = "/Scenes/";
        private const string LOADER_SCRIPT_PATH = "/Plugins/CodePlugins/HiraishinLibraries/Editor/SceneLoader.cs";
        private const string ASSETS_SCENES_PATH = "Assets/Scenes/";

        [MenuItem("Tools/Scene Manager Helper/Reload Scenes")]
        public static void ReloadScenes()
        {
            StringBuilder generateData = new StringBuilder();
            string basePath = Application.dataPath + SCENES_FOLDER_PATH;
            AddHeader(generateData);
            GenerateCodeFromSceneFile(new DirectoryInfo(basePath), generateData, basePath);
            AddFooter(generateData);
            
            string scriptPath = Application.dataPath + LOADER_SCRIPT_PATH;
            using (FileStream file = new FileStream(scriptPath, FileMode.Create))
            {
                using (StreamWriter writer = new  StreamWriter(file))
                {
                    writer.Write(generateData);
                }
            }
            AssetDatabase.Refresh();
        }

        private static void GenerateCode(FileInfo fileInfo, StringBuilder generateData, string basePath)
        {
            string subPath = fileInfo.FullName.Replace('\\', '/').Replace(basePath, String.Empty);
            string assetPath = ASSETS_SCENES_PATH + subPath;

            string methodName = fileInfo.Name.Replace(".unity", string.Empty).Replace(" ", String.Empty)
                .Replace("-", String.Empty);
            generateData.Append(Environment.NewLine);
            generateData.Append("\t\t[MenuItem(\"Tools/Scene Manager Helper/Open/")
                .Append(fileInfo.Name.Replace(".unity", String.Empty)).Append("\")]");
            generateData.Append(Environment.NewLine);
            generateData.Append("\t\tpublic static void Load").Append(methodName).Append("()");
            generateData.Append(Environment.NewLine);
            generateData.Append("\t\t{");
            generateData.Append(Environment.NewLine);
            generateData.Append("\t\t\tOpenScene(\"").Append(assetPath).Append("\");");
            generateData.Append(Environment.NewLine);
            generateData.Append("\t\t}");

        }
        private static void GenerateCodeFromSceneFile(DirectoryInfo directory, StringBuilder generateData, string basePath)
        {
            FileInfo[] files = directory.GetFiles();
            for (int i = 0; i < files.Length; i++)
            {
                FileInfo file = files[i];
                if (file.Extension.Equals(".unity", StringComparison.Ordinal))
                {
                    GenerateCode(file, generateData, basePath);
                }
            }
            DirectoryInfo[] subDirectories = directory.GetDirectories();
            for (int i = 0; i < subDirectories.Length; i++)
            {
                GenerateCodeFromSceneFile(subDirectories[i], generateData, basePath);
            }
        }
        private static void AddHeader(StringBuilder generateData)
        {
            generateData.Append("using UnityEditor;");
            generateData.Append(Environment.NewLine);
            generateData.Append("namespace Hiraishin.EditorUtilities");
            generateData.Append(Environment.NewLine);
            generateData.Append("{");
            generateData.Append(Environment.NewLine);
            generateData.Append("\tpublic partial class SceneManagerHelper");
            generateData.Append(Environment.NewLine);
            generateData.Append("\t{");
            generateData.Append(Environment.NewLine);
            generateData.Append("\t\t#if UNITY_EDITOR");
        }

        private static void AddFooter(StringBuilder generateData)
        {
            generateData.Append(Environment.NewLine);
            generateData.Append("\t\t#endif");
            generateData.Append(Environment.NewLine);
            generateData.Append("\t}");
            generateData.Append(Environment.NewLine);
            generateData.Append("}");
        }

        private static void OpenScene(string scenePath)
        {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
            }
        }
    }
}