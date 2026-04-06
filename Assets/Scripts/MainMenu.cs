using UnityEngine;
using UnityEngine.SceneManagement; 

public partial class MainMenu : MonoBehaviour
{
    [Header("Paneles")]
    public GameObject mainMenuPanel;
    public GameObject controlsPanel;

    public void PlayGame()
    {
        // Carga la siguiente escena en la lista de Build Settings
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OpenControls()
    {
        mainMenuPanel.SetActive(false);
        controlsPanel.SetActive(true);
    }

    public void BackToMenu()
    {
        controlsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void QuitGame()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
}