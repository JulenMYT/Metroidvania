using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "Collectibles/Map")]
public class MapCollectibleSO : CollectibleSO
{
    public List<string> roomsToReveal;

    public override void Collect(Player player)
    {
        MiniMapSystem miniMap = ServiceLocator.Get<MiniMapSystem>();
        miniMap.MapRooms(roomsToReveal);
        miniMap.VisitRoom(SceneManager.GetActiveScene().name);
    }
}
