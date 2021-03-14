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
    private bool canShoot = false;

    void Update()
    {

        if (Input.GetMouseButtonDown(0) && !isReloading) {
            canShoot = true;
        }
        Debug.Log("isreloading: " + isReloading);
    }

    private void FixedUpdate() {
        Shoot();
    }

    private void Shoot() {
        if (canShoot) {
            StartCoroutine(Reload());

            Vector3 position = bulletSpawner.position;
            Rigidbody projectileRb = Instantiate(projectilePrefab, position, bulletSpawner.rotation) as Rigidbody;
            projectileRb.AddRelativeForce(new Vector3(0, 0, shootingPower));

            canShoot = false;
        }
    }

    IEnumerator Reload() {
        isReloading = true;
        yield return new WaitForSeconds(reloadTime);
        isReloading = false;
    }


}
