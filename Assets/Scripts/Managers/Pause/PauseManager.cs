using UnityEngine;

public class PauseManager : MonoBehaviour
{
    private bool isPaused = false;
    private PlayerInputs _pauseInputs;
    [SerializeField] private GameObject _pauseCanvas;

    private void Awake()
    {
        _pauseInputs = new PlayerInputs();
        _pauseInputs.Enable();
        _pauseCanvas =  GameObject.Find("PauseCanvas");
        _pauseCanvas.SetActive(false);
    }

    private void Update()
    {
        if (_pauseInputs.Player.Pause.WasPressedThisFrame())
        {
            TogglePause();
            
        }
    }

    #region Metodos de pausa
    /// <summary>
    /// Al llamar al metodo cambia el timeScale de 0 a 1, o viceversa.
    /// Tambien desactiva (timeScale=0) los inputs, y reanuda (timeScale=1)
    /// </summary>
    private void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0 : 1;
        Debug.Log($"PAUSA = <color=yellow>{isPaused}</color>");

        switch (isPaused)
        {
            case  true: 
                _pauseCanvas.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                break;
            case  false: 
                _pauseCanvas.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                break;
        }
    }
    /// <summary>
    /// Metodo de "despausa" para el menu de pausa
    /// </summary>
    public void Resume()
    {
        TogglePause();
        //_pauseCanvas.SetActive(false);
    }
    #endregion

    public void Quit()
    {
        Debug.Log("<color=yellow>IMPLEMENTAR</color>");
    }
}