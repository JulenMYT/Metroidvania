using UnityEngine;

public class MusicTrigger : MonoBehaviour
{
    [SerializeField] private AudioData musicData;
    
    private bool hasPlayed;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasPlayed)
            return;

        if (!collision.CompareTag("Player"))
            return;

        AudioManager audio = ServiceLocator.Get<AudioManager>();

        audio.PlayMusic(musicData);

        hasPlayed = true;
    }
}
