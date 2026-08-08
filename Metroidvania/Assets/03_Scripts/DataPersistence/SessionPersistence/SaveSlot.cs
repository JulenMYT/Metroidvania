using System;
using System.IO;
using TMPro;
using UnityEngine;

public class SaveSlot : MonoBehaviour
{
    [SerializeField] private int slotIndex;
    [SerializeField] private TMP_Text roomText;
    [SerializeField] private TMP_Text dateText;

    private void Start()
    {
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        string path = Path.Combine(Application.persistentDataPath, $"Slot {slotIndex}.json");

        if (!File.Exists(path))
        {
            roomText.text = "Empty";
            dateText.text = "";
            return;
        }

        string json = File.ReadAllText(path);
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        roomText.text = data.lastSceneName;
        DateTime saveDate = DateTime.FromBinary(long.Parse(data.lastSaveTime));
        dateText.text = saveDate.ToString("MMM d * h:mm tt");
    }
}
