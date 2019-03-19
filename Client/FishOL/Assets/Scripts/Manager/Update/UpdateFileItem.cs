using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateFileItem  {
    public string filePath;
    public string hashCode;
    public bool needCopy;
    public float fileSize;

    public EFromType fromType = EFromType.none;
}

public enum EFromType
{
    none = 0,
    sdcard,
    app,
    web,
}

public class UpdateFile
{
    public EFromType fromType = EFromType.none;
    public Dictionary<string, UpdateFileItem> itemDic = new Dictionary<string, UpdateFileItem>();

    private string _content;

    public UpdateFile(string content, EFromType eType)
    {
        _content = content;
        fromType = eType;
        if(!string.IsNullOrEmpty(content))
            SplitData();
    }

    void SplitData()
    {
        itemDic.Clear();
        string[] filesStrs = _content.Split('\n');
        for (int i = 0; i < filesStrs.Length; i++)
        {
            string[] filePathItem = filesStrs[i].Split('|');
            if (filePathItem.Length < 2)
                continue;
            UpdateFileItem item = new UpdateFileItem();
            item.fromType = fromType;
            item.filePath = filePathItem[0];
            item.hashCode = filePathItem[1];
            //item.needCopy = int.Parse(filePathItem[2]) > 0;
            int needCopyIntVal = 0;
            if (!int.TryParse(filePathItem[2], out needCopyIntVal))
                needCopyIntVal = 1;
            item.needCopy = needCopyIntVal > 0;
            if (!float.TryParse(filePathItem[3], out item.fileSize))
                item.fileSize = 0;
            item.fileSize /= 1000.0f;
            itemDic[item.filePath] = item;
        }
    }

    public bool Equals(UpdateFileItem target)
    {
        UpdateFileItem item = GetItem(target.filePath);
        if(item != null)
        {
            if (item.hashCode.Equals(target.hashCode))
                return true;
        }
        return false;
    }

    public UpdateFileItem GetItem(string filePath)
    {
        UpdateFileItem item = null;
        if (itemDic.TryGetValue(filePath, out item))
        {
            return item;
        }
        return null;
    }

    public void AddItem(UpdateFileItem item)
    {
        itemDic[item.filePath] = item;
    }

    public void Save(string filePath)
    {
        FileHelp.WriteFile(_content, filePath);
    }

    public void SaveItem(string filePath)
    {
        List<string> lineList = new List<string>();
        foreach(var val in itemDic)
        {
            string strLine = string.Format("{0}|{1}|{2}|{3}", val.Value.filePath, val.Value.hashCode, val.Value.needCopy ? 1:0, val.Value.fileSize*1000);
            lineList.Add(strLine);
        }
        System.IO.File.WriteAllLines(filePath, lineList.ToArray());
    }
}