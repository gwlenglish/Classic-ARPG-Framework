using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using UnityEngine;

namespace GWLPXL.ARPGCore.com
{
    [System.Serializable]
    public class Documentation
    {
        public List<CodeDocument> Namespaces = new List<CodeDocument>();
    }
    /// <summary>
    /// a scriptable object that provides a list of namespaces, classes, and their methods with parameters in the entire ARPG solution.
    /// </summary>
    [CreateAssetMenu(menuName ="GWLPXL/ARPG/CodeDocument")]
    public class CodeDocumentSO : ScriptableObject
    {
        public Documentation Documentation;


        public void UpdateDocumentation()
        {
            GetEntireDocument();
        }


        BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly;

        void GetEntireDocument()
        {
            Assembly asm = Assembly.GetExecutingAssembly();

            Dictionary<string, List<string>> namespaceclassdic = new Dictionary<string, List<string>>();
            List<string> namespacelist = new List<string>();
            List<string> classlist = new List<string>();
            List<CodeDocument> _temp = new List<CodeDocument>();
            foreach (Type type in asm.GetTypes())
            {
                //Debug.Log(type.Namespace);
                if (type == null) continue;
                if (type.Namespace == null) continue;

                if (type.Namespace.StartsWith("GWLPXL", StringComparison.OrdinalIgnoreCase))
                {
                    namespaceclassdic.TryGetValue(type.Namespace, out List<string> value);
                    if (value == null)
                    {
                        value = new List<string>();

                    }
                    value.Add(type.FullName);
                    namespaceclassdic[type.Namespace] = value;
                }
            }

            foreach (var kvp in namespaceclassdic)
            {
                CodeDocument newdocument = new CodeDocument(kvp.Key);
                newdocument.Classes = GetMethods(kvp.Value);
                _temp.Add(newdocument);
            }

            Documentation.Namespaces = _temp;

        }


        List<CodeClass> GetMethods(List<string> classes)
        {

            List<CodeClass> _temp = new List<CodeClass>();
            for (int i = 0; i < classes.Count; i++)
            {
                //Debug.Log(classes[i]);
                Type type = Type.GetType(classes[i]);
                CodeClass codeclass = new CodeClass();
                codeclass.ClassName = classes[i];
                MethodInfo[] methodi = type.GetMethods(flags);
                List<CodeMethod> methods = new List<CodeMethod>();
                for (int j = 0; j < methodi.Length; j++)
                {
                    CodeMethod codemethod = new CodeMethod();
                    MethodInfo methinfo = methodi[j];

                    codemethod.MethodName = methinfo.Name;
                    ParameterInfo[] parainfo = methinfo.GetParameters();
                    List<string> paramTypes = new List<string>();
                    for (int k = 0; k < parainfo.Length; k++)
                    {
                        string paraname = parainfo[k].ParameterType.Name;
                        paramTypes.Add(paraname);

                    }
                    codemethod.MethodParams = paramTypes.ToArray();
                    methods.Add(codemethod);
                }

                codeclass.Methods = methods;
                _temp.Add(codeclass);
            }

            return _temp;

        }
    }
}
