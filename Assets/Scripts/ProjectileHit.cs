using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileHit : MonoBehaviour
{
    public GameObject explosion;

    private void Update() {
        if (transform.position.y < 0) {
            gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter(Collision collision) {
        ParticleSystem.EmitParams emitOverride = new ParticleSystem.EmitParams();
        emitOverride.startLifetime = 4f;
        //shootingSmokeParticle.Emit(emitOverride, 500);

        GameObject explosionParticle = Instantiate(explosion, collision.GetContact(0).point, Quaternion.identity);
        explosion.GetComponent<ParticleSystem>().Emit(emitOverride, 200);
        gameObject.SetActive(false);
    }
}
