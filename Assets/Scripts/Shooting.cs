using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public AudioSource shootingAudioSource;

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

            shootingAudioSource.Play();

            Vector3 spawnerPosition = bulletSpawner.position;
            
            // get the pre-instantiated projectile and set it up
            GameObject projectile = ProjectilePooler.SharedInstance.GetPooledObject();
            if (projectile != null) {
                projectile.transform.position = spawnerPosition;
                projectile.transform.rotation = bulletSpawner.rotation;
                projectile.SetActive(true);
            }
            projectile.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, 0, shootingPower));
            
            // TODO: Compare the performence between new and old way of projectile spawning
            // Rigidbody projectileRb = Instantiate(projectilePrefab, spawnerPosition, bulletSpawner.rotation) as Rigidbody;
            // projectileRb.AddRelativeForce(new Vector3(0, 0, shootingPower));

            canShoot = false;
        }
    }

    IEnumerator Reload() {
        isReloading = true;
        yield return new WaitForSeconds(reloadTime);
        isReloading = false;
    }


}
