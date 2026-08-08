using UnityEngine;

public class PlayerPersistence : MonoBehaviour, IDataPersistence
{
    [SerializeField] private Health health;
    [SerializeField] private Magic magic;


    private void Start()
    {
        SaveManager saveManager = ServiceLocator.Get<SaveManager>();
        saveManager.Register(this);
    }

    public void SaveData(SaveData saveData)
    {
        saveData.playerPosition = transform.position;
        saveData.playerHealth = health.health;

        foreach (SpellSO spell in magic.GetLearnedSpells())
        {
            saveData.learnedSpells.Add(spell.itemName);
        }

        saveData.selectedSpell = magic.GetSelectedSpellIndex();
    }

    public void LoadData(SaveData saveData)
    {
        transform.position = saveData.playerPosition;
        health.SetHealth(saveData.playerHealth);

        magic.LoadSpells(saveData.learnedSpells, saveData.selectedSpell);
    }
}
