using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Nico.Util
{
    /// <summary>
    /// 代码生成器
    /// </summary>
    /// TODO 暂时用 SO 来做 后面改成 EditorWindow
    [CreateAssetMenu(fileName = "CodeGenerate", menuName = "Test/CodeGenerate", order = 0)]
    public class CodeGenerate : ScriptableObject
    {
        public string directorPath = "Excels/";
        public string csSaveDirePath = "Assets/Test/";
        public string soSaveDirPath = "Assets/Test/SO/";

        public void Generate()
        {
            Debug.Log("生成代码~~~");
            string nameSpace = "Nico.Util";
            string className = "TestMetaData";
            string parentContainerClassName = "MetaDataContainer";
            string parentClassName = "MetaData";

            List<string[]> test_data = new List<string[]>();
            var field1 = new[]
            {
                "id", "int"
            };
            test_data.Add(field1);
            // TODO 生成MetaData类的代码
            string resultStr = "";
            resultStr += $"using UnityEngine;\n";
            resultStr += $"namespace {nameSpace}\n";
            resultStr += "{\n";
            resultStr += $"\tpublic class {className} : {parentClassName} \n";
            resultStr += "\t{\n";
            foreach (var field in test_data)
            {
                resultStr += $"\t\t[field: SerializeField] public {field[1]} {field[0]} {{ get; private set; }}\n";
            }

            resultStr += "\t}\n";
            resultStr += "}\n";
            Debug.Log(resultStr);
        }
    }

    [CustomEditor(typeof(CodeGenerate))]
    public class CodeGenerateEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            var deliveryManager = target as CodeGenerate;
            if (GUILayout.Button("生成代码"))
            {
                if (deliveryManager != null) deliveryManager.Generate();
            }
        }
    }
}