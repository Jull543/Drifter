using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;    //文件输入与输出
using System.Runtime.Serialization.Formatters.Binary;   //将数据转化为二进制

public class GameSaveManager : MonoBehaviour
{
    public Inventory myinventory;   //背包

    public void SaveGame()
    {
        //输出文件夹路径
        Debug.Log(Application.persistentDataPath);
        //如果在游戏绝对路径下方，没有包含存储文件夹，创建文件夹
        if(!Directory.Exists(Application.persistentDataPath + "/game_SaveData"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/game_SaveData");
        }

        BinaryFormatter formatter = new BinaryFormatter();  //二进制转化

        FileStream file = File.Create(Application.persistentDataPath + "/game_SaveData/inventory.txt");

        var json = JsonUtility.ToJson(myinventory);

        formatter.Serialize(file, json);

        file.Close();
    }

    public void LoadGame()
    {
        BinaryFormatter bf = new BinaryFormatter();

        if(File.Exists(Application.persistentDataPath + "/game_SaveData/inventory.txt"))
        {
            FileStream file = File.Open(Application.persistentDataPath + "/game_SaveData/inventory.txt", FileMode.Open);

            JsonUtility.FromJsonOverwrite((string)bf.Deserialize(file), myinventory);

            file.Close();
        }

    }

}
