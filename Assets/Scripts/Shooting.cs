using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Transform bulletSpawner;
    public Rigidbody projectilePrefab;
    public float shootingPower = 100_000f;
    public float reloadTime = 4.0f;

    private bool isReloading = false;
    private bool isShootButtonPressed = false;

    void Update()
    {

        if (Input.GetMouseButtonDown(0)) {
            isShootButtonPressed = true;
        }

    }

    private void FixedUpdate() {
        if (isReloading) return;
        Shoot();
    }

    private void Shoot() {
        if (isShootButtonPressed) {
            Vector3 position = bulletSpawner.position;
            Rigidbody projectileRb = Instantiate(projectilePrefab, position, bulletSpawner.rotation) as Rigidbody;
            projectileRb.AddRelativeForce(new Vector3(0, 0, shootingPower));

            StartCoroutine(Reload());
            isShootButtonPressed = false;
        }
    }

    IEnumerator Reload() {
        isReloading = true;
        yield return new WaitForSeconds(reloadTime);

        isReloading = false;
    }


}
