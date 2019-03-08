using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PathEditor
{
    [MenuItem("GameObject/Path/CreatePath", false, 0)]
    static GameObject CreatePath()
    {
        GameObject go = new GameObject("PathRoot");
        go.AddUniqueCompoment<EditorPathRoot>();
        return go;
    }
}
