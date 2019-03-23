using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseNode : MonoBehaviour
{
    public static Vector3[] INITPOS = new Vector3[] {
        new Vector3(0, 0, 0),  new Vector3(5, 0, 5),new Vector3(15, 0, 5), new Vector3(20, 0, 0)
    };
    public static string[] INITMATPATH = new string[]
    {
        "matRed","matYellow","matBlue","matGreen",
    };
    public enum ENodeType
    {
        bezier, line,
    }

    public BaseNode preNode;
    public Color lineColor = Color.yellow;
    public EditorPathRoot _root;
    public List<Vector3> pathList = new List<Vector3>();
    public Transform[] transChild;

    public List<Vector3> GetNodePath()
    {
        return pathList;
    }


    [Button("删除节点")]
    private void RemoveNode()
    {
        if (_root != null)
        {
            _root.RemovePathNode(this);
        }
    }

    public virtual void Init(EditorPathRoot rt)
    {
        _root = rt;
    }

    public virtual void CreatePath()
    {
        if(transChild != null)
        {
            if (preNode != null)
            {
                transChild[0].position = preNode.GetEndPos();
            }
        }
        pathList.Clear();
    }

    public Vector3 GetEndPos()
    {
        if (transChild == null)
            return Vector3.zero;
        return transChild[transChild.Length - 1].position;
    }

    public Material GetColorMat(int index)
    {
#if UNITY_EDITOR
        return UnityEditor.AssetDatabase.LoadAssetAtPath<Material>(string.Format("Assets/Art/mat/{0}.mat", BaseNode.INITMATPATH[index]));
#endif
        return null;
    }

    public void OnDrawGizmos()
    {
        if (transChild == null)
            return;
        CreatePath();
        Gizmos.color = lineColor;
        for (int i = 0; i < pathList.Count - 1; i++)
        {
            Gizmos.DrawLine(pathList[i], pathList[i + 1]);
        }
    }
}
