using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Collections.Generic;

public class FileHelp  {

	public static string[]  GetFileLines(string filePath)
    {
        string[] lines = System.IO.File.ReadAllLines(filePath);
        return lines;
    }

    public static void Recursive(string path, ref List<string> files, ref List<string> paths, string searchPattern = "")
    {
        string[] names = searchPattern == "" ? Directory.GetFiles(path) : Directory.GetFiles(path, searchPattern);
        string[] dirs = Directory.GetDirectories(path);
        foreach (string filename in names)
        {
            string ext = Path.GetExtension(filename);
            if (ext.Equals(".meta")) continue;
            files.Add(filename.Replace('\\', '/'));
        }
        foreach (string dir in dirs)
        {
            paths.Add(dir.Replace('\\', '/'));
            Recursive(dir, ref files, ref paths, searchPattern);
        }
    }

    /// <summary>
    /// 根据路径删除文件
    /// </summary>
    /// <param name="path"></param>
    public static void DeleteFile(string path)
    {
        if (File.Exists(path))
            File.Delete(path);
    }


    public static void WriteFile(byte[] data, string filePath)
    {
        string dirPath = Util.GetDirName(filePath);
        if (!Directory.Exists(dirPath))
            Directory.CreateDirectory(dirPath);

        File.WriteAllBytes(filePath, data);
    }

    public static void WriteFile(string content, string filePath)
    {
        string dirPath = Util.GetDirName(filePath);
        if (!Directory.Exists(dirPath))
            Directory.CreateDirectory(dirPath);

        File.WriteAllText(filePath, content);
    }

    public static bool FileCopy(string sourceFileName, string destFileName)
    {
        if (!FileHelp.FileExit(sourceFileName))
            return false;
        string dirPath = Util.GetDirName(destFileName);
        if (!Directory.Exists(dirPath))
            Directory.CreateDirectory(dirPath);
        File.Copy(sourceFileName, destFileName, true);
        return true;
    }

    public static byte[] ReadFile(string filePath)
    {
        using (FileStream fsRead = new FileStream(filePath, FileMode.Open))
        {
            int fsLen = (int)fsRead.Length;
            byte[] heByte = new byte[fsLen];
            int r = fsRead.Read(heByte, 0, heByte.Length);
            return heByte;
        }
    }

    public static bool FileExit(string filePath)
    {
        return File.Exists(filePath);
    }

    public static long GetFileSize(string sFullName)
    {
        long lSize = 0;
        if (File.Exists(sFullName))
            lSize = new FileInfo(sFullName).Length;
        return lSize;
    }

    /// <summary>
    /// 删除文件夹（及文件夹下所有子文件夹和文件）
    /// </summary>
    /// <param name="directoryPath"></param>
    public static void DeleteFolder(string srcPath)
    {
        Debug.Log("DeleteFolder:" + srcPath);
        if (!Directory.Exists(srcPath))
            return;

        try
        {
            DirectoryInfo dir = new DirectoryInfo(srcPath);
            FileSystemInfo[] fileinfo = dir.GetFileSystemInfos();  //返回目录中所有文件和子目录
            foreach (FileSystemInfo i in fileinfo)
            {
                if (i is DirectoryInfo)            //判断是否文件夹
                {
                    DirectoryInfo subdir = new DirectoryInfo(i.FullName);
                    subdir.Delete(true);          //删除子目录和文件
                }
                else
                {
                    File.Delete(i.FullName);      //删除指定文件
                }
            }
        }
        catch (Exception e)
        {
            throw;
        }
    }
}
