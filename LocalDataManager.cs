using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LocalDataManager : MonoBehaviour
{
    private static LocalDataManager instance;

    private static List<LocalSaveData> saveDatas = new List<LocalSaveData>();
    
    public static void addLocalSaveData(LocalSaveData data) {
        LocalDataManager.saveDatas.Add(data);
    }
    
    private void Start() {
        if (LocalDataManager.instance != null) {
            Debug.LogError("Multiple instances of LocalDataManager exist, remove all but one");
            return;
        }
        
        LocalDataManager.instance = this;
    }

    private void OnApplicationQuit() {
        if (LocalDataManager.instance != this) return; // Double check we are the correct instance

        foreach (LocalSaveData localSaveData in LocalDataManager.saveDatas) {
            localSaveData.write();
        }
    }
}