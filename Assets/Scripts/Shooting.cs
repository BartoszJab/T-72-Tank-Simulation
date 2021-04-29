using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shooting : MonoBehaviour
{
    public AudioSource shootingAudioSource;

    public Image crosshair;
    private float currentReloadTime = 0f;

    public Transform bulletSpawner;
    public Rigidbody projectilePrefab;
    public float shootingPower = 100_000f;
    public float reloadTime = 4.0f;

    public ParticleSystem shootingSmokeParticle;

    public Vector3 recoilForce;
    public float distanceFromCenter = 5f;

    private bool isReloading = false;
    private bool canShoot = false;

    void Update()
    {
        if (isReloading) {
            currentReloadTime += Time.deltaTime;
            crosshair.fillAmount = currentReloadTime / reloadTime;
        }
        
        if (Input.GetMouseButtonDown(0) && !isReloading) {
            canShoot = true;
        }
    }

    private void FixedUpdate() {
        Shoot();
    }

    private void Shoot() {
        if (canShoot) {
            StartCoroutine(Reload());

            ParticleSystem.EmitParams emitOverride = new ParticleSystem.EmitParams();
            emitOverride.startLifetime = reloadTime;
            shootingSmokeParticle.Emit(emitOverride, 500);

            shootingAudioSource.Play();

            Vector3 spawnerPosition = bulletSpawner.position;
            
            // get the pre-instantiated projectile and set it up
            GameObject projectile = ObjectPooler.SharedInstance.GetPooledObject("Projectile");
            if (projectile != null) {
                projectile.transform.position = spawnerPosition;
                projectile.transform.rotation = bulletSpawner.rotation;
                projectile.SetActive(true);
            }
            projectile.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, 0, shootingPower));

            // TODO: Compare the performence between new and old way of projectile spawning
            // Rigidbody projectileRb = Instantiate(projectilePrefab, spawnerPosition, bulletSpawner.rotation) as Rigidbody;
            // projectileRb.AddRelativeForce(new Vector3(0, 0, shootingPower));

            AddRecoil();

            canShoot = false;
        }
    }

    void AddRecoil() {
        Vector3 rifleLookVector = bulletSpawner.transform.forward;
        rifleLookVector *= distanceFromCenter;
        rifleLookVector.y = 0f;
        GetComponent<Rigidbody>().AddForceAtPosition(recoilForce, transform.position + rifleLookVector);
    }

    IEnumerator Reload() {
        crosshair.color = Color.red;
        currentReloadTime = 0f;
        isReloading = true;
        yield return new WaitForSeconds(reloadTime);
        isReloading = false;
        crosshair.color = Color.black;
    }


}
