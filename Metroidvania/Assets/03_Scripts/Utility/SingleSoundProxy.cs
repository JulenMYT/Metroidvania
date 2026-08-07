using UnityEngine;

public class SingleSoundProxy : MonoBehaviour
{
    [SerializeField] private AudioData sound;

    public void PlaySound() => ServiceLocator.Get<AudioManager>().PlaySFX(sound);
}
