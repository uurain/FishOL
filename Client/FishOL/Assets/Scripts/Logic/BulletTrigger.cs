using LuaInterface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTrigger : MonoBehaviour
{
    public LuaFunction luaFunc = null;

    private void OnTriggerEnter(Collider other)
    {
        if(luaFunc != null)
        {
            FishTrigger fish = other.GetComponent<FishTrigger>();
            if(fish != null)
                luaFunc.Call(fish.fishId);
        }        
    }

    private void OnDestroy()
    {
        if(luaFunc != null)
        {
            luaFunc.Dispose();
            luaFunc = null;
        }
    }
}
