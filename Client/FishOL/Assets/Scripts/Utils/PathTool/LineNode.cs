using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineNode : BaseNode
{
    public override void Init(EditorPathRoot rt)
    {
        base.Init(rt);
        transChild = new Transform[2];
        for (int i = 0; i < transChild.Length; ++i)
        {
            Transform node = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
            node.name = string.Format("node{0}", i + 1);
            node.SetParent(transform);
            if(i == 1)
                node.localPosition = BaseNode.INITPOS[3];
            else
                node.localPosition = BaseNode.INITPOS[i];
            node.GetComponent<Renderer>().material = GetColorMat(i);
            transChild[i] = node;
        }
    }

    public override void CreatePath()
    {
        base.CreatePath();
        for (int i = 0; i < transChild.Length; i++)
        {
            pathList.Add(transChild[i].position);
        }
    }
}
