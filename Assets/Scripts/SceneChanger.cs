using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para gestionar escenas

public class SceneChanger : MonoBehaviour
{
    [SerializeField] private GameScenes EscenaDestino;

    private void OnTriggerEnter(Collider other)
    {
        // Verificamos que el objeto que entr� tenga el Tag "Player"
        if (other.CompareTag("Player"))
        {
            // Cargamos la siguiente escena
            ChangeScene(EscenaDestino);
        }
    }

    void ChangeScene(GameScenes scene)
    {
        string sceneName = scene.ToString();
        
        // Cargamos la escena
        SceneManager.LoadScene(sceneName);
    }
}

public enum GameScenes
{
    SCN_Tunels,
    SCN_Office_LVL,
    SCN_MainMenu,
    SCN_Archive_LVL,
    TEST_Enemy,
    SCN_PUZZLE1
}