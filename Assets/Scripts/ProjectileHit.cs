using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileHit : MonoBehaviour
{
    private void Update() {
        if (transform.position.y < 0) {
            gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter(Collision collision) {
        gameObject.SetActive(false);
    }
}
