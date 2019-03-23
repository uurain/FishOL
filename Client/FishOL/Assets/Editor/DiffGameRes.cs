
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

public class DiffGameRes  {

    static List<string> paths = new List<string>();
    static List<string> files = new List<string>();

    static int def_forceUpdateVersion = 0;
    static string def_updateUrl = "";

    static string HaveUpdateText {
        get
        {
            return OldResPath + "HaveUpdate.txt";
        }
    }

    static string DLLName
    {
        get
        {
            return "Assembly-CSharp.dll";
        }
    }


    static string OldResPath
    {
        get
        {
            return Application.dataPath + "/../GameRes/";
        }
    }

    static string OldFileName
    {
        get
        {
            return OldResPath + UpdateMgr.filesName;
        }
    }

    static string NewResPath
    {
        get
        {
            return Application.dataPath + "/StreamingAssets/";
        }
    }

    static string NewFileName
    {
        get
        {
            return NewResPath + UpdateMgr.filesName;
        }
    }

    static string ServerLocalFilePath
    {
        get
        {
            string basePath = Application.dataPath + "/../GameResRemote/gameres/";
#if UNITY_ANDROID
            basePath += "android/";
#else
            basePath += "ios/";
#endif
            return basePath;
        }
    }

    [MenuItem("Custom/Build/Diff/DiffDLL")]
    static void OnBuildAndroidDll()
    {
        DiffGameRes.BuildAndroidDll("D:/Project_MMORPG/src/client/dt/boom_0915-1644_26916");
    }

    [MenuItem("Custom/Build/Diff/MoveServerConfig")]
    static void OnMoveServerLocalConfig()
    {
        DiffGameRes.MoveServerLocalConfig();
    }

    [MenuItem("Custom/Build/Diff/GetDiffTotalSize")]
    static void OnGetDiffTotalSize()
    {
        DiffGameRes.GetDiffTotalSize();
    }

    public static void BuildAllFiles(int version)
    {
        string newFilePath = NewFileName;
        if (File.Exists(newFilePath)) File.Delete(newFilePath);

        paths.Clear(); files.Clear();
        FileHelp.Recursive(NewResPath + "/lua", ref files, ref paths);
        FileHelp.Recursive(NewResPath + "/config", ref files, ref paths);

        files.Add(NewResPath + "/AssetBundles/UIUpdate.ab");
        files.Add(NewResPath + "/AssetBundles/dep.all");
        FileStream fs = new FileStream(newFilePath, FileMode.CreateNew);
        StreamWriter sw = new StreamWriter(fs);
        for (int i = 0; i < files.Count; i++)
        {
            string file = files[i];
            string ext = Path.GetExtension(file);
            if (file.EndsWith(".meta") || file.Contains(".DS_Store") || file.EndsWith(".manifest")) continue;

            string md5 = Util.md5file(file);
            string value = file.Replace(NewResPath + "/", string.Empty);
            int needCopy = 0;
            if (value.StartsWith("lua"))
            {
                // lua可以不需要拷贝 暂时先拷贝 后面再处理
                needCopy = 1;
            }
            else
            {
                needCopy = 1;
            }
            long fileSize = FileHelp.GetFileSize(file);
            sw.WriteLine(string.Format("{0}|{1}|{2}|{3}", value, md5, needCopy, fileSize));
        }


        List<string> abFilesList = GetAbFiles(NewResPath);
        for (int i = 0; i < abFilesList.Count; ++i)
        {
            sw.WriteLine(abFilesList[i]);
        }

        string dllRes = GetDllFiles();
        if(!string.IsNullOrEmpty(dllRes))
            sw.WriteLine(dllRes);

        sw.Close(); fs.Close();

        CreateDiffRes(version);

        EditorUtility.ClearProgressBar();
        AssetDatabase.Refresh();
    }

