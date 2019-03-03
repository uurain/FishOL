#define ASYNC_MODE

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using LuaInterface;
using System.Reflection;
using System.IO;
using pg;
using FairyGUI;

public class GameMgr : MonoBehaviour
{
    public static GameMgr instance = null; 

    public LuaFunction LuaEventHandle;

    LuaMgr _luaMgr;
    ResMgr _resMgr;
    SaveDataMgr _saveDataMgr;
    NetworkMgr _networkMgr;

    /// <summary>
    /// 初始化游戏管理器
    /// </summary>
    void Awake() {
        GameObject.DontDestroyOnLoad(gameObject);
        instance = this;

        Init();

        LuaEventHandle = Util.GetFunction("PgEventDispatch", "DoEvent");
    }

    public LuaMgr luaMgr
    {
        get
        {
            if(_luaMgr == null)
            {
                _luaMgr = gameObject.AddComponent<LuaMgr>();
            }
            return _luaMgr;
        }
    }

    public ResMgr resMgr
    {
        get
        {
            if (_resMgr == null)
            {
                _resMgr = gameObject.AddComponent<ResMgr>();
            }
            return _resMgr;
        }
    }

    public SaveDataMgr saveDataMgr
    {
        get
        {
            if (_saveDataMgr == null)
                _saveDataMgr = gameObject.AddUniqueCompoment<SaveDataMgr>();
            return _saveDataMgr;
        }
    }

    public NetworkMgr networkMgr
    {
        get
        {
            if (_networkMgr == null)
                _networkMgr = gameObject.AddUniqueCompoment<NetworkMgr>();
            return _networkMgr;
        }
    }


    void Update()
    {
     
    }

    /// <summary>
    /// 初始化
    /// </summary>
    void Init() {
        InitUIConfig();
        OnInitialize();
    }

    void InitUIConfig()
    {
        GRoot.inst.SetContentScaleFactor(1334, 750);
        GameObject.DontDestroyOnLoad(Stage.inst.GetRenderCamera().gameObject);
        UIConfig.defaultFont = "afont";
        FontManager.RegisterFont(FontManager.GetFont("txjlzy"), "Tensentype JiaLiZhongYuanJ");
        FontManager.RegisterFont(FontManager.GetFont("STXINWEI_1"), "华文新魏");

        UIObjectFactory.SetLoaderExtension(typeof(pg.MyGLoader));
    }


    /// <summary>
    /// 资源初始化结束
    /// </summary>
    public void OnResourceInited() {
        //ResManager.Initialize(AppConst.AssetDir, delegate() {
        //    Debug.Log("Initialize OK!!!");
        //    this.OnInitialize();
        //});
    }
    void OnInitialize() {

        //return;
        Debug.Log(" OnInitialize Manager/Game");
        luaMgr.InitStart();
        luaMgr.DoFile("Manager/Game");         //加载游戏
        luaMgr.DoFile("Manager/Network");         
        Util.CallMethod("Game", "OnInitOK");     //初始化完成
    }



    /// <summary>
    /// 析构函数
    /// </summary>
    void OnDestroy() {
        //if (LuaManager != null) {
        //    LuaManager.Close();
        //}
        Debug.Log("~GameManager was destroyed");
    }
}