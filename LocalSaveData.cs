using System;
using System.IO;using System.Net.Security;
using System.Runtime.Serialization.Formatters.Binary;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public abstract class LocalSaveData
{
    private string path;

    protected LocalSaveData() {
        LocalDataManager.addLocalSaveData(this);
    }
    
    /// <param name="filepath">The place to save the data. Must not end with a slash</param>
    /// <exception cref="ArgumentException">If filepath points to a directory instead of a file</exception>
    protected LocalSaveData(string filepath) : this() {
        this.setPath(filepath);
    }
    
    public void write() {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(this.path, FileMode.Create);
        
        formatter.Serialize(stream, this);
        
        stream.Close();
    }

    private static string transformPath(string path) {
        return Application.persistentDataPath + "/" + path.TrimStart('/');
    }

    public void setPath(string filepath) {
        if (filepath.EndsWith("/")) {
            throw new ArgumentException("Filepath must point to a file, not directory");
        }

        this.path = LocalSaveData.transformPath(filepath);
    }
    
    public static T load<T>(string filepath) where T : LocalSaveData, new() {
        string path = LocalSaveData.transformPath(filepath);

        if (!File.Exists(path)) {
            T ret = new T();
            ret.setPath(filepath);
            return ret;
        }
        
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Open);

        T data = (T)formatter.Deserialize(stream);
        data.setPath(filepath);
        
        stream.Close();

        return data;
    }
}