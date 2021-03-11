using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Transform bulletSpawner;
    public Rigidbody projectilePrefab;
    public float shootingPower = 100_000f;

    private bool isShooting = false;

    void Start()
    {

    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            Debug.Log("mouse btn pressed");
            isShooting = true;
        }

    }

    private void FixedUpdate() {
        Shoot();
    }

    private void Shoot() {
        if (isShooting) {
            Vector3 position = bulletSpawner.position;
            Rigidbody projectileRb = Instantiate(projectilePrefab, position, bulletSpawner.rotation) as Rigidbody;
            projectileRb.AddRelativeForce(new Vector3(0, 0, shootingPower));
            
            isShooting = false;
        }
    }


}
