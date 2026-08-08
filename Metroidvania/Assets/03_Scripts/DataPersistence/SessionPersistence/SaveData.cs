using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    //PLAYER PERSISTENCE
    public Vector2 playerPosition;
    public int playerHealth;

    public List<string> learnedSpells = new();
    public int selectedSpell;

    //PROGRESSION
    public int availableStatPoins;

    public int power;
    public int vitality;
    public int focus;
    public int agility;

    //WORLD STATE
    public List<string> defeatedEnemies = new();
    public List<string> openedChest = new();
    public List<string> collectedLoot = new();

    //SCENE DATA
    public string lastSceneName;
    public string lastSaveTime;
}
