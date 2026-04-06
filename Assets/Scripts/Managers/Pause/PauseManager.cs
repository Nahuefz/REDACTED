using UnityEngine;

public class PauseManager : MonoBehaviour
{
    private bool isPaused = false;
    private PlayerInputs _pauseInputs;

    private void Awake()
    {
        _pauseInputs = new PlayerInputs();
        _pauseInputs.Enable();
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
        _pauseInputs.Player.Enable();
    }
    #endregion
}