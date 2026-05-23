using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para cambiar de escena

public partial class MainMenu : MonoBehaviour
{
    [Header("Paneles")]
    public GameObject mainMenuPanel;
    public GameObject controlsPanel;
    public GameObject creditsPanel;
    public GameObject popUpPanel;
    public GameObject exitPanel;


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
        // ELIMINADO: mainMenuPanel.SetActive(false); 
        controlsPanel.SetActive(true);
    }

    public void OpenCredits()
    {
        // ELIMINADO: mainMenuPanel.SetActive(false);
        creditsPanel.SetActive(true);
    }

    public void OpenExit()
    {
        exitPanel.SetActive(true);
    }

    public void BackToMenu()
    {
        // Simplemente cerramos las ventanas, el menú principal ya está ahí
        controlsPanel.SetActive(false);
        creditsPanel.SetActive(false);
    }
    // Método para cerrar los Controles
    public void CloseControls()
    {
        controlsPanel.SetActive(false);
    }

    // Método para cerrar los Créditos
    public void CloseCredits()
    {
        creditsPanel.SetActive(false);
    }

    public void CloseExit()
    {
        exitPanel.SetActive(false);
    }

    public void QuitGame()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
}