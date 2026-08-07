using UnityEngine;

public class SoundProxy : MonoBehaviour
{
    [SerializeField] private AudioData walkSound;
    [SerializeField] private AudioData slideSound;
    [SerializeField] private AudioData landSound;

    private AudioManager audioManager;

    private void Start() => audioManager = ServiceLocator.Get<AudioManager>();

    public void PlayWalkSound() => audioManager.PlaySFX(walkSound);
    public void PlaySlideSound() => audioManager.PlaySFX(slideSound);
    public void PlayLandSound() => audioManager.PlaySFX(landSound);

}