    public static void BuildAndroidDll(string projPath)
    {
        string dllPath = projPath + "/" + PlayerSettings.productName + "/src/main/assets/bin/Data/Managed/" + UpdateMgr.dllName;

        if (!FileHelp.FileExit(dllPath))
        {
            Debug.Log("dll null:" + dllPath);
            return;
        }
        // 暂时用一下最简单的加密方法
        byte[] bytes = File.ReadAllBytes(dllPath);
        bytes[0] += 1;
        File.WriteAllBytes(dllPath, bytes);

        string dllMd5 = Util.md5file(dllPath);
        long dllSize = FileHelp.GetFileSize(dllPath);
        string dllVal = string.Format("{0}|{1}|{2}|{3}", "Assembly-CSharp.dll", dllMd5, 0, dllSize);

        string[] allLines = File.ReadAllLines(OldFileName);
        if(allLines != null)
        {
            string lastLineStr = allLines[allLines.Length - 1];
            if(lastLineStr.IndexOf(UpdateMgr.dllName) >= 0)
            {
                string[] lastInfos = lastLineStr.Split('|');
                if(!lastInfos[1].Equals(dllMd5))
                {
                    allLines[allLines.Length - 1] = dllVal;
                    File.WriteAllLines(OldFileName, allLines);

                    if (!FileHelp.FileExit(HaveUpdateText))
                    {
                        int forceUpdateVersion;
                        int version = GetOldVersion(out forceUpdateVersion);
                        if(version < 0)
                        {
                            Debug.LogError("can't find old version！");
                        }
                        version++;
                        SetOldVersion(version, forceUpdateVersion);                        
                    }
                }
            }
            else
            {
                string[] newAllLines = new string[allLines.Length + 1];
                allLines.CopyTo(newAllLines, 0);
                newAllLines[newAllLines.Length - 1] = dllVal;
                File.WriteAllLines(OldFileName, newAllLines);
            }
            FileHelp.FileCopy(dllPath, OldResPath + UpdateMgr.dllName);
        }        
        else
        {
            Debug.LogError("not exit files!");
        }
        string gradleProjPath = projPath + "/" + PlayerSettings.productName + "/src/main/assets/";
        FileHelp.FileCopy(OldFileName, gradleProjPath + UpdateMgr.filesName);
        FileHelp.FileCopy(dllPath, gradleProjPath + UpdateMgr.dllName);

        FileHelp.DeleteFile(HaveUpdateText);

        Debug.Log("BuildAndroidDll sucess!");
    }

    public static void MoveServerLocalConfig()
    {
        int forceUpdateVersion;
        int version = GetOldVersion(out forceUpdateVersion);

        paths.Clear(); files.Clear();
        string sLocalPath = ServerLocalFilePath + version + "/";
        FileHelp.Recursive(OldResPath, ref files, ref paths);
        for(int i = 0; i < files.Count; ++i)
        {
            string file = files[i].Substring(files[i].IndexOf("GameRes") + "GameRes".Length);
            FileHelp.FileCopy(files[i], sLocalPath + file);
        }
        FileHelp.FileCopy(OldResPath + UpdateMgr.versionFileName, ServerLocalFilePath + UpdateMgr.versionFileName);
        Debug.Log("MoveServerLocalConfig sucess!");
    }

    static void SetOldVersion(int version, int forceUpdateVersion)
    {
        string content = string.Format("{0}|{1}|{2}", version, forceUpdateVersion, def_updateUrl);
        FileHelp.WriteFile(content, OldResPath + UpdateMgr.versionFileName);
    }

    static int GetOldVersion(out int forceUpdateVersion)
    {
        int version = 0;
        forceUpdateVersion = 0;
        if (File.Exists(OldResPath + UpdateMgr.versionFileName))
        {            
            string verStr = File.ReadAllText(OldResPath + UpdateMgr.versionFileName);
            string[] tempAry = verStr.Split('|');
            if (tempAry.Length > 0)
            {
                int tempVal;
                if (int.TryParse(tempAry[0], out tempVal))
                    version = tempVal;
            }
            if (tempAry.Length > 1)
            {
                int tempVal;
                if (int.TryParse(tempAry[1], out tempVal))
                    forceUpdateVersion = tempVal;
            }
            return version;
        }
        return -1;
    }

