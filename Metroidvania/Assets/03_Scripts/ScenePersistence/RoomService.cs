using System.Collections.Generic;
using UnityEngine;

public class RoomService : MonoBehaviour
{
    public CameraConfinerProvider provider;
    public ParallaxManager parallax;
    public List<SpawnPoint> spawns;

    private void Awake()
    {
        ServiceLocator.Register<RoomService>(this);
    }

    public SpawnPoint GetSpawn(string id)
    {
        return spawns.Find(spawn => spawn.SpawnID == id);
    }
}
