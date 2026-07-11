using UnityEngine;
using UnityEngine.SceneManagement;

public class WinMenuManager : MonoBehaviour
{
    private void Start()
    {
        SetCursorVisible();
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(nameof(GameScenes.SCN_Office_LVL));
    }
    public void QuitGame() => Application.Quit();

    private void SetCursorVisible()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
