using UnityEngine;

public class PlayerInitialPosition : MonoBehaviour
{
    void Start()
    {
        // Leemos el ID que guardamos antes de cambiar de escena
        string spawnID = PlayerPrefs.GetString("NextSpawnID");

        // Buscamos el objeto que tenga ese nombre exacto en la nueva escena
        GameObject spawnPoint = GameObject.Find(spawnID);

        if (spawnPoint != null)
        {
            // Teletransportamos al personaje al punto exacto
            transform.position = spawnPoint.transform.position;
            transform.rotation = spawnPoint.transform.rotation;
        }
    }
}