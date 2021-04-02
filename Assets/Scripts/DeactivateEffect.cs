using UnityEngine;

public class DeactivateEffect : MonoBehaviour
{
    private ParticleSystem particle;
    private AudioSource audioSource;

    private void Start() {
        particle = GetComponent<ParticleSystem>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!audioSource.isPlaying) {
            gameObject.SetActive(false);
        }
    }
}
