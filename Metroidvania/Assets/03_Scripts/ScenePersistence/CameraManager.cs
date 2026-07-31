using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private CinemachineCamera vCam;
    public Transform camTransform;

    private CinemachineConfiner2D confiner;

    private void Awake()
    {
        confiner = vCam.GetComponent<CinemachineConfiner2D>();
    }

    public void SetConfiner(Collider2D newConfiner)
    {
        confiner.BoundingShape2D = newConfiner;
    }
}
