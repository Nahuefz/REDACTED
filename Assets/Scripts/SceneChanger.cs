using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para gestionar escenas

public class SceneChanger : MonoBehaviour
{
    [SerializeField] private string Escena;

    private void OnTriggerEnter(Collider other)
    {
        // Verificamos que el objeto que entró tenga el Tag "Player"
        if (other.CompareTag("Player"))
        {
            // Cargamos la siguiente escena
            SceneManager.LoadScene(Escena);
        }
    }
}