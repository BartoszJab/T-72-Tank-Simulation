using UnityEngine;

public class DeactivateEffect : MonoBehaviour
{
    private ParticleSystem particle;

    private void Start() {
        particle = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if (!particle.isPlaying) {
            gameObject.SetActive(false);
        }
    }
}
