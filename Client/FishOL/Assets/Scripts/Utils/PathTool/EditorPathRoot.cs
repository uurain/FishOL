using Newtonsoft.Json;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorPathRoot : MonoBehaviour
{
    public class PathData
    {
        public struct PathVector3
        {
            public float x;
            public float y;
            public float z;
        }
        public int ident;
        public List<PathVector3> posList = new List<PathVector3>();
    }

    public int pathId = 0;
    public List<BaseNode> nodeList = new List<BaseNode>();

    [Button("添加一个Bezier路径")]
    private void AddPathNode()
    {
        CreatePathNode(BaseNode.ENodeType.bezier);
    }

    [Button("添加一个Line路径")]
    private void AddLinePathNode()
    {
        CreatePathNode(BaseNode.ENodeType.line);
    }

    [Button("清除所有节点")]
    private void RemoveAll()
    {
        for (int i = 0; i < nodeList.Count; ++i)
        {
            GameObject.DestroyImmediate(nodeList[i].gameObject);
        }
        nodeList.Clear();
    }

    [Button("生成路径")]
    private void CreatePathJson()
    {
        string jsonPath = string.Format("{0}/StreamingAssets/path{1}.json", Application.dataPath, pathId);
        List<Vector3> posList = new List<Vector3>();
        for(int i = 0; i < nodeList.Count; ++i)
        {
            List<Vector3> tempList = new List<Vector3>();
            tempList.AddRange(nodeList[i].GetNodePath());
            if (i > 0)
            {
                tempList.RemoveAt(0);
            }
            posList.AddRange(tempList);
        }

        PathData pathData = new PathData();
        pathData.ident = pathId;
        for(int i = 0; i < posList.Count; ++i)
        {
            PathData.PathVector3 v = new PathData.PathVector3();
            v.x = ((int)posList[i].x * 1000) / 1000.0f;
            v.y = ((int)posList[i].y * 1000) / 1000.0f;
            v.z = ((int)posList[i].z * 1000) / 1000.0f;
            pathData.posList.Add(v);
        }
        string json = JsonConvert.SerializeObject(pathData);
        System.IO.File.WriteAllText(jsonPath, json);
        Debug.Log("生成路径:" + jsonPath);
    }

    void CreatePathNode(BaseNode.ENodeType eType)
    {
        GameObject go = new GameObject("PathNode" + nodeList.Count);
        go.transform.SetParent(transform);
        BaseNode bNode = null;
        if (eType == BaseNode.ENodeType.bezier)
            bNode = go.AddComponent<BezierNode>();
        else if (eType == BaseNode.ENodeType.line)
            bNode = go.AddComponent<LineNode>();
        bNode.Init(this);
        if (nodeList.Count > 0)
        {
            bNode.preNode = nodeList[nodeList.Count - 1];
        }
        nodeList.Add(bNode);
    }

    public void RemovePathNode(BaseNode bNode)
    {
        int index = -1;
        for(int i = 0; i < nodeList.Count; ++i)
        {
            if(bNode == nodeList[i])
            {
                index = i;
                break;
            }
        }

        if (index >= 0)
        {
            if (index < nodeList.Count - 1)
            {
                nodeList[index + 1].preNode = null;
            }
            nodeList.RemoveAt(index);

            GameObject.DestroyImmediate(bNode.gameObject);
        }
    }
}
