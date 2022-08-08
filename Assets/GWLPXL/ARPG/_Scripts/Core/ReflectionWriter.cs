using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System.Linq;
using System;
using GWLPXL.ARPGCore.com;

[System.Serializable]
public class ParamInfo
{
    public string ParamType;
    public string ParamName;
}
[System.Serializable]
public class CodeMethod
{
    public string MethodName;
    public string[] MethodParams;
}
[System.Serializable]
public class CodeClass
{
    public string ClassName;
    public List<CodeMethod> Methods;
}
[System.Serializable]
public class CodeDocument
{
    public string NameSpace;
    public List<CodeClass> Classes;
    public CodeDocument(string namespacename)
    {
        NameSpace = namespacename;
        Classes = new List<CodeClass>();
    }
   
}
/// <summary>
/// used in the creation of the code document
/// </summary>
public class ReflectionWriter : MonoBehaviour
{
    public List<CodeDocument> CodeDocument = new List<CodeDocument>();
    public CodeDocumentSO Data;
    static BindingFlags flags = BindingFlags.Instance  | BindingFlags.Public  | BindingFlags.DeclaredOnly;
    private void Start()
    {
       
    }


    [ExecuteAlways]
    [ContextMenu("Test Write")]
  public void TestWrite()
    {
        GetEntireDocument();

    
    }

    private Type[] GetTypesInNamespace(Assembly assembly, string nameSpace)
    {
        return
          assembly.GetTypes()
                  .Where(t => String.Equals(t.Namespace, nameSpace, StringComparison.Ordinal))
                  .ToArray();
    }


    [ExecuteAlways]
    static List<string> GetClassesString(string nameSpace)
    {
        Assembly asm = Assembly.GetExecutingAssembly();

        List<string> namespacelist = new List<string>();
        List<string> classlist = new List<string>();

        foreach (Type type in asm.GetTypes())
        {
            Debug.Log(type.Namespace);
            if (type.Namespace == nameSpace)
            {
                namespacelist.Add(type.FullName);

            }

        }

        foreach (string classname in namespacelist)
            classlist.Add(classname);

        return classlist;
    }

    void GetEntireDocument()
    {
        Assembly asm = Assembly.GetExecutingAssembly();

        Dictionary<string, List<string>> namespaceclassdic = new Dictionary<string, List<string>>();
        List<string> namespacelist = new List<string>();
        List<string> classlist = new List<string>();

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
        CodeDocument.Clear();
        foreach (var kvp in namespaceclassdic)
        {
            CodeDocument newdocument = new CodeDocument(kvp.Key);
            newdocument.Classes = GetMethods(kvp.Value);
            CodeDocument.Add(newdocument);
        }


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
