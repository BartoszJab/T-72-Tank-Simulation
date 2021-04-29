using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructable : MonoBehaviour
{
    public GameObject fracturedBunker;

    private void OnCollisionEnter(Collision collision) {
        if (collision.collider.tag == "Projectile") {
            Instantiate(fracturedBunker, transform.position, transform.rotation);
            Destroy(gameObject);
            LevelManager.bunkersCount -= 1;
        }
    }
}
