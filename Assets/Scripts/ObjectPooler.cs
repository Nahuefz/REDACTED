using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler Instance; 

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int poolSize = 20;
    private List<GameObject> pooledObjects;

    private void Awake()
    {
        Instance = this;

        // Llena la caja de balas 
        pooledObjects = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(bulletPrefab);
            obj.SetActive(false);
            pooledObjects.Add(obj);
        }
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy) return pooledObjects[i];
        }
        return null; // Si no hay más balas, el arma no dispara
    }
}