using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UpdateMgr : Singleton<UpdateMgr> {

    public enum EUpdateErrorCode
    {
        sucess = 0,
        debugMode,
        netError,
        nullFile, // 找不到更新的文件，需要查看files.txt

    }

    public const string Text1 = "发现新版本,请立即前往商店更新";
    public const string Text2 = "请尝试开启网络后重启游戏";
    public const string Text3 = "当前为wifi环境下，请放心下载";
    public const string Text4 = "请在wifi环境下下载";
    public const string Text5 = "获取版本失败!";
    public const string Text6 = "获取更新文件失败!";
    public const string Text7 = "发现版本更新，本次更新文件大小为{0:0.00}MB，是否更新？";
    public const string Text8 = "下载资源文件失败";
    public const string Text9 = "正在解压中，请耐心等候，不消耗流量";
    public const string Text10 = "正在更新:{0:0.00}MB/{1:0.00}MB 速度:{2:0.00}KB/S";
    public const string Text11 = "加载中...";

    public const int NoTipUpdateSize = 5;

    public static string dllName = "Assembly-CSharp.dll";
    public static string versionFileName = "version.txt";
    public static string filesName = "files.txt";
    public static string UpdateUrlTxt = "updateurl.txt";
    const string uiUpdateFilePath = "AssetBundles/UIUpdate.ab";

    string RandTime
    {
        get
        {
            return "?" + Time.time;
        }
    }

    public Action ActionComplete = null;
    public Action<string> UpdateProcess = null;
    public Action<string> ErrorAction = null;

    private string updateWebFullUrl = "";
    private string updateWebUrl = "";

    int localVersion = 0;
    int remoteVersion = 0;
    int forceUpdateAppVersion = 0;
    int appInsideVersion = 0;

    int newVersion = 0;

    int checkUpdateInfoProgress = 0;
    DownloadManager downloadMgr = null;

    UpdateFile oldFiles = null;
    UpdateFile newFiles = null;
    DownLoadBreakResume fileBreakResume = null;
    bool androidRestart = false;


    bool isComplete = false;
    bool isWaitShowUI = false;
    UIUpdate _uiUpdate = null;
    string _uiUpdatePackName = "";

    private string PathPrefix
    {
        get
        {
            if (Application.isEditor)
            {
                return @"file:///";
            }
            return "";
        }
    }

    public void Begin()
    {
        if (downloadMgr == null)
            downloadMgr = AppFacade.Instance.GetManager<DownloadManager>(ManagerName.Download);

        isComplete = false;
        if (AppConst.DebugMode)
        {
            End(EUpdateErrorCode.debugMode);
            return;
        }        
        androidRestart = false;
        checkUpdateInfoProgress = 0;
        oldFiles = null;
        newFiles = null;
        newVersion = 0;
        localVersion = -1;
        remoteVersion = -1;
        appInsideVersion = -1;

        ShowUI(LoadConfig, null);
    }

    void LoadConfig()
    {        
        localVersion = GetLocalVersion();
        if (AppConst.UpdateMode)
        {
            LoadLocalUpdateUrl();
            LoadRemoteVersion();
        }
        else
        {
            CheckUpdateInfo();
        }
        Util.UpdatePlayerLog(1003, localVersion.ToString());
        downloadMgr.LocalDownload.DownloadFile(PathPrefix + Util.AppContentPath() + versionFileName, delegate (string url, string content, bool isError, string error)
        {
            if (isError)
            {
                OnErrorAciton("Download app inside Version error:" + error);
            }
            int tempVal;
            if (int.TryParse(content, out tempVal))
                appInsideVersion = tempVal;
            CheckUpdateInfo();
        }, true);
    }

    void LoadRemoteVersion()
    {
        string remoteVersionPath = updateWebFullUrl + versionFileName;
        if (AppConst.DebugUpdateVersion)
            remoteVersionPath = updateWebFullUrl + "debugversion.txt";
        DownloadRemoteFile(remoteVersionPath + RandTime, delegate (string url, string content, bool isError, string error)
        {
            if (isError)
            {
                End(EUpdateErrorCode.netError, Text5);
            }
            else
            {
                CheckRemoteVersion(content);
            }
        }, true);
    }

    void CheckRemoteVersion(string remoteVersionContent)
    {
        if(!string.IsNullOrEmpty(remoteVersionContent))
        {
            string[] tempAry = remoteVersionContent.Split('|');
            if(tempAry.Length > 0)
            {
                int tempVal;
                if (int.TryParse(tempAry[0], out tempVal))
                    remoteVersion = tempVal;
                else
                    remoteVersion = 0;
            }
            if(tempAry.Length > 1)
            {
                int tempVal;
                if (int.TryParse(tempAry[1], out tempVal))
                    forceUpdateAppVersion = tempVal;
            }
            if (tempAry.Length > 2)
            {
                if (tempAry[2] != "" && updateWebUrl != tempAry[2])
                {
                    FileHelp.WriteFile(tempAry[2], Util.DataPath + UpdateUrlTxt);
                    updateWebFullUrl = tempAry[2];
                    updateWebUrl = updateWebFullUrl;
                    UpdatePlatformUpdateUrl();
                    LoadRemoteVersion();
                    return;
                }
            }
            CheckUpdateInfo();
        }
    }

    void CheckUpdateInfo()
    {
        checkUpdateInfoProgress++;
        if(checkUpdateInfoProgress >= 2)
        {            
            //所有版本检查完毕 开始判断
            if(localVersion >= appInsideVersion && localVersion >= remoteVersion)
            {
                newVersion = localVersion;
                // 已经是最新了
                End();
            }
            else if(appInsideVersion > localVersion && appInsideVersion >= remoteVersion)
            {
                // 新装了一个更新的版本号app
                // 删除sd卡中的dll
                //DeleteLocalDll();
                newVersion = appInsideVersion;
                oldFiles = new UpdateFile(GetLocalFiles(), EFromType.sdcard);
                downloadMgr.LocalDownload.DownloadFile(PathPrefix + Util.AppContentPath() + filesName, delegate (string url, string content, bool isError, string error)
                {
                    if (isError)
                    {
                        OnErrorAciton("Download app inside filesName error:" + error);
                        content = "";
                    }
                    newFiles = new UpdateFile(content, EFromType.app);
                    CheckFiles();
                }, true);
            }
            else
            {
                newVersion = remoteVersion;
                if(localVersion < forceUpdateAppVersion && appInsideVersion < forceUpdateAppVersion)
                {
                    ShowForceUpdateTip();
                    return;
                }
                if(localVersion >= appInsideVersion)
                {
                    // sdcard中已经有第二大的版本了,后面只要下载最新的文件就即可
                    oldFiles = new UpdateFile(GetLocalFiles(), EFromType.sdcard);
                }
                else
                {
                    // app中已经有第二大的版本了,需要移动app到sdcard（服务器不更新的文件但是又需要移动sdcard中）
                    downloadMgr.LocalDownload.DownloadFile(PathPrefix + Util.AppContentPath() + filesName, delegate (string url, string content, bool isError, string error)
                    {
                        if (isError)
                        {
                            OnErrorAciton("Download app inside filesName error:" + error);
                            content = "";
                        }
                        oldFiles = new UpdateFile(content, EFromType.app);
                        CheckFiles();
                    }, true);
                }

                DownloadRemoteFile(updateWebFullUrl + remoteVersion + "/" + filesName, delegate (string url, string content, bool isError, string error)
                {
                    if (isError)
                    {
                        //OnErrorAciton("Download Remote filesName error:" + error);
                        End(EUpdateErrorCode.netError, Text6);
                        content = "";
                        return;
                    }
                    newFiles = new UpdateFile(content, EFromType.web);
                    CheckFiles();
                }, true);
            }
        }
    }

    void CheckFiles()
    {
        if(newFiles == null || oldFiles == null)
        {
            return;
        }
        List<UpdateFileItem> waitUpdateList = new List<UpdateFileItem>();
        foreach(var val in newFiles.itemDic)
        {
            if (!oldFiles.Equals(val.Value))
            {
                // 如果不相等，从web下载的必须加进去，
                // app内部的:1.如果sdcard有文件那么直接加Download erro进去。2.如果sdcard本地没有文件根据needCopy来判断是否需要拷贝
                if(val.Value.fromType == EFromType.web || val.Value.needCopy || FileHelp.FileExit(Util.DataPath + val.Value.filePath))
                    waitUpdateList.Add(val.Value);
            }
            else
            {
                if (oldFiles.fromType == EFromType.app && (val.Value.needCopy || FileHelp.FileExit(Util.DataPath + val.Value.filePath)))
                {
                    // web有，app内部也有的时候，如果相等那么优先从app内部拷贝,这种情况本地正常情况下是没有的
                    // app内部的:1.如果sdcard有文件那么直接加进去。2.如果sdcard本地没有文件根据needCopy来判断是否需要拷贝
                    waitUpdateList.Add(oldFiles.GetItem(val.Value.filePath));
                }
            }
        }
        if(waitUpdateList.Count <= 0)
        {
            End();
            return;
        }

        List<UpdateFileItem> waitAppList = new List<UpdateFileItem>();
        List<UpdateFileItem> waitRemoteList = new List<UpdateFileItem>();
        for (int i = 0; i < waitUpdateList.Count; ++i)
        {
            if (waitUpdateList[i].fromType == EFromType.app)
                waitAppList.Add(waitUpdateList[i]);
            else if(waitUpdateList[i].fromType == EFromType.web)
                waitRemoteList.Add(waitUpdateList[i]);
            else
            {
                Debug.LogError("update other type!!!!");
            }
        }
        // 优先拷贝本地资源
        if(waitAppList.Count > 0)
        {
            DownLoadBreakResume.ClearCached();
            downloadMgr.StartCoroutine( BeginUpdateRes(EFromType.app, waitAppList, delegate() {
                BeginRemoteRes(waitRemoteList);
            }));
        }
        else
            BeginRemoteRes(waitRemoteList);
    }

    // 后下载远程资源
    void BeginRemoteRes(List<UpdateFileItem> waitRemoteList)
    {
        if(waitRemoteList.Count > 0)
        {
            float totalSize = 0;
            for(int i = 0; i < waitRemoteList.Count; ++i)
            {
                totalSize += waitRemoteList[i].fileSize;
            }
            //mb
            totalSize /= 1000.0f;
            if (totalSize < 0.01f)
                totalSize = 0.01f;
            if(totalSize < NoTipUpdateSize)
            {
                OnConfirmUpdate(waitRemoteList);
            }
            else
            {
                string tip1 = string.Format(Text7, totalSize);
                string tip2;
                if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
                    tip2 = Text3;
                else
                    tip2 = Text4;
                _uiUpdate.ShowCheckBox(tip1, tip2, OnCancelUpdate, delegate ()
                {
                    OnConfirmUpdate(waitRemoteList);
                });
            }
        }
        else
        {
            End(EUpdateErrorCode.sucess);
        }
    }

    IEnumerator BeginUpdateRes(EFromType fromType, List<UpdateFileItem> waitUpdateList, Action comptAction)
    {
        yield return null;
        // 单位kb
        double totalSize = 0;
        double curGetSize = 0;
        float beginTime = Time.realtimeSinceStartup;
        for(int i = 0; i < waitUpdateList.Count; ++i)
        {
            totalSize += waitUpdateList[i].fileSize;
        }        
        int completeIndex = 0;
        int maxCount = waitUpdateList.Count;
        for (int i = 0; i < waitUpdateList.Count; ++i)
        {
            UpdateFileItem item = waitUpdateList[i];
            Action<string, byte[], bool, string> _actionComplete = delegate (string url, byte[] bytes, bool isError, string error)
            {                
                if (isError)
                {
                    if (Application.platform == RuntimePlatform.Android && item.fromType == EFromType.app && item.filePath.EndsWith(dllName))
                    {
                        // 如果dll拷贝失败就算了
                    }
                    else
                    {
                        Debug.LogError("Download error:" + url + ":" + error);
                        End(EUpdateErrorCode.netError, Text8);
                        return;
                    }
                }
                else
                {
                    if(bytes != null)
                    {
                        //Debug.Log("write file2:" + Application.persistentDataPath + "/" + item.filePath);
                        //LogMgr.Instance.Log("write file:" + item.fromType.ToString() + ":" + bytes.Length + ":" + Util.DataPath + item.filePath);
                        FileHelp.WriteFile(bytes, Util.DataPath + item.filePath);
                        if (item.fromType == EFromType.web)
                        {
                            if (fileBreakResume != null)
                            {
                                fileBreakResume.AddFileCached(item);
                            }
                        }
                    }                
                    if(Application.platform == RuntimePlatform.Android)
                    {
                        if (item.filePath.EndsWith(dllName))
                        {
                            if (item.fromType == EFromType.web)
                            {
                                androidRestart = true;
                            }
                            else if (item.fromType == EFromType.app)
                            {
                                // 如果是从app内部拷贝的dll 第一次启动游戏的时候不拷贝
                                if(PlayerPrefs.GetInt("check_dll_first", 0) > 0)
                                {
                                    androidRestart = true;
                                }
                                else
                                {
                                    PlayerPrefs.SetInt("check_dll_first", 1);
                                }
                            }                            
                        }
                    }
                }
                completeIndex++;
                curGetSize += item.fileSize;
                double speed = curGetSize / (Time.realtimeSinceStartup - beginTime);
                //if (item.fromType == EFromType.web)
                //    speed = downloadMgr.DownloadSpeed;
                OnUpdateProgress(fromType, completeIndex, maxCount, speed, curGetSize, totalSize);
                if (completeIndex == maxCount)
                {
                    comptAction();
                }
            };
            switch (item.fromType)
            {
                case EFromType.app:
                    if(Application.platform == RuntimePlatform.Android)
                    {
                        string srcPath = Util.AppContentPath() + item.filePath;
                        downloadMgr.LocalDownload.DownloadFile(srcPath, _actionComplete, false);
                    }
                    else
                    {
                        string srcPath = Util.AppContentNoFilePath() + item.filePath;
                        bool sucess = FileHelp.FileCopy(srcPath, Util.DataPath + item.filePath);
                        if (Application.isEditor && item.filePath.EndsWith(dllName))
                        {
                            _actionComplete(srcPath, null, false, "");
                        }
                        else
                            _actionComplete(srcPath, null, !sucess, "");
                        yield return new WaitForEndOfFrame();
                    }                    
                    break;
                case EFromType.web:
                    {
                        string srcPath = updateWebFullUrl + remoteVersion  + "/" + item.filePath;
                        if (fileBreakResume != null)
                        {
                            if(fileBreakResume.Equals(item))
                            {
                                _actionComplete(srcPath, null, false, "");
                                continue;
                            }
                        }
                        DownloadRemoteFile(srcPath, _actionComplete, false);
                    }                    
                    break;
            }
        }
    }

    void DownloadRemoteFile(string fileUrl, System.Action<string, byte[], bool, string> OverFun, bool IsInsert, int reConnect = 2)
    {
        downloadMgr.DownloadFile(fileUrl, delegate (string url, byte[] bytes, bool isError, string error) {
            if(isError)
            {
                if(reConnect <= 0)
                {
                    OverFun(url, bytes, isError, error);
                    return;
                }
                reConnect -= 1;
                DownloadRemoteFile(fileUrl, OverFun, IsInsert, reConnect);
                return;
            }
            OverFun(url, bytes, isError, error);
        }, IsInsert);
    }

    void DownloadRemoteFile(string fileUrl, System.Action<string, string, bool, string> OverFun, bool IsInsert, int reConnect = 1)
    {
        downloadMgr.DownloadFile(fileUrl, delegate (string url, string content, bool isError, string error)
        {
            if (isError)
            {
                if (reConnect <= 0)
                {
                    OverFun(url, content, isError, error);
                    return;
                }
                reConnect -= 1;
                DownloadRemoteFile(fileUrl, OverFun, IsInsert, reConnect);
                return;
            }
            OverFun(url, content, isError, error);
        }, IsInsert);
    }

    int GetLocalVersion()
    {
        int localVersion = -1;
        string localPath = Util.DataPath + versionFileName;
        if (FileHelp.FileExit(localPath))
        {
            string versionStr = Util.GetFileText(localPath);
            if (!string.IsNullOrEmpty(versionStr))
            {
                if(!int.TryParse(versionStr, out localVersion))
                {
                    localVersion = -1;
                }
            }
        }
        return localVersion; 
    }

    void LoadLocalUpdateUrl()
    {
        string localPath = Util.DataPath + UpdateUrlTxt;
        if (FileHelp.FileExit(localPath))
        {
            updateWebFullUrl = Util.GetFileText(localPath);
        }
        if (string.IsNullOrEmpty(updateWebFullUrl))
        {
            updateWebFullUrl = AppConst.WebUrl;
        }
        updateWebUrl = updateWebFullUrl;
        UpdatePlatformUpdateUrl();
    }

    void UpdatePlatformUpdateUrl()
    {
        if (UnityEngine.Application.platform == RuntimePlatform.IPhonePlayer)
        {
            updateWebFullUrl += "gameres/ios/";
        }
        else
        {
            updateWebFullUrl += "gameres/android/";
        }
    }

    string GetLocalFiles()
    {
        string localFiles = "";
        string localPath = Util.DataPath + filesName;
        if (FileHelp.FileExit(localPath))
        {
            localFiles = Util.GetFileText(localPath);
        }
        return localFiles;
    }

    void DeleteLocalDll()
    {
        string localPath = Util.DataPath + dllName;
        FileHelp.DeleteFile(localPath);
    }

    void ClearBreakResume()
    {

    }

    void End(EUpdateErrorCode errorCode = EUpdateErrorCode.sucess, string errorStr = "")
    {          
        if (errorCode == EUpdateErrorCode.sucess)
        {
            if (fileBreakResume != null)
            {
                fileBreakResume.Clear();
            }
            DownLoadBreakResume.ClearCached();

            if (newVersion > 0)
                FileHelp.WriteFile(newVersion.ToString(), Util.DataPath + versionFileName);
            if(newFiles != null)
                newFiles.Save(Util.DataPath + filesName);
            // 暂时用一下这个
            AppConst.SocketPort = newVersion;
            Debug.Log("update res complete");
            if(androidRestart)
            {
                PlatformInstance.RestartActivity(0);
                return;
            }
            Util.UpdatePlayerLog(1004);
        }
        else if (errorCode == EUpdateErrorCode.debugMode)
        {
            AppConst.SocketPort = -1;
        }
        else if(errorCode == EUpdateErrorCode.netError)
        {
            OnErrorAciton("update res error:" + errorCode + ":" + errorStr);
            _uiUpdate.ShowCheckBox(errorStr, Text2, OnCancelUpdate, OnCancelUpdate);
            return;
        }
        else
        {
            OnErrorAciton("update res error:" + errorCode + ":" + errorStr);
        }
        //HideUI();
        if (ActionComplete != null)
            ActionComplete();
    }

    public void HideUI()
    {
        if (_uiUpdate != null)
        {
            _uiUpdate.Dispose();
            FairyGUI.UIPackage.RemovePackage(_uiUpdatePackName);
            _uiUpdate = null;
        }
    }

    public void SetPrg(float val)
    {
        if (_uiUpdate != null)
        {
            _uiUpdate.SetProgress(val);
        }
    }

    public void SetPrgText(string val)
    {
        if (_uiUpdate != null)
        {
            _uiUpdate.SetText(val);
        }
    }

    void OnErrorAciton(string errorLog)
    {
        Debug.LogError("OnErrorAciton:" + errorLog);
        if (ErrorAction != null)
            ErrorAction(errorLog);
    }

    void OnUpdateProgress(EFromType fromType, int curIndex, int maxCount, double speed, double curGetSize, double totalSize)
    {
        //Debug.Log(string.Format("OnUpdateProgress:{0}/{1}", curIndex, maxCount));
        if(_uiUpdate != null)
        {
            if (fromType == EFromType.app)
                SetPrgText(Text9);
            //_uiUpdate.SetText(string.Format("正在拷贝资源,进度:{0}/{1},速度:{2:0.00}kb/秒", curIndex, maxCount, speed));
            else
            {
                curGetSize /= 1000.0f;
                totalSize /= 1000.0f;
                if (totalSize < 0.01f)
                    totalSize = 0.01f;
                if (curGetSize < 0.01f)
                    curGetSize = 0.01f;
                SetPrgText(string.Format(Text10, curGetSize, totalSize, speed));
            }
            SetPrg((float)curIndex / maxCount * 100);
        }
    }


    public void ShowUI(System.Action completeFunc, LuaInterface.LuaFunction luaFunc)
    {
        if (isWaitShowUI)
            return;
        if(_uiUpdate != null)
        {
            if (completeFunc != null)
                completeFunc();
            if (luaFunc != null)
            {
                luaFunc.Call();
                luaFunc.Dispose();
            }
            return;
        }
        string uiAbPath = Util.DataPath + uiUpdateFilePath;
        if (!FileHelp.FileExit(uiAbPath))
        {
            uiAbPath = Util.AppContentPath() + uiUpdateFilePath;
        }        
        else
        {
            if(Application.isMobilePlatform)
                uiAbPath = "file://" + uiAbPath;
        }
        isWaitShowUI = true;
        // 这里不能直接assetbundle，具体原因待查!
        downloadMgr.LocalDownload.DownloadFile(PathPrefix + uiAbPath, delegate (string url, byte[] bytes, bool isError, string error)
        {
            isWaitShowUI = false;
            if (!isError)
            {
                if(isComplete)
                {
                    bytes = null;
                    return;
                }
                try
                {
                    AssetBundle ab = AssetBundle.LoadFromMemory(bytes);
                    _uiUpdatePackName = FairyGUI.UIPackage.AddPackage(ab).name;
                    FairyGUI.GObject uiGobj = FairyGUI.UIPackage.CreateObject(_uiUpdatePackName, "ui_Update");
                    _uiUpdate = new UIUpdate();

                    _uiUpdate.contentPane = uiGobj.asCom;
                    _uiUpdate.Show();
                    SetPrgText(Text11);

                    if (completeFunc != null)
                        completeFunc();
                    if(luaFunc != null)
                    {
                        luaFunc.Call();
                        luaFunc.Dispose();
                    }
                }
                catch(Exception exp)
                {
                    FileHelp.DeleteFile(Util.DataPath + uiUpdateFilePath);
                    Debug.LogError("loadUpdateuierror:" + exp.ToString());
                }
            }
            else
            {
                OnErrorAciton("load uiupdate error:" + error);
            }
        }, true);
    }

    void ShowForceUpdateTip()
    {
        _uiUpdate.ShowCheckBox(Text1, "", OnCancelUpdate, OnCancelUpdate);
    }

    void OnConfirmUpdate(List<UpdateFileItem> waitRemoteList)
    {
        fileBreakResume = new DownLoadBreakResume();
        downloadMgr.StartCoroutine(BeginUpdateRes(EFromType.web, waitRemoteList, delegate ()
        {
            End(EUpdateErrorCode.sucess);
        }));
    }

    void OnCancelUpdate()
    {
        Application.Quit();
    }
}
