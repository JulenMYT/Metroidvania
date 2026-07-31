using System.Collections;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomTransitionManager : MonoBehaviour
{
    [SerializeField] private ScreenFader screenFader;
    [SerializeField] private CameraManager camManager;

    private string currentRoom = "";

    void Start()
    {
        EnterRoom("", "");
    }

    public void EnterRoom(string sceneName, string spawnID)
    {
        StartCoroutine(Transition(sceneName, spawnID));
    }

    private IEnumerator Transition(string sceneName, string spawnID)
    {   
        yield return screenFader.Fade(0f, 1f, 0.5f);
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
        SetupRoom(spawnID);
        SetupCameraConfiner();
        ResetParallax();

        yield return new WaitForSeconds(0.5f);
        yield return screenFader.Fade(1f, 0f, 1f);
    }

    private void SetupRoom(string spawnID)
    {
        SpawnPoint[] spawns = FindObjectsByType<SpawnPoint>(FindObjectsSortMode.None);
        SpawnPoint spawnToUse = spawns[0];

        if (!string.IsNullOrEmpty(spawnID))
        {
            foreach (SpawnPoint spawn in spawns)
            {
                if (spawn.SpawnID == spawnID)
                {
                    spawnToUse = spawn;
                    transform.position = spawn.transform.position;
                    break;
                }
            }
        }
    }

    private void SetupCameraConfiner()
    {
        CameraConfinerProvider provider = FindFirstObjectByType<CameraConfinerProvider>();
        camManager.SetConfiner(provider.confiner);
    }

    private void ResetParallax()
    {
        ParallaxManager parallax = FindFirstObjectByType<ParallaxManager>();
        if (parallax != null)
            parallax.Initialize(camManager.camTransform);
    }
}
