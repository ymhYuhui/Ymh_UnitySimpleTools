using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

public class JsonOperation
{

    public static string GetFilePath(string fileName)
    {
        string filePath = "";
#if UNITY_ANDROID
            filePath = Application.persistentDataPath + "/" + fileName + ".json";
#endif
#if UNITY_EDITOR
        filePath = "Assets/Resources/" + fileName + ".json";
#endif
        Debug.Log("filePath" + filePath);
        return filePath;
    }


    /// <summary>
    /// 这个方法是针对安卓端路径，需要保存至 路径下persistentDataPath不可以从pc端直接打包过去，所以在第一次读取是将本地存储的文件复制过去
    /// </summary>
    /// <param name="fileName"></param>
    public static void FirstLoad(string fileName)
    {

        TextAsset t = (TextAsset)Resources.Load(fileName);
        string json = t.text.ToString().Trim();
        FileStream fs = new FileStream(GetFilePath(fileName), FileMode.Create);
        byte[] bytes = new UTF8Encoding().GetBytes(json.ToString());
        fs.Write(bytes, 0, bytes.Length);
        fs.Close();
    }

    public static T ReadFile<T>(string fileName)
    {

        FileInfo t = new FileInfo(fileName);
        if (!t.Exists)
        {
            Debug.Log("can not find file");
        }
        StreamReader sr = null;

        sr = File.OpenText(GetFilePath(fileName));

        string json = sr.ReadToEnd();

        T GameDataByJson = JsonUtility.FromJson<T>(json);
        sr.Close();
        sr.Dispose();

        Debug.Log("读取完成");

        return GameDataByJson;

    }

    public static void WriteFile<T>(string fileName,T GameDataToJson)
    {
        string str = JsonUtility.ToJson(GameDataToJson);

        FileStream fs = new FileStream(GetFilePath(fileName), FileMode.Create);
        byte[] bytes = new UTF8Encoding().GetBytes(str.ToString());
        fs.Write(bytes, 0, bytes.Length);
        fs.Close();
        Debug.Log("写入完成");
    }

}
