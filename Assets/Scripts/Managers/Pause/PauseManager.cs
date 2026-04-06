using System;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    private bool isPaused = false;
    private PlayerInputs pauseInput;

    private void Awake()
    {
        pauseInput = new PlayerInputs();
        pauseInput.Player.
    }

    private void Update()
    {
       Pause();
       Resume();
    }

    #region Metodos de pausa
    /// <summary>
    /// Este metodo cambia la escala interna del tiempo a 0, simulando una "pausa".
    /// </summary>
    void Pause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 0;
        }
    }
    
    /// <summary>
    /// Este metodo cambia la escala interna del tiempo a 1, solo si la escala del tiempo es 0.
    /// </summary>
    void Resume()
    {
        if (!isPaused && Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 1; 
        }
    }
    
    #endregion
}