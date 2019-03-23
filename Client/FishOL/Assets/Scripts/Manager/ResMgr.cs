using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using LuaInterface;
using UObject = UnityEngine.Object;
using UnityEngine.SceneManagement;
using Tangzx.ABSystem;

public class ResMgr : MonoBehaviour
{
    public enum ETaskType
    {
        gameobject,
        abinfo,
        asset,
    }
    public class TaskLoader
    {
        public TaskLoader(string p, ETaskType t)
        {
            path = p;
            taskType = t;
        }

        private ETaskType taskType = ETaskType.gameobject;
        private string path;

        public LoadGameObjectCompleteHandler onGoComplete;
        public AssetBundleManager.LoadAssetCompleteHandler onAbInfoComplete;
        public LoadObjectCompleteHandler onAssetComplete;

        public void Begin(int prority = 0)
        {
            if(taskType == ETaskType.gameobject)
            {
                AssetObject assetObj = GameMgr.instance.resMgr.GetCached(path);
                if(assetObj != null)
                {
                    RequestGoComplete(assetObj);
                    RequstComplete();
                    return;
                }
            }
            GameMgr.instance.resMgr.assetManager.Load(path, prority, taskType == ETaskType.gameobject, OnLoadComplete);
        }

        private void OnLoadComplete(AssetBundleInfo abInfo)
        {
            if (abInfo != null)
            {
                switch(taskType)
                {
                    case ETaskType.gameobject:
                        {
                            GameObject go = abInfo.Instantiate();
                            AssetObject assetObj = go.AddUniqueCompoment<AssetObject>();
                            assetObj.key = path;
                            RequestGoComplete(assetObj);
                        }
                        break;
                    case ETaskType.abinfo:
                        {
                            RequestAbInfoComplete(abInfo);
                        }
                        break;
                    case ETaskType.asset:
                        {
                            RequestAssetComplete(abInfo);
                        }
                        break;
                }
            }
            else
            {
                Debug.LogAssertionFormat("TaskLoader abinfonull:{0}", path);
            }
            RequstComplete();
        }

        private void RequestGoComplete(AssetObject assetObj)
        {
            if (onGoComplete != null)
            {
                var handler = onGoComplete;
                onGoComplete = null;
                handler(assetObj);
            }
        }

        private void RequestAbInfoComplete(AssetBundleInfo abInfo)
        {
            if (onAbInfoComplete != null)
            {
                var handler = onAbInfoComplete;
                onAbInfoComplete = null;
                handler(abInfo);
            }
        }

        private void RequestAssetComplete(AssetBundleInfo abInfo)
        {
            if (onAssetComplete != null)
            {
                var handler = onAssetComplete;
                onAssetComplete = null;
                UnityEngine.Object asset = abInfo.mainObject;
                abInfo.UnloadBundle(false);
                handler(abInfo.mainObject);
            }
        }

        private void RequstComplete()
        {
            GameMgr.instance.resMgr.LoadComplete(this);
        }
    }

    public delegate void LoadGameObjectCompleteHandler(AssetObject go);
    public delegate void LoadObjectCompleteHandler(UnityEngine.Object obj);

    private const int MaxRequstCount = 5;

    // 加载中
    private bool _isCurrentLoading;
    // 缓存
    private Dictionary<string, Tangzx.ABSystem.ObjectPool<AssetObject>> _cachedAssetObj = new Dictionary<string, Tangzx.ABSystem.ObjectPool<AssetObject>>();

    // 等待加载的task
    private List<TaskLoader> _waitLoadQueue = new List<TaskLoader>();
    // 正在加载的task
    private HashSet<TaskLoader> _currentLoadQueue = new HashSet<TaskLoader>();
    // 加载loader缓存
    private Dictionary<string, TaskLoader> _loaderCache = new Dictionary<string, TaskLoader>();

    public AssetBundleManager assetManager;
    AssetBundleLoadProgress mProcess;

    Transform _cachedParent = null;

    public void Initialize(Action initOK)
    {
        GameObject cachedGo = new GameObject("CachedParent");
        cachedGo.SetActive(false);
        _cachedParent = cachedGo.transform;
        _cachedParent.SetParent(transform);

        if (assetManager == null)
            assetManager = gameObject.AddUniqueCompoment<AssetBundleManager>();
        assetManager.Init(initOK);
        assetManager.onProgress = OnAssetProgress;
    }

    private void OnAssetProgress(AssetBundleLoadProgress progress)
    {
        mProcess = progress;
    }

    public float GetProgress()
    {
        if(mProcess != null)
        {
            return mProcess.complete;
        }
        return 0;
    }

