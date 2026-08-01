using System.Collections.Generic;
using UnityEngine;

public class WorldState : MonoBehaviour
{
    public HashSet<string> collectedLoot = new();
    public HashSet<string> openedChests = new();
    public HashSet<string> defeatedEnemies = new();

    private void Awake() => ServiceLocator.Register<WorldState>(this);
}
