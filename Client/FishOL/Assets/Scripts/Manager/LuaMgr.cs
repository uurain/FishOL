using UnityEngine;
using System.Collections;
using LuaInterface;
using System.Collections.Generic;

public class LuaMgr : LuaClient
{

    protected override void LoadLuaFiles()
    {

    }

    public void InitStart() {
        InitLuaPath();
        OnLoadFinished();
    }

    /// <summary>
    /// 初始化Lua代码加载路径
    /// </summary>
    void InitLuaPath() {
        if (AppConst.DebugMode) {
            string rootPath = AppConst.FrameworkRoot;
            luaState.AddSearchPath(rootPath + "/Lua");
            luaState.AddSearchPath(rootPath + "/ToLua/Lua");
        } else {
            luaState.AddSearchPath(Util.DataPath + "lua");
        }
    }     

    public void DoFile(string filename) {
        luaState.DoFile(filename);
    }

    // Update is called once per frame
    public object[] CallFunction(string funcName, params object[] args) {
        LuaFunction func = luaState.GetFunction(funcName);
        if (func != null) {
            return func.LazyCall(args);
        }
        return null;
    }

    public LuaFunction GetFunction(string funcName)
    {
        LuaFunction func = luaState.GetFunction(funcName);
        return func;
    }

    public void LuaGC() {
        luaState.LuaGC(LuaGCOptions.LUA_GCCOLLECT);
    }

    public void Close() {
        DetachProfiler();
        LuaState state = luaState;
        luaState = null;

        if (levelLoaded != null)
        {
            levelLoaded.Dispose();
            levelLoaded = null;
        }

        if (loop != null)
        {
            loop.Destroy();
            loop = null;
        }

        state.Dispose();
        Instance = null;
    }
}