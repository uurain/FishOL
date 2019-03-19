using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownLoadLocal : MonoBehaviour {

    private const int TimeOut = 5;
    private const int MaxDownloadNumber = 2000;

    private List<string> ListidleUrl;
    private List<string> ListDownloadIng;

    // Use this for initialization
    private void Awake()
    {
        ListidleUrl = new List<string>();
        ListDownloadIng = new List<string>();
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
    public void DownloadFile(string url, System.Action<string, Texture2D, bool, string> OverFun, bool IsInsert)
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
        Byte = 1,
        AssetBundle = 2,
        Text = 3,
        AudioClip = 4,
        Texture = 5
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
        WWW WebRequest = new WWW(url);
        yield return WebRequest;
        if (!string.IsNullOrEmpty(WebRequest.error))
        {
            if (OverFun != null)
                OverFun(obj, true, WebRequest.error);
            yield break;
        }
        switch (type)
        {
            case DownloadType.Byte:
                obj = WebRequest.bytes;
                break;
            case DownloadType.AssetBundle:
                obj = WebRequest.assetBundle;
                break;
            case DownloadType.Text:
                obj = WebRequest.text;
                break;
            case DownloadType.AudioClip:
                obj = WebRequest.GetAudioClip();
                break;
            case DownloadType.Texture:
                obj = WebRequest.texture;
                break;
        }
        WebRequest.Dispose();
        ListDownloadIng.Remove(url);
        if (OverFun != null)
            OverFun(obj, false, null);
    }
}