    public void LoadPackage(string fullPath, Action<string> dl, LuaFunction luaFunc)
    {
#if AB_MODE
        Load(fullPath, delegate (Tangzx.ABSystem.AssetBundleInfo abInfo) {
            string pkgName = "";
            if (abInfo == null || abInfo.bundle == null)
            {
                Debug.LogError("LoadPackage error:" + fullPath);
            }
            else
            {
                pkgName = FairyGUI.UIPackage.AddPackage(abInfo.bundle).name;
                abInfo.bundle = null;
                assetManager.RemoveBundleInfo(abInfo);
            }
            if (dl != null)
                dl(pkgName);
            if (luaFunc != null)
            {
                luaFunc.Call(pkgName);
                luaFunc.Dispose();
                luaFunc = null;
            }
        });
#else
#if UNITY_EDITOR
        string pkgName = FairyGUI.UIPackage.AddPackage(fullPath, (string name, string extension, System.Type type, out FairyGUI.DestroyMethod destroyMethod) => {
            destroyMethod = FairyGUI.DestroyMethod.None;
            int tIndex = name.LastIndexOf('&');
            string pName = name.Substring(tIndex+1, name.Length-tIndex-1);
            string path = "Assets/AbAsset/" + fullPath + "/" + pName + extension;
            return UnityEditor.AssetDatabase.LoadAssetAtPath(path, type);
        }).name;
        if (dl != null)
            dl(pkgName);
        if (luaFunc != null)
        {
            luaFunc.Call(pkgName);
            luaFunc.Dispose();
            luaFunc = null;
        }
#endif
#endif
    }

    void Update()
    {
        if(_isCurrentLoading)
        {
            if(_waitLoadQueue.Count > 0)
            {
                int canLoadCount = MaxRequstCount - _currentLoadQueue.Count;
                while (canLoadCount > 0 && _waitLoadQueue.Count > 0)
                {
                    TaskLoader taskLoader = _waitLoadQueue[0];
                    _waitLoadQueue.RemoveAt(0);
                    _currentLoadQueue.Add(taskLoader);
                    taskLoader.Begin();
                    canLoadCount--;
                }
            }
        }
    }

    public void Load(string path, AssetBundleManager.LoadAssetCompleteHandler handler)
    {
        TaskLoader taskLoader = CreateLoadTaskLoader(path, ETaskType.abinfo);
        taskLoader.onAbInfoComplete += handler;

        AddWaitQueue(taskLoader);
    }

    public void Load(string path, LoadObjectCompleteHandler handler)
    {
        TaskLoader taskLoader = CreateLoadTaskLoader(path, ETaskType.asset);
        taskLoader.onAssetComplete += handler;

        AddWaitQueue(taskLoader);
    }

    public void Load(string path, LoadGameObjectCompleteHandler handler)
    {
        TaskLoader taskLoader = CreateLoadTaskLoader(path, ETaskType.gameobject);
        taskLoader.onGoComplete += handler;

        AddWaitQueue(taskLoader);
    }

    public void LoadGameObject(string path, LoadGameObjectCompleteHandler handler)
    {
        TaskLoader taskLoader = CreateLoadTaskLoader(path, ETaskType.gameobject);
        taskLoader.onGoComplete += handler;

        AddWaitQueue(taskLoader);
    }

    public void LoadScene(string sceneName, System.Action handler)
    {
#if AB_MODE
        string path = string.Format("Scene/{0}.unity", sceneName);
        Load(path, delegate (AssetBundleInfo info)
        {
            SceneManager.LoadScene(sceneName);
            if (handler != null)
                handler();
        });
#else
        SceneManager.LoadScene(sceneName);
        if (handler != null)
            handler();
#endif
    }

    public void LoadComplete(TaskLoader task)
    {
        _currentLoadQueue.Remove(task);
        if (_waitLoadQueue.Count <= 0)
            _isCurrentLoading = false;
    }

    public AssetObject GetCached(string path)
    {
        Tangzx.ABSystem.ObjectPool<AssetObject> aPool = null;
        if (_cachedAssetObj.TryGetValue(path, out aPool))
        {
            if (aPool.countInactive > 0)
            {
                AssetObject ao = aPool.Get();
                ao.CachedTransform.SetParent(null);
                return ao;
            }
        }
        return null;
    }

    public void Unload(AssetObject assetObj)
    {
        if(assetObj == null)
        {
            Debug.LogError("Unknow Destroy!");
            return;
        }
        Tangzx.ABSystem.ObjectPool<AssetObject> aPool = null;
        if(!_cachedAssetObj.TryGetValue(assetObj.key, out aPool))
        {
            aPool = new Tangzx.ABSystem.ObjectPool<AssetObject>(null, null);
        }
        if (aPool.countInactive > 5)
            GameObject.DestroyImmediate(assetObj.gameObject);
        else
        {
            assetObj.CachedTransform.SetParent(_cachedParent);
            aPool.Release(assetObj);
        }
    }

    private void AddWaitQueue(TaskLoader taskLoader)
    {
        if(!_waitLoadQueue.Contains(taskLoader))
            _waitLoadQueue.Add(taskLoader);
        _isCurrentLoading = true;
    }


    private TaskLoader CreateLoadTaskLoader(string abFileName, ETaskType t)
    {
        TaskLoader loader = null;

        if (_loaderCache.ContainsKey(abFileName))
        {
            loader = _loaderCache[abFileName];
        }
        else
        {
            loader = new TaskLoader(abFileName, t);
            _loaderCache[abFileName] = loader;
        }
        return loader;
    }
}
