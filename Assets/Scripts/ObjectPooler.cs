using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class ObjectPoolItem {
    // object to create in pool
    public GameObject objectToPool;
    // how many objects to create in pool
    public int amountToPool;
    // if pool should be expandable
    public bool shouldExpand;
}

public class ObjectPooler : MonoBehaviour {
    public static ObjectPooler SharedInstance;
    public List<ObjectPoolItem> itemsToPool;
    public List<GameObject> pooledObjects;

    void Awake() {
        SharedInstance = this;
    }

    void Start() {
        pooledObjects = new List<GameObject>();
        GameObject temp;
        foreach (ObjectPoolItem item in itemsToPool) {
            for (int i = 0; i < item.amountToPool; i++) {
                temp = Instantiate(item.objectToPool);
                temp.SetActive(false);
                pooledObjects.Add(temp);
            }
        }
    }

    public GameObject GetPooledObject(string tag) {
        for (int i = 0; i < pooledObjects.Count; i++) {
            if (!pooledObjects[i].activeInHierarchy && pooledObjects[i].tag == tag) {
                if (tag == "Projectile")
                    pooledObjects[i].GetComponent<Rigidbody>().velocity = Vector3.zero;
                return pooledObjects[i];
            }
        }
        foreach (ObjectPoolItem item in itemsToPool) {
            if (item.objectToPool.tag == tag) {
                if (item.shouldExpand) {
                    GameObject temp = Instantiate(item.objectToPool);
                    temp.SetActive(false);
                    pooledObjects.Add(temp);
                    return temp;
                }
            }
        }
        return null;
    }
}
