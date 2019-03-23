using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class DownloadMgr : MonoBehaviour
{
    public class DownloadNode
    {
        public string url;
        public float progress;
    }

    private const int TimeOut = 5;
    private const int MaxDownloadNumber = 5;

    private List<string> ListidleUrl;
    private List<string> ListDownloadIng;
    private Dictionary<string, DownloadNode> dicDownload = new Dictionary<string, DownloadNode>(); 

    public DownLoadLocal LocalDownload = null;
    private static DownloadMgr Sole;
    public static DownloadMgr Instance
    {
        get
        {
            return Sole;
        }
    }

    // kb/s
    public float DownloadSpeed { get; private set; }

    private ulong totalByteCount = 0;
    private float startByteCountTime = 0;

    // Use this for initialization
    private void Awake()
    {
        Sole = this;
        LocalDownload = gameObject.AddUniqueCompoment<DownLoadLocal>();
        ListidleUrl = new List<string>();
        ListDownloadIng = new List<string>();
    }
    void Start()
    {

    
    }
    void LateUpdate()
    {
        if (ListDownloadIng.Count < MaxDownloadNumber)
        {
            if (ListidleUrl.Count > 0)
            {
                string StrThisUrl = ListidleUrl[0];
                ListDownloadIng.Add(StrThisUrl);
                ListidleUrl.RemoveAt(0);
            }
        }
    }

    public void DownloadText(string url, System.Action<string, string, bool, string> OverFun)
    {
        this.DownloadFile(url, OverFun, true);
    }

    public void DownloadFile(string url, System.Action<string, byte[], bool, string> OverFun, bool IsInsert)
    {
        StartCoroutine(IEDownloadFile(url, DownloadType.Byte, delegate (object obj, bool IsError, string Error)
          {
              byte[] tmpBytes = obj as byte[];
              if (OverFun != null)
                  OverFun(url, tmpBytes, IsError, Error);

          }, IsInsert));

    }
    public void DownloadFile(string url, System.Action<string, AssetBundle, bool, string> OverFun, bool IsInsert)
    {

        StartCoroutine(IEDownloadFile(url, DownloadType.AssetBundle, delegate (object obj, bool IsError, string Error)
        {
            AssetBundle tmpAB = obj as AssetBundle;
            if (OverFun != null)
                OverFun(url, tmpAB, IsError, Error);

        }, IsInsert));
    }
    public void DownloadFile(string url, System.Action<string, string, bool, string> OverFun, bool IsInsert)
    {
        StartCoroutine(IEDownloadFile(url, DownloadType.Text, delegate (object obj, bool IsError, string Error)
        {
            string tmpStr = obj as string;
            //文本读取默认会把\n转成\\n 
            if (!string.IsNullOrEmpty(tmpStr))
                tmpStr = tmpStr.Replace("\\n", "\n");
            if (OverFun != null)
                OverFun(url, tmpStr, IsError, Error);
        }, IsInsert));
    }
    public void DownloadFile(string url, System.Action<string, AudioClip, bool, string> OverFun, bool IsInsert)
    {
        StartCoroutine(IEDownloadFile(url, DownloadType.AudioClip, delegate (object obj, bool IsError, string Error)
        {
            AudioClip tmpAudioClip = obj as AudioClip;
            if (OverFun != null)
                OverFun(url, tmpAudioClip, IsError, Error);
        }, IsInsert));

    }
    public void DownloadFile(string url, System.Action<string, Texture2D,bool,string> OverFun, bool IsInsert)
    {
        StartCoroutine(IEDownloadFile(url, DownloadType.Texture, delegate (object obj, bool IsError, string Error)
        {
            Texture2D tmpTexture = obj as Texture2D;
            if (OverFun != null)
                OverFun(url, tmpTexture, IsError, Error);
        }, IsInsert));

    }
    public enum DownloadType
    {
        Byte=1,
        AssetBundle=2,
        Text=3,
        AudioClip=4,
        Texture=5
    }
    private IEnumerator IEDownloadFile(string url, DownloadType type, System.Action<object, bool, string> OverFun, bool IsInsert)
    {
        if (url != null && !ListidleUrl.Contains(url))
        {
            if (IsInsert) ListidleUrl.Insert(0, url);
            else ListidleUrl.Add(url);
        }
        while (!ListDownloadIng.Contains(url))
            yield return 0;
        object obj = null;
        DownloadNode loadNode = new DownloadNode();
        loadNode.url = url;
        loadNode.progress = 0;
        dicDownload[url] = loadNode;

        ulong preDownLoadBytes = 0;
        float waitTime = 0;
        bool timeOut = false;
        UnityWebRequest WebRequest = UnityWebRequest.Get(url);
        AsyncOperation ao = WebRequest.Send();
        while(!ao.isDone)
        {
            if(totalByteCount <= 0)
            {
                startByteCountTime = Time.realtimeSinceStartup;
            }
            totalByteCount += WebRequest.downloadedBytes - preDownLoadBytes;
            DownloadSpeed = (totalByteCount / 1000.0f) / (Time.realtimeSinceStartup - startByteCountTime);

            // 如果TimeOut时间内下载的数量小于0那么表示time out
            if (WebRequest.downloadedBytes - preDownLoadBytes <= 0)
            {
                if (waitTime <= 0.001f)
                    waitTime = Time.realtimeSinceStartup;
                else
                {
                    if(Time.realtimeSinceStartup - waitTime > TimeOut)
                    {
                        timeOut = true;
                        break;
                    }
                }
            }
            else
            {
                preDownLoadBytes = WebRequest.downloadedBytes;
                waitTime = 0;
            }
            yield return null;
        }
        if (WebRequest.isNetworkError || timeOut || WebRequest.responseCode != 200)
        {
            string errorContent = "";
            if (timeOut)
                errorContent = "TimeOut";
            else
                errorContent = WebRequest.error;
            RemoveDownloadList(url);
            WebRequest.Dispose();

            if (OverFun != null)
                OverFun(obj, true, errorContent);
            yield break;
        }
        switch (type)
        {
            case DownloadType.Byte:
                obj = WebRequest.downloadHandler.data;
                break;
            case DownloadType.AssetBundle:
                obj = DownloadHandlerAssetBundle.GetContent(WebRequest);
                break;
            case DownloadType.Text:
                obj = DownloadHandlerBuffer.GetContent(WebRequest);
                break;
            case DownloadType.AudioClip:
                obj = DownloadHandlerAudioClip.GetContent(WebRequest);
                break;
            case DownloadType.Texture:
                obj = DownloadHandlerTexture.GetContent(WebRequest);
                break;
        }
        WebRequest.Dispose();
        RemoveDownloadList(url);
        if (OverFun != null)
            OverFun(obj, false, null);
    }

    void RemoveDownloadList(string urlPath)
    {
        ListDownloadIng.Remove(urlPath);
        if (ListDownloadIng.Count <= 0)
            totalByteCount = 0;
    }


    // Update is called once per frame
    void Update()
    {

    }
}
