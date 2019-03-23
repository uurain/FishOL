using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DownLoadBreakResume{

    static string CachedFile
    {
        get
        {
            return Util.DataPath + "resume.txt";
        }
    }

    public static void ClearCached()
    {
        FileHelp.DeleteFile(CachedFile);
    }

    UpdateFile resumeFile;
    int saveDist = 0;

    public DownLoadBreakResume()
    {
        if(File.Exists(CachedFile))
            resumeFile = new UpdateFile(File.ReadAllText(CachedFile), EFromType.none);
    }

    public void AddFileCached(UpdateFileItem fileItem)
    {
        if (resumeFile == null)
            resumeFile = new UpdateFile("", EFromType.none);
        resumeFile.AddItem(fileItem);

        if(++saveDist > 3)
        {// 10个记录一次
            saveDist = 0;
            resumeFile.SaveItem(CachedFile);
        }        
    }

    public bool Equals(UpdateFileItem target)
    {
        if(resumeFile != null)
        {
            return resumeFile.Equals(target);
        }
        return false;
    }

    public void Clear()
    {
        resumeFile = null;
        ClearCached();
    }
}
