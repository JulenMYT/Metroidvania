using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomService : MonoBehaviour
{
    public CameraConfinerProvider provider;
    public ParallaxManager parallax;
    public List<SpawnPoint> spawns;
    public AudioData roomMusic;

    private void Awake()
    {
        ServiceLocator.Register<RoomService>(this);
    }

    private void Start()
    {
        DiscoverRoom();
    }

    public SpawnPoint GetSpawn(string id)
    {
        return spawns.Find(spawn => spawn.SpawnID == id);
    }

    private void DiscoverRoom()
    {
        ServiceLocator.Get<MiniMapSystem>().VisitRoom(SceneManager.GetActiveScene().name);
    }
}
