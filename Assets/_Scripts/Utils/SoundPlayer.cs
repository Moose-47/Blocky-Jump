using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (!audioSource.isPlaying) Destroy(gameObject);
    }
}
