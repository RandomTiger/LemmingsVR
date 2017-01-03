// http://answers.unity3d.com/questions/33263/how-to-get-names-of-all-available-levels.html

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ReadSceneNames : MonoBehaviour
 {
     public string[] scenes;
     #if UNITY_EDITOR
     private static string[] ReadNames()
     {
         List<string> temp = new List<string>();
         foreach (EditorBuildSettingsScene S in EditorBuildSettings.scenes)
         {
             if (S.enabled)
             {
                 string name = S.path.Substring(S.path.LastIndexOf('/')+1);
                 name = name.Substring(0,name.Length-6);
                 temp.Add(name);
             }
         }
         return temp.ToArray();
     }
     [MenuItem("CONTEXT/ReadSceneNames/Update Scene Names", false, 100)]
    private static void UpdateNames(MenuCommand command)
     {
         ReadSceneNames context = (ReadSceneNames)command.context;
         context.scenes = ReadNames();
     }
     
     private void Reset()
     {
         scenes = ReadNames();
     }
    
    void Start()
    {
        scenes = ReadNames();
    }
#endif
}