using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static event Action OnSaveDataLoaded;

    private readonly List<IDataPersistence> persistenceObjects = new();
    private string SavePath => Path.Combine(Application.persistentDataPath, $"Slot {LoadSession.SelectedSlot}.json");

    private void Awake()
    {
        ServiceLocator.Register<SaveManager>(this);
    }

    private void Start()
    {
        StartCoroutine(LoadAfterInit());   
    }

    private IEnumerator LoadAfterInit()
    {
        yield return null;
        LoadGame();
    }

    public void Register(IDataPersistence persistenceObject)
    {
        persistenceObjects.Add(persistenceObject);
    }

    public void SaveGame()
    {
        SaveData saveData = new();

        foreach (var obj in persistenceObjects)
        {
            obj.SaveData(saveData);
        }

        saveData.lastSaveTime = DateTime.Now.ToBinary().ToString();

        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(SavePath, json);
    }

    public void LoadGame()
    {
        if (!File.Exists(SavePath))
            return;

        string json = File.ReadAllText(SavePath);
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        foreach (var obj in persistenceObjects)
        {
            obj.LoadData(data);
        }

        OnSaveDataLoaded?.Invoke();
    }

    private void OnApplicationQuit()
    {
        if (LoadSession.SelectedSlot < 0)
            return;
        SaveGame();
    }
}
