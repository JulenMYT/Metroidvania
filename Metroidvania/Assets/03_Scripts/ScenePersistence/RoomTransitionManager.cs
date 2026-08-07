using System.Collections;
using Unity.Android.Gradle.Manifest;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomTransitionManager : MonoBehaviour
{
    [SerializeField] private ScreenFader screenFader;
    [SerializeField] private CameraManager camManager;

    private string currentRoom = "";
    private bool isTransitioning;

    void Start()
    {
        EnterRoom("", "");
    }

    public void EnterRoom(string sceneName, string spawnID)
    {
        if (isTransitioning)
            return;

        StartCoroutine(Transition(sceneName, spawnID));
    }

    private IEnumerator Transition(string sceneName, string spawnID)
    {   
        isTransitioning = true;    //Lock

        Player player = ServiceLocator.Get<Player>();
        player.isControlLocked = true;

        if (!string.IsNullOrEmpty(spawnID))
        {
            yield return screenFader.Fade(0f, 1f, 0.5f);
        }

        if (!string.IsNullOrEmpty(currentRoom))
        {
            yield return SceneManager.UnloadSceneAsync(currentRoom);
            yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        }

        Scene newScene = SceneManager.GetSceneByName(sceneName);

        if (newScene.IsValid())
        {
            SceneManager.SetActiveScene(newScene);
        }

        currentRoom = SceneManager.GetActiveScene().name;

        yield return null;
        RoomService service = ServiceLocator.Get<RoomService>();

        SetupRoom(service, spawnID);
        SetupCameraConfiner(service);

        ServiceLocator.Get<AudioManager>().PlayMusic(service.roomMusic);

        isTransitioning = false;
        player.isControlLocked = false;

        yield return new WaitForSeconds(0.8f);
        ResetParallax(service);

        if (!string.IsNullOrEmpty(spawnID))
        {
            yield return screenFader.Fade(1f, 0f, 0.5f);
        }
    }

    private void SetupRoom(RoomService service, string spawnID)
    {
        if (string.IsNullOrEmpty(spawnID))
            return;

        SpawnPoint spawnToUse = service.GetSpawn(spawnID);

        if (spawnToUse != null)
        {
            transform.position = spawnToUse.transform.position;
        }
    }

    private void SetupCameraConfiner(RoomService service)
    {
        if (service != null && service.provider != null)
            camManager.SetConfiner(service.provider.confiner);
    }

    private void ResetParallax(RoomService service)
    {
        if (service != null && service.parallax != null)
            service.parallax.Initialize(camManager.camTransform);
    }
}
