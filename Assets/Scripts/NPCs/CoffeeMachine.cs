using UnityEngine;

public class CoffeeMachine : MonoBehaviour, IInteractable
{
    [SerializeField] private OutlineTesting outline;
    [SerializeField] private GameObject coffeePrefab;
    [SerializeField] private GameObject cheesePrefab;
    [SerializeField] private Transform spawnPoint;

    public void Interact()
    {
        Debug.Log(" INTERACCION Coffee Machine");
        // Random.Range(1, 11) devuelve un entero entre 1 y 10 (el máximo es exclusivo para enteros)
        int randomValue = Random.Range(1, 11); 
        SpawnInteractable(randomValue);
    }

    void SpawnInteractable(int randomNumber)
    {
        // 1-5 (50%) -> Café, 6-10 (50%) -> Queso
        GameObject prefabToSpawn = (randomNumber <= 5) ? coffeePrefab : cheesePrefab;

        if (prefabToSpawn != null && spawnPoint != null)
        {
            Instantiate(prefabToSpawn, spawnPoint.position, spawnPoint.rotation);
        }
        else
        {
            Debug.LogWarning("CoffeeMachine: Asegúrate de asignar los prefabs y el spawnPoint en el Inspector.");
        }
    }
}
