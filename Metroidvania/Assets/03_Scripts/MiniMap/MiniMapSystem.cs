using System.Collections.Generic;
using UnityEngine;

public class MiniMapSystem : MonoBehaviour
{
    [SerializeField] private List<MiniMapRoom> rooms;
    [SerializeField] private RectTransform playerIcon;
    private Dictionary<string, MiniMapRoom> roomData;
    private HashSet<string> visitedRooms = new();

    private void Awake()
    {
        ServiceLocator.Register<MiniMapSystem>(this);

        roomData = new Dictionary<string, MiniMapRoom>();

        foreach (MiniMapRoom room in rooms)
        {
            roomData.Add(room.name, room);
        }

        playerIcon.gameObject.SetActive(false);
    }

    public void MapRooms(List<string> roomNames)
    {
        foreach (string name in roomNames)
        {
            if (!roomData.TryGetValue(name, out var room))
            {
                continue;
            }

            if (visitedRooms.Contains(name))
            {
                room.SetState(RoomState.Visited);
            }
            else
            {
                room.SetState(RoomState.Mapped);
            }
        }
    }

    public void VisitRoom(string roomName)
    {
        visitedRooms.Add(roomName);

        if (!roomData.TryGetValue(roomName, out var room))
        {
            Debug.LogWarning($"Room '{roomName}' not found.");
            return;
        }

        if (room.State == RoomState.Mapped)
        {
            room.SetState(RoomState.Visited);
        }

        playerIcon.gameObject.SetActive(true);
        playerIcon.localPosition = room.transform.localPosition;
    }
}
