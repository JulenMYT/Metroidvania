using System.Collections.Generic;
using UnityEngine;

public class WorldState : MonoBehaviour, IDataPersistence
{
    public HashSet<string> collectedLoot = new();
    public HashSet<string> openedChests = new();
    public HashSet<string> defeatedEnemies = new();

    private SaveManager saveManager;

    private void Awake() => ServiceLocator.Register<WorldState>(this);

    private void Start()
    {
        saveManager = ServiceLocator.Get<SaveManager>();
        saveManager.Register(this);
    }

    public void SaveData(SaveData saveData)
    {
        saveData.defeatedEnemies = new List<string>(defeatedEnemies);
        saveData.openedChest = new List<string>(openedChests);
        saveData.collectedLoot = new List<string>(collectedLoot);
    }

    public void LoadData(SaveData saveData)
    {
        defeatedEnemies = new HashSet<string>(saveData.defeatedEnemies);
        openedChests = new HashSet<string>(saveData .openedChest);
        collectedLoot = new HashSet<string>(saveData.collectedLoot);
    }
}
