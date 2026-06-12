using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] private GameScenes EscenaDestino;
    [SerializeField] private string spawnIDDestino;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!string.IsNullOrEmpty(spawnIDDestino))
            {
                PlayerPrefs.SetString("NextSpawnID", spawnIDDestino);
                PlayerPrefs.Save();
            }

            ChangeScene(EscenaDestino);
        }
    }

    public void ChangeScene(GameScenes scene)
    {
        StartCoroutine(ChangeSceneRoutine(scene.ToString()));
    }

    private IEnumerator ChangeSceneRoutine(string sceneName)
    {
        // Buscamos el componente en la escena actual
        TransitionFade fade = Object.FindFirstObjectByType<TransitionFade>();

        if (fade != null)
        {
            // Esperamos a que la pantalla se ponga negra
            yield return StartCoroutine(fade.FadeToBlack());
        }

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