    static void CreateDiffRes(int version)
    {
        bool isNewPack = false;
        int forceUpdateVersion = 0;
        if (version == 0)
        {
            version = GetOldVersion(out forceUpdateVersion);
            if(version < 0)
            {
                isNewPack = true;
                version = 1;
            }
        }
        forceUpdateVersion = def_forceUpdateVersion;

        FileHelp.DeleteFile(HaveUpdateText);
        if (!isNewPack)
        {
            UpdateFile oldFile = GetUpdateFile(OldFileName);
            UpdateFile newFile = GetUpdateFile(NewFileName);

            bool haveNewFile = false;
            List<UpdateFileItem> waitUpdateList = new List<UpdateFileItem>();
            foreach (var val in newFile.itemDic)
            {
                if (!oldFile.Equals(val.Value))
                {
                    FileHelp.FileCopy(NewResPath + val.Value.filePath, OldResPath + val.Value.filePath);
                    haveNewFile = true;
                    UpdateProgress(1, 2, "copy to:" + val.Value.filePath);
                }
            }
            if (haveNewFile)
            {
                version++;
                FileHelp.WriteFile(new byte[] { 0 }, HaveUpdateText);
            }
        }
        FileHelp.WriteFile(version.ToString(), NewResPath + UpdateMgr.versionFileName);
        SetOldVersion(version, forceUpdateVersion);
        FileHelp.FileCopy(NewFileName, OldFileName);

        UnityEngine.Debug.Log("BuildAllFiles sucess, version:" + version);
    }

    static UpdateFile GetUpdateFile(string filePath)
    {
        if (File.Exists(filePath))
        {
            return new UpdateFile(File.ReadAllText(filePath), EFromType.none);
        }
        else
        {
            return new UpdateFile("", EFromType.none);
        }
    }

    static string GetDllFiles()
    {
        string dllPath = OldResPath + DLLName;
        if(FileHelp.FileExit(dllPath))
        {
            string dllMd5 = Util.md5file(dllPath);
            long dllSize = FileHelp.GetFileSize(dllPath);
            string dllVal = string.Format("{0}|{1}|{2}|{3}", DLLName, dllMd5, 1, dllSize);
            return dllVal;
        }
        return "";
    }

    static List<string> GetAbFiles(string resPath)
    {
        List<string> dataList = new List<string>();
        string depPath = resPath + "AssetBundles/dep.all";
        Tangzx.ABSystem.AssetBundleDataReader _depInfoReader = null;
        FileStream fs = new FileStream(depPath, FileMode.Open);
        if (fs.Length > 4)
        {
            BinaryReader br = new BinaryReader(fs);
            if (br.ReadChar() == 'A' && br.ReadChar() == 'B' && br.ReadChar() == 'D')
            {
                if (br.ReadChar() == 'T')
                    _depInfoReader = new Tangzx.ABSystem.AssetBundleDataReader();
                else
                    _depInfoReader = new Tangzx.ABSystem.AssetBundleDataBinaryReader();

                fs.Position = 0;
                _depInfoReader.Read(fs);
            }
        }
        fs.Close();

        if (_depInfoReader != null)
        {
            foreach (var val in _depInfoReader.infoMap)
            {
                string filePath = resPath + "AssetBundles/" + val.Value.fullName;
                long fileSize = FileHelp.GetFileSize(filePath);
                string md5Str = Util.md5file(filePath);
                string content = string.Format("AssetBundles/{0}|{1}|{2}|{3}", val.Value.fullName, md5Str, 0, fileSize);
                dataList.Add(content);
            }
        }

        return dataList;
    }

    static void UpdateProgress(int progress, int progressMax, string desc)
    {
        string title = "Processing...[" + progress + " - " + progressMax + "]";
        float value = (float)progress / (float)progressMax;
        EditorUtility.DisplayProgressBar(title, desc, value);
    }

    static void GetDiffTotalSize()
    {
        string path1 = Application.dataPath + "/../files_old.txt";
        string path2 = Application.dataPath + "/../files_new.txt";

        if (!File.Exists(path1))
        {
            Debug.LogError(path1 + " not find!");
        }
        if (!File.Exists(path2))
        {
            Debug.LogError(path2 + " not find!");
        }
        UpdateFile oldFiles = new UpdateFile(System.IO.File.ReadAllText(path1), EFromType.none);
        UpdateFile newFiles = new UpdateFile(System.IO.File.ReadAllText(path2), EFromType.none);
        double totalSize = 0;
        foreach (var val in newFiles.itemDic)
        {
            if (!oldFiles.Equals(val.Value))
            {
                totalSize += val.Value.fileSize;
                Debug.Log("change file:" + val.Value.filePath);
            }
        }
        Debug.LogFormat("tDiffTotalSize:{0}MB", totalSize/1000);
    }
}