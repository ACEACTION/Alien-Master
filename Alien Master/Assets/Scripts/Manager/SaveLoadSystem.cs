using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveLoadSystem 
{
    static void Save<T>(T data, string path, Action OnSuccess, Action<Exception> OnError)
    {
        
        try
        {
            BinaryFormatter binary = new BinaryFormatter();
            Stream stream = new FileStream(path, FileMode.Create);
            binary.Serialize(stream, data);
            OnSuccess();
            stream.Dispose();
        }
        catch (Exception e)
        {
            OnError(e);
        }
        
    }

    static void Load<T>(Action<T> LoadData, string path, Action OnSuccess, Action<Exception> OnError)
    {

        try 
        {
            BinaryFormatter binary = new BinaryFormatter();
            Stream stream = File.OpenRead(path);
            LoadData((T)binary.Deserialize(stream));
            OnSuccess();
            stream.Dispose();
        }
        catch(Exception e)
        {
            OnError(e);
        }
    }


    public static void SaveData<T>(T data, string path)
    {
        path = string.Concat(Application.persistentDataPath, path);

        Save(data, path,
            () =>
            {
                Debug.Log("data saved!");
            }, (Exception error) =>
            {
                Debug.Log(error.Message);
            }
        );
    }


    public static T LoadData<T>(string path)
    {
        T data = default;
        path = string.Concat(Application.persistentDataPath, path);

        Load((T d) =>
        {
            data = d;
        }, path,
        () =>
        {
            Debug.Log("data loaded!");
        }, (Exception error) =>
        {
            Debug.Log(error.Message);
            //throw error;
        });

        return (T)Convert.ChangeType(data, typeof(T));
    }

}
