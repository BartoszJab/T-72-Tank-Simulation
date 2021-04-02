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
        
        //shootingSmokeParticle.Emit(emitOverride, 500);

        GameObject explosion = ObjectPooler.SharedInstance.GetPooledObject("ExplosionEffect");
        if (explosion != null) {
            ParticleSystem.EmitParams emitOverride = new ParticleSystem.EmitParams();
            emitOverride.startLifetime = 4f;
            explosion.transform.position = collision.GetContact(0).point;
            explosion.transform.rotation = Quaternion.identity;
            explosion.SetActive(true);
            explosion.GetComponent<ParticleSystem>().Emit(emitOverride, 200);
        }

        gameObject.SetActive(false);
    }
}
