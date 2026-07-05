using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para cambiar de escena

public partial class MainMenu : MonoBehaviour
{
    [Header("Paneles")]
    public GameObject mainMenuPanel;
    public GameObject controlsPanel;
    public GameObject creditsPanel;
    public GameObject popUpPanel;


    private void Start()
    {
        if (popUpPanel != null) popUpPanel.SetActive(false);
    }

    public void PlayGame()
    {
        // Carga la siguiente escena en la lista de Build Settings
        //SceneManager.LoadScene("SCN_Office_LVL");
        //SceneManager.LoadScene("TEST_Enemy");

        if (popUpPanel != null) popUpPanel.SetActive(true); 
    }

    public void ConfirmarMensaje()
    {
        if (popUpPanel != null) popUpPanel.SetActive(false);
        Debug.Log("Se acepto el mensaje, viene la animacion...");
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);

        
        if (DirectorInicio.Instance != null)
        {
            DirectorInicio.Instance.IniciarSecuenciaLevantarse();
        }
    }
    
    public void OpenControls()
    {
        mainMenuPanel.SetActive(false);
        controlsPanel.SetActive(true);
    }
    public void OpenCredits()
    {
        mainMenuPanel.SetActive(false);
        creditsPanel.SetActive(true);
    }
    public void BackToMenu()
    {
        controlsPanel.SetActive(false);
        creditsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void QuitGame()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
    public void TestLevel(string level)
    {
        SceneManager.LoadScene(level);
    }
}