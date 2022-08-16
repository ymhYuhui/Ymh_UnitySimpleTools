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
        filePath = Application.streamingAssetsPath + "/MatchDatas/" + fileName + ".json";

        Debug.LogError("filePath" + filePath);
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

        TextAsset t = (TextAsset)Resources.Load("MatchDatas/" + fileName);
        string json = t.text.ToString().Trim();

        T GameDataByJson = JsonUtility.FromJson<T>(json);

        Debug.LogError("读取完成");

        return GameDataByJson;

    }

    public static void WriteFile<T>(string fileName, T GameDataToJson)
    {
        string str = JsonUtility.ToJson(GameDataToJson);

        FileStream fs = new FileStream(GetFilePath(fileName), FileMode.Create);
        byte[] bytes = new UTF8Encoding().GetBytes(str.ToString());
        fs.Write(bytes, 0, bytes.Length);
        fs.Close();
        Debug.LogError("写入完成");
    }

}
