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
   
        if (Instance == null)
        {
            Instance = this;
            // Esto hace que el Pool NO se destruya al cambiar de escena
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Si ya existe uno (porque volviste a una escena anterior), borramos el duplicado
            Destroy(gameObject);
            return;
        }

        LlenarPool();
    }

    private void LlenarPool()
    {
        pooledObjects = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(bulletPrefab);
            obj.SetActive(false);
            // Hacemos que las balas tambien sean hijas del objeto persistente
            obj.transform.SetParent(transform);
            pooledObjects.Add(obj);
        }
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy) return pooledObjects[i];
        }
        return null;
    }
}