using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePooler : MonoBehaviour
{
    public static ProjectilePooler SharedInstance;
    public List<GameObject> pooledObjects;
    // prefab to pre-instantiate
    public GameObject projectileToPool;
    // amount of objects to pre-instantiate
    public int amountToPool;

    private void Awake() {
        SharedInstance = this;
    }

    // setup pre-instantiated objects
    void Start()
    {
        pooledObjects = new List<GameObject>();
        GameObject temporary;

        for (int i = 0; i < amountToPool; i++) {
            temporary = Instantiate(projectileToPool);
            temporary.SetActive(false);
            pooledObjects.Add(temporary);
        }
    }
    
    // get the first inactive object if exists, return null otherwise
    public GameObject GetPooledObject() {
        foreach (GameObject pooledObject in pooledObjects) {
            if (!pooledObject.activeInHierarchy) {
                // reset given force of pooled object so it's not increased when spawned again
                pooledObject.GetComponent<Rigidbody>().velocity = Vector3.zero; 
                return pooledObject;
            }
        }
        return null;
    }

}
