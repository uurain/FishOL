using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierNode : BaseNode
{
    public int pointCount = 100;
      
    public override void Init(EditorPathRoot root)
    {
        base.Init(root);
        transChild = new Transform[4];
        for (int i = 0; i < 4; ++i)
        {
            Transform node = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;            
            node.name = string.Format("node{0}", i +1);
            node.SetParent(transform);
            node.localPosition = BaseNode.INITPOS[i];
            node.GetComponent<Renderer>().material = GetColorMat(i);
            transChild[i] = node;
        }
    }

    public override void CreatePath()
    {
        base.CreatePath();
        for (int i = 0; i < pointCount; i++)
        {
            Vector3 pos = Bezier.BezierCurve(transChild[0].position, transChild[1].position
                , transChild[2].position, transChild[3].position, (float)i / pointCount);
            pathList.Add(pos);
        }
    }
}
