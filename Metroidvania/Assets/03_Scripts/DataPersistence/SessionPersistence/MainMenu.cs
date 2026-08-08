using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button[] saveSlots;
    [SerializeField] private string firstLevel = "Room1";

    private bool isNewGame;

    private void Start()
    {
        for (int i = 0; i < saveSlots.Length; i++)
        {
            saveSlots[i].interactable = SlotExists(i);
        }
    }

    public void OnNewGameClicked()
    {
        isNewGame = true;

        foreach (Button slot in saveSlots)
        {
            slot.interactable = true;
        }
    }

    public void OnSaveSlotClicked(int slot)
    {
        LoadSession.SelectedSlot = slot;
        if (isNewGame)
        {
            DeleteSlot(slot);
            LoadSession.IsNewGame = true;
            SceneManager.LoadScene(firstLevel);
        }
        else
        {
            LoadSession.IsNewGame = false;
            string json = File.ReadAllText(GetSavePath(slot));
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            SceneManager.LoadScene(data.lastSceneName);
        }
    }

    private bool SlotExists(int slot)
    {
        string path = GetSavePath(slot);
        return File.Exists(path);
    }

    private void DeleteSlot(int slot)
    {
        string path = GetSavePath(slot);
        if (File.Exists(path))
            File.Delete(path);
    }

    private string GetSavePath(int slot) => Path.Combine(Application.persistentDataPath, $"Slot {slot}.json");
}
